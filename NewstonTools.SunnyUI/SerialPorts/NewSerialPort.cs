
using NewstonTools.WinformControl.Framework.ToolFunctions;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NewstonTools.SunnyUI.SerialPorts
{
    public partial class NewSerialPort : UIUserControl
    {
        public NewSerialPort()
        {
            InitializeComponent();
            InitBoundEnum();
            InitBoundEvents();
            bool resFlag = InitUseConfig();
            if (resFlag)
            {
                serialportClientConfigChangeFromUI();
            }
            //InitFont();
            //this.FontChanged += NewSerialPort_FontChanged;
        }

        private void NewSerialPort_FontChanged(object sender, EventArgs e)
        {
            Font font = FontManager.GetScaledFontSize(this);//获取缩放后的字体
            FontManager.SetFontRecursively(this, font);//递归设置字体
        }

        private void InitFont()
        {
            // 1. 父控件（UserControl）设置字体,这个只要设置过字体就没办法了
            this.Font = new Font("微软雅黑", 10F); // 父控件字体大小 10pt

            // 2. 清除子控件的手动字体设置（关键：让子控件回归继承状态）
            // 设计时拖入的子控件需手动清除 Font 属性（在属性窗口中选“(默认)”）
            // 代码创建的子控件无需额外设置（默认 Font 为 null，自动继承）
        }

        [Browsable(true)]
        [Category("New")]
        [Description("配置文件保存路径，需配置")]
        public string ConfigPath { get; set; }//属性不能私有

        [Browsable(true)]
        [Category("New")]
        [Description("配置文件文件名，需配置")]
        public string ConfigFileName { get; set; }//属性不能私有

        [Browsable(true)]
        [Category("New")]
        [Description("发送的串口命令，需配置")]
        public string SendCommand { get; set; }


        /// <summary>
        /// 数据接收事件
        /// </summary>
        [Browsable(true)]
        [Category("New")]
        [Description("数据接收事件")]
        public event Action<List<byte>> DataReceived;
        /// <summary>
        /// 串口接收错误数据处理程序
        /// </summary>
        [Browsable(true)]
        [Category("New")]
        [Description("串口接收错误数据处理程序")]
        public event SerialErrorReceivedEventHandler ErrorReceived;

        //[Browsable(true)]
        //[Category("New")]
        //[Description("保存文件处理程序，第一个参数是ConfigPath，第二个配置内容")]
        //public event Action<string,string> FileSaved;

        private SerialPort serialportClient { get; set; } = new SerialPort();

        /// <summary>
        /// 测试连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton1_Click(object sender, EventArgs e)
        {
            serialportClient = new SerialPort();
            serialportClientConfigChangeFromUI();

        }

        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>DataBits
        private void uiButton2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ConfigPath) && !string.IsNullOrEmpty(ConfigFileName))
            {
                string content = serialportClientConfigChangeFromUI();
                try
                {
                    if (!Directory.Exists(ConfigPath))
                    {
                        Directory.CreateDirectory(ConfigPath);
                    }
                    string localFile = Path.Combine(ConfigPath, ConfigFileName);
                    StreamWriter sw = File.CreateText(localFile);
                    sw.Write(content);
                    sw.Flush();
                    sw.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("错误信息：" + ex.Message);
                }
            }
            else
            {
                MessageBox.Show(nameof(ConfigPath) + "未配置");
            }
        }

        /// <summary>
        /// 使用配置初始化SerialPort
        /// </summary>
        private bool InitUseConfig()
        {
            if (!string.IsNullOrEmpty(ConfigPath) && !string.IsNullOrEmpty(ConfigFileName))
            {
                try
                {
                    string localfile = Path.Combine(ConfigPath, ConfigFileName);
                    string content = File.ReadAllText(localfile);
                    string[] lines = content.Split(';');
                    string[] temp0 = lines.Select(i => i.Split(':')[0]).ToArray();
                    string[] temp1 = lines.Select(i => i.Split(':')[1]).ToArray();
                    if (lines.Length == 5 && temp0.Length == 5 && temp1.Length == 5)
                    {
                        uiComboBox1.SelectedItem = temp1[0];
                        uiComboBox2.SelectedItem = temp1[1];
                        uiComboBox3.SelectedItem = temp1[2];
                        uiComboBox4.SelectedItem = temp1[3];
                        uiComboBox5.SelectedItem = temp1[4];
                        return true;
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("错误信息：" + ex.Message);
                    return false;
                }

            }
            else
            {
                MessageBox.Show(nameof(ConfigPath) + "未配置");
                return false;
            }
        }

        private string serialportClientConfigChangeFromUI()
        {
            serialportClient.PortName = (uiComboBox1.SelectedText ?? "COM1");//因为有获取不到串口信息的情况
            serialportClient.BaudRate = ((int)uiComboBox2.SelectedValue);
            serialportClient.DataBits = (int)uiComboBox3.SelectedValue;
            serialportClient.StopBits = (StopBits)uiComboBox4.SelectedValue;
            serialportClient.Parity = (Parity)uiComboBox5.SelectedValue;
            //英文冒号，引号
            return $"{nameof(uiComboBox1)}:{uiComboBox1.SelectedText};"
                 + $"{nameof(uiComboBox2)}:{(int)uiComboBox2.SelectedValue};"
                 + $"{nameof(uiComboBox3)}:{(int)uiComboBox3.SelectedValue};"
                 + $"{nameof(uiComboBox4)}:{(StopBits)uiComboBox4.SelectedValue};"
                 + $"{nameof(uiComboBox5)}:{(Parity)uiComboBox5.SelectedValue};";
        }

        private bool InitBoundEnum()
        {
            try
            {
                NewSerialPortStatic.SetPortNameValues(uiComboBox1.Items);
                NewSerialPortStatic.SetBauRateValues(uiComboBox2.Items);
                NewSerialPortStatic.SetDataBitsValues(uiComboBox3.Items);
                NewSerialPortStatic.SetStopBitValues(uiComboBox4.Items);
                NewSerialPortStatic.SetParityValues(uiComboBox5.Items);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误信息：" + ex.Message);
                return false;
            }

        }

        private void InitBoundEvents()
        {
            serialportClient.DataReceived += new SerialDataReceivedEventHandler(comPort_DataReceived);
            serialportClient.ErrorReceived += new SerialErrorReceivedEventHandler(comPort_ErrorReceived);
        }

        private void comPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            if (ErrorReceived != null)
            {
                ErrorReceived(sender, e);
            }
        }

        /// <summary>
        /// 数据仓库
        /// </summary>
        List<byte> datapool = new List<byte>();//存放接收的所有字节
        /// <summary>
        /// 数据接收处理
        /// </summary>
        private void comPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (serialportClient.IsOpen)     //判断是否打开串口
            {

                Byte[] receivedData = new Byte[serialportClient.BytesToRead];        //创建接收字节数组
                serialportClient.Read(receivedData, 0, receivedData.Length);         //读取数据

                //触发整条记录的处理
                if (DataReceived != null)
                {
                    datapool.AddRange(receivedData);
                    DataReceived(datapool);
                }

            }
            else
            {
                Console.WriteLine("请打开某个串口", "Error");
            }

        }
    }
}
