using System;
using System.Collections.Generic;
using System.Management;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace LiuLiuTools.Helpers
{
    /// <summary>
    ///Computer 的摘要说明
    /// </summary>
    public class NetWorkHelper
    {
        /// <summary>
        /// 根据Intel(R) Wi-Fi 6 AX200 160MHz返回对应IP
        /// 如果有多个IP在线,通过名字获取指定IP。那么问题又来了，如果一个网口下面有多个IP怎么办
        /// mo.Properties["IpAddress"].Value;这是一个String数组
        /// </summary>
        /// <param name="netName"></param>
        /// <returns>对应网口下有多少IP就有</returns>
        public static string[] GetIPAddress(string netName)
        {
            ManagementObjectCollection adapterConfigs = null;
            try
            {
                var query = new SelectQuery("SELECT * FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled = 'True'");
                using (var searcher = new ManagementObjectSearcher(query))
                {
                    adapterConfigs = searcher.Get();
                    foreach (ManagementObject config in adapterConfigs)
                    {
                        // 获取关联的 Win32_NetworkAdapter 对象以获取适配器名称
                        var adapterQuery = new RelatedObjectQuery(
                            $"Associators of {{Win32_NetworkAdapterConfiguration.Index='{config["Index"]}'}} " +
                            "WHERE ResultClass=Win32_NetworkAdapter");

                        using (var adapterSearcher = new ManagementObjectSearcher(adapterQuery))
                        using (var adapters = adapterSearcher.Get())
                        {
                            foreach (ManagementObject adapter in adapters)
                            {
                                string adapterName = adapter["Name"]?.ToString();
                                if (adapterName != null && adapterName.Equals(netName, StringComparison.OrdinalIgnoreCase))
                                {
                                    var ipAddresses = config["IpAddress"] as string[];
                                    return ipAddresses ?? new string[0];
                                }
                            }
                        }
                    }
                }
                return new string[0]; // 未找到匹配的适配器
            }
            catch (ManagementException ex)
            {
                Console.WriteLine($"WMI查询失败: {ex.Message}");
                return new string[] { "unknow" };
            }
            finally
            {
                adapterConfigs?.Dispose();
            }
        }

        /// <summary>
        /// 根据友好名称（如 "WLAN"）获取对应网络适配器的 IP 地址
        /// </summary>
        /// <param name="friendlyName">控制面板中显示的名称（如 "WLAN"），不区分大小写</param>
        /// <returns>IP 地址数组</returns>
        public static string[] GetIPByFriendlyName(string friendlyName)
        {
            try
            {
                // 1. 查询所有活动的网络适配器
                var adapterQuery = new ManagementObjectSearcher(
                    "SELECT * FROM Win32_NetworkAdapter " +
                    "WHERE NetConnectionStatus = 2 AND NetConnectionID IS NOT NULL"
                );

                foreach (ManagementObject adapter in adapterQuery.Get())
                {
                    // 2. 获取适配器的友好名称（如 "WLAN"）
                    string netConnectionID = adapter["NetConnectionID"]?.ToString();

                    //StringComparison.OrdinalIgnoreCase 不区分大小写
                    if (netConnectionID?.Equals(friendlyName, StringComparison.OrdinalIgnoreCase) == true)
                    {
                        // 3. 通过 Index 关联到 Win32_NetworkAdapterConfiguration
                        int adapterIndex = Convert.ToInt32(adapter["Index"]);
                        return GetIPByAdapterIndex(adapterIndex);
                    }
                }

                return new string[0];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"查询失败: {ex.Message}");
                return new string[] { "unknown" };
            }
        }


        /// <summary>
        /// 获取本机IP,最朴素的方式
        /// </summary>
        private void GetAllNetInterface(string friendlyName)
        {
            string adapter_IP = "";
            //获取所有网卡信息
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in nics)
            {
                string adapter_Name = adapter.Name.ToString();
                if (adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    //获取以太网卡网络接口信息
                    IPInterfaceProperties ip4 = adapter.GetIPProperties();
                    //获取单播地址集
                    UnicastIPAddressInformationCollection ipCollection = ip4.UnicastAddresses;
                    foreach (UnicastIPAddressInformation ipadd in ipCollection)//从接口集合中获取ip
                    {
                        //InterNetwork    IPV4地址      InterNetworkV6        IPV6地址
                        //Max            MAX 位址

                        //判断是否为MES/ipv4
                        if (adapter_Name.ToUpper().Contains(friendlyName) &&
                            ipadd.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            adapter_IP = ipadd.Address.ToString();//获取ip
                            string tempLogStr = adapter_Name + " IP= " + adapter_IP;
                            //Log.AddLog(tempLogStr);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 通过适配器索引获取 IP 地址
        /// </summary>
        private static string[] GetIPByAdapterIndex(int index)
        {
            var configQuery = new ManagementObjectSearcher(
                $"SELECT * FROM Win32_NetworkAdapterConfiguration " +
                $"WHERE Index = {index} AND IPEnabled = True"
            );

            foreach (ManagementObject config in configQuery.Get())
            {
                return (string[])config["IPAddress"] ?? new string[0];
            }

            return new string[0];
        }

        /// <summary>
        /// 获取所有活动适配器的友好名称列表（如 ["WLAN", "以太网"]）
        /// </summary>
        public static List<string> GetActiveAdapterNames()
        {
            var names = new List<string>();
            var query = new ManagementObjectSearcher(
                "SELECT NetConnectionID FROM Win32_NetworkAdapter " +
                "WHERE NetConnectionStatus = 2 AND NetConnectionID IS NOT NULL"
            );

            foreach (ManagementObject adapter in query.Get())
            {
                string name = adapter["NetConnectionID"].ToString();
                if (!string.IsNullOrEmpty(name)) names.Add(name);
            }

            return names;
        }

        /// <summary>
        /// 如果有多个IP在线，这种方法获取是不准的。
        /// </summary>
        /// <returns></returns>
        public string GetMacAddress()
        {
            try
            {
                //获取网卡硬件地址 
                string mac = "";
                //int count = 0;
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();//获取一个所有网口的集合，不知道为什么数字是18个
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        //count++;
                        mac = mo["MacAddress"].ToString();
                        break;
                    }
                }
                moc = null;
                mc = null;
                return mac;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }

        }

        /// <summary>
        /// 如果有多个IP在线，这种方法获取是不准的。
        /// </summary>
        /// <returns></returns>
        public string GetIPAddress()
        {
            try
            {
                //获取IP地址 
                string st = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();//获取到的排序是和上面Mac是一致的
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)//这个位置是取出IP在线的
                    {
                        //foreach (PropertyData item in mo.SystemProperties)
                        //{
                        //    SystemProperties是系统数据
                        //    string str = item.Name;
                        //}
                        //st=mo["IpAddress"].ToString(); 
                        System.Array ar;
                        ar = (System.Array)(mo.Properties["IpAddress"].Value);
                        st = ar.GetValue(0).ToString();
                        break;
                    }
                }
                moc = null;
                mc = null;
                return st;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }

        }
    }
}