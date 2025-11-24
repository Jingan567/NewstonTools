using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewstonTools.WinformControl.Framework.ToolFunctions
{
    public static class FontManager
    {
        // 基础DPI（96为Windows默认DPI）
        private const float BaseDpi = 96F;

        // 根据当前DPI缩放后的字体大小
        internal static Font GetScaledFontSize(Control control)
        {
            // 获取当前屏幕DPI
            using (Graphics g = Graphics.FromHwnd(control.Handle))
            {
                g.DrawString("测试",control.Font,new SolidBrush(Color.Blue),new Point(0,0));
                float currentDpi = g.DpiX;
                // 计算缩放比例
                float scaleFactor = currentDpi / BaseDpi;
                // 缩放字体大小
                float scaledSize = control.Font.Size * scaleFactor;
                return new Font(control.Font.FontFamily, scaledSize);
            }
        }



        // 递归设置字体的方法（同方案2）
        internal static void SetFontRecursively(Control rootControl, Font newFont)
        {
            if (rootControl == null || newFont == null) return;
            rootControl.Font = newFont;
            foreach (Control child in rootControl.Controls)
            {
                SetFontRecursively(child, newFont);
            }
        }
    }
}
