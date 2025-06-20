using NewstonTools.WinformControl.Framework.SerialPorts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;

namespace NewstonTools.WinformControl.Framework.SerialPorts
{

    class SerialPortClient
    {
        

        #region 变量属性
        public event Action<List<byte>> DataReceived;
        public event SerialErrorReceivedEventHandler ErrorReceived;

        private SerialPort comPort = new SerialPort();
        private string portName = "COM1";//串口号，默认COM1
        private BaudRates baudRate = BaudRates.BR_9600;//波特率
        private Parity parity = Parity.None;//校验位
        private StopBits stopBits = StopBits.One;//停止位
        private DataBits dataBits = DataBits.Eight;//数据位        

        /// <summary>
        /// 串口号
        /// </summary>
        public string PortName
        {
            get { return portName; }
            set { portName = value; }
        }

        /// <summary>
        /// 波特率
        /// </summary>
        public BaudRates BaudRate
        {
            get { return baudRate; }
            set { baudRate = value; }
        }

        /// <summary>
        /// 奇偶校验位
        /// </summary>
        public Parity Parity
        {
            get { return parity; }
            set { parity = value; }
        }

        /// <summary>
        /// 数据位
        /// </summary>
        public DataBits DataBits
        {
            get { return dataBits; }
            set { dataBits = value; }
        }

        /// <summary>
        /// 停止位
        /// </summary>
        public StopBits StopBits
        {
            get { return stopBits; }
            set { stopBits = value; }
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public SerialPortClient()
        {
            BoundEvents();
        }

        void BoundEvents()
        {
            comPort.DataReceived += new SerialDataReceivedEventHandler(comPort_DataReceived);
            comPort.ErrorReceived += new SerialErrorReceivedEventHandler(comPort_ErrorReceived);
        }

        /// <summary>
        /// 参数构造函数（使用枚举参数构造）
        /// </summary>
        /// <param name="baud">波特率</param>
        /// <param name="par">奇偶校验位</param>
        /// <param name="sBits">停止位</param>
        /// <param name="dBits">数据位</param>
        /// <param name="name">串口号</param>
        public SerialPortClient(string name, BaudRates baud, Parity par, DataBits dBits, StopBits sBits)
        {
            this.portName = name;
            this.baudRate = baud;
            this.parity = par;
            this.dataBits = dBits;
            this.stopBits = sBits;
            BoundEvents();
        }

        /// <summary>
        /// 参数构造函数（使用字符串参数构造）
        /// </summary>
        /// <param name="baud">波特率</param>
        /// <param name="par">奇偶校验位</param>
        /// <param name="sBits">停止位</param>
        /// <param name="dBits">数据位</param>
        /// <param name="name">串口号</param>
        public SerialPortClient(string name, string baud, string par, string dBits, string sBits)
        {
            this.portName = name;
            this.baudRate = (BaudRates)Enum.Parse(typeof(BaudRates), baud);
            this.parity = (Parity)Enum.Parse(typeof(Parity), par);
            this.dataBits = (DataBits)Enum.Parse(typeof(DataBits), dBits);
            this.stopBits = (StopBits)Enum.Parse(typeof(StopBits), sBits);
            BoundEvents();
        }
        #endregion

        #region 事件处理函数
        /// <summary>
        /// 数据仓库
        /// </summary>
        List<byte> datapool = new List<byte>();//存放接收的所有字节
        /// <summary>
        /// 数据接收处理
        /// </summary>
        void comPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (comPort.IsOpen)     //判断是否打开串口
            {

                Byte[] receivedData = new Byte[comPort.BytesToRead];        //创建接收字节数组
                comPort.Read(receivedData, 0, receivedData.Length);         //读取数据

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
        /// <summary>
        /// 错误处理函数
        /// </summary>
        void comPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            if (ErrorReceived != null)
            {
                ErrorReceived(sender, e);
            }
        }
        #endregion

        #region 串口关闭/打开
        /// <summary>
        /// 端口是否已经打开
        /// </summary>
        public bool IsOpen
        {
            get
            {
                return comPort.IsOpen;
            }
        }

        /// <summary>
        /// 打开端口
        /// </summary>
        /// <returns></returns>
        public void Open()
        {
            if (comPort.IsOpen) comPort.Close();

            comPort.PortName = portName;
            comPort.BaudRate = (int)baudRate;
            comPort.Parity = parity;
            comPort.DataBits = (int)dataBits;
            comPort.StopBits = stopBits;

            comPort.Open();
        }

        /// <summary>
        /// 关闭端口
        /// </summary>
        public void Close()
        {
            if (comPort.IsOpen) comPort.Close();
        }

        /// <summary>
        /// 丢弃来自串行驱动程序的接收和发送缓冲区的数据
        /// </summary>
        public void DiscardBuffer()
        {
            comPort.DiscardInBuffer();
            comPort.DiscardOutBuffer();
        }
        #endregion

        #region 写入数据
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="buffer"></param>
        public void Write(byte[] buffer, int offset, int count)
        {
            if (!(comPort.IsOpen)) comPort.Open();
            comPort.Write(buffer, offset, count);
        }

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="buffer">写入端口的字节数组</param>
        public void Write(byte[] buffer)
        {
            if (!(comPort.IsOpen)) comPort.Open();
            comPort.Write(buffer, 0, buffer.Length);
        }
        #endregion
    }


}
