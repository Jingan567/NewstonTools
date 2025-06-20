using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace NewstonTools.WinformControl.Framework.Tests
{
    public partial class LedControl : UserControl
    {
        public LedControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton1_Click(object sender, EventArgs e)
        {
            StartEvent?.Invoke(sender, e);
        }

        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton2_Click(object sender, EventArgs e)
        {
            StopEvent?.Invoke(sender, e);
        }

        private int ledNo;
        [Browsable(true)]//属性页面展示
        [Category("Test")]//分类
        [Description("指示灯号")]//描述
        public int LedNo
        {
            get { return ledNo; }
            set
            {
                ledNo = value;
                this.uiLabel1.Text = "指示灯" + ledNo.ToString().PadLeft(2, '0') + ":";
            }
        }

        private bool ledStatus;
        [Category("Test")]
        [Description("指示灯状态")]
        public bool LedStatus
        {
            get { return ledStatus; }
            set
            {
                ledStatus = value;
                if (ledStatus == true)
                {
                    this.uiLabel2.BackColor = Color.FromArgb(0, 192, 0);//绿色
                }
                else
                {
                    this.uiLabel2.BackColor = Color.Red;
                }
            }
        }

        [Category("Test")]
        public event EventHandler StartEvent;
        [Category("Test")]
        public event EventHandler StopEvent;
    }
}
