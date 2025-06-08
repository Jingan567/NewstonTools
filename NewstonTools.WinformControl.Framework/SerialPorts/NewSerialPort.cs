using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewstonTools.WinformControl.Framework.SerialPorts
{
    public partial class NewSerialPort : UIUserControl
    {
        public NewSerialPort()
        {
            InitializeComponent();
        }

        [Browsable(true)]
        [Description("配置文件保存地址，需配置")]
        public string ConfigPath { get; set; }//属性不能私有

        [Browsable(true)]
        [Description("发送的串口命令，需配置")]
        public string SendCommand { get; set; }


        /// <summary>
        /// 数据接收事件
        /// </summary>
        [Browsable(true)]
        public event Action<List<byte>> DataReceived;
        /// <summary>
        /// 串口接收错误数据处理程序
        /// </summary>
        [Browsable(true)]
        public event SerialErrorReceivedEventHandler ErrorReceived;

        private SerialPort serialportClient { get; set; }

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

        }

        /// <summary>
        /// 使用配置初始化SerialPort
        /// </summary>
        private void InitUseConfig()
        {

        }

        private void serialportClientConfigChangeFromUI()
        {
            serialportClient.PortName = uiComboBox1.SelectedText;
            serialportClient.BaudRate = (int)uiComboBox2.SelectedValue;
            serialportClient.DataBits = (int)uiComboBox3.SelectedValue;
            serialportClient.StopBits = (StopBits)uiComboBox4.SelectedValue;
            serialportClient.Parity = (Parity)uiComboBox5.SelectedValue;
        }
    }
}
