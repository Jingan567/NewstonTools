using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewstonTools.WinformControl.Framework.ToolFunctions
{
    /// <summary>
    /// 无边框的窗体拖动
    /// </summary>
    public class NoBorderDragForm : Form
    {
        private Point mPoint;//定义一个位置信息Point用于存储鼠标位置
                             //控件注册下面两个事件

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            mPoint = new Point(e.X, e.Y);
        }


        /// <summary>
        /// 鼠标移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.Button == MouseButtons.Left)
            {
                this.Location = new Point(this.Location.X + e.X - mPoint.X, this.Location.Y + e.Y - mPoint.Y);
            }
        }
    }
}
