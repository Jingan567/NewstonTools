using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewstonTools.WinformControl.Framework.SerialPorts
{
    public class NewSerialPortStatic
    {
        /// <summary>
        /// 设置串口号
        /// </summary>
        /// <param name="obj">需要绑定的项的集合（如：ComboBox中项的集合ComboBox.Items）</param>
        public static void SetPortNameValues(IList obj)
        {
            obj.Clear();
            try
            {
                foreach (string str in SerialPort.GetPortNames())
                {
                    obj.Add(str);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("未能查询到串口名称——" + e.ToString());
            }

        }

        /// <summary>
        /// 设置波特率
        /// </summary>
        /// <param name="obj">需要绑定的项的集合（如：ComboBox中项的集合ComboBox.Items）</param>
        public static void SetBauRateValues(IList obj)
        {
            obj.Clear();
            foreach (BaudRates rate in Enum.GetValues(typeof(BaudRates)))
            {
                obj.Add(((int)rate).ToString());
            }
        }

        /// <summary>
        /// 设置数据位
        /// </summary>
        /// <param name="obj">需要绑定的项的集合（如：ComboBox中项的集合ComboBox.Items）</param>
        public static void SetDataBitsValues(IList obj)
        {
            obj.Clear();
            foreach (DataBits databit in Enum.GetValues(typeof(DataBits)))
            {
                obj.Add(((int)databit).ToString());
            }
        }

        

        /// <summary>
        /// 设置停止位
        /// </summary>
        /// <param name="obj">需要绑定的项的集合（如：ComboBox中项的集合ComboBox.Items）</param>
        public static void SetStopBitValues(IList obj)
        {
            obj.Clear();
            foreach (string str in Enum.GetNames(typeof(StopBits)))
            {
                obj.Add(str);
            }
        }

        /// <summary>
        /// 设置校验位列表
        /// </summary>
        /// <param name="obj">需要绑定的项的集合（如：ComboBox中项的集合ComboBox.Items）</param>
        public static void SetParityValues(IList obj)
        {
            obj.Clear();
            foreach (string str in Enum.GetNames(typeof(Parity)))
            {
                obj.Add(str);
            }
        }

    }
}
