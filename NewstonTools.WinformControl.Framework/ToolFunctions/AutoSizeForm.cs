using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewstonTools.WinformControl.Framework.ToolFunctions
{
    public class AutoSizeForm : Form
    {
        
        private new AutoAdaptWindowsSize AutoSize;
        /// <summary>
        /// 没有背景图片可以不需要，有背景图片需要加上
        /// Winform 控件开发中用于优化双缓冲、解决闪烁问题 
        /// 强制控件及其子控件使用 “整体双缓冲” 绘制，避免刷新时的闪烁现象。
        /// CreateParams 是 Winform 控件的基类（Control）提供的属性，
        /// 用于定义控件创建时的 窗口样式、扩展样式、类名、父窗口 等底层参数（本质是 Windows API 中的 CREATESTRUCT 结构体封装）。
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;//获取基类默认参数，先获取基类（如 UserControl、Control）的默认 CreateParams，
                                                    //确保控件保留默认的窗口行为（如基础样式、父窗口关联等），避免直接新建 CreateParams 导致默认功能丢失。

                cp.ExStyle |= WindowStyles.WS_EX_COMPOSITED;//CreateParams 为控件添加 WS_EX_COMPOSITED 扩展样式（对应十六进制 0x02000000）
                return cp;
            }
        }
        override protected void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            AutoSize = new AutoAdaptWindowsSize(this);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            //窗体大小改变事件
            if (AutoSize != null) // 一定加这个判断，电脑缩放布局不是100%的时候，会报错
            {
                AutoSize.FormSizeChanged();
            }
        }
    }
}
