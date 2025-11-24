using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewstonTools.WinformControl.Framework.ToolFunctions
{
    // 窗口样式常量管理类
    public static class WindowStyles
    {
        /// <summary>
        /// 启用整体组合绘制（双缓冲），解决控件闪烁
        /// FrameWork支持 NativeMethods.WS_EX_COMPOSITED
        /// </summary>
        public const int WS_EX_COMPOSITED = 0x02000000;

        /// <summary>
        /// 透明窗口样式（点击穿透）
        /// </summary>
        public const int WS_EX_LAYERED = 0x00080000;

        /// <summary>
        /// 顶层窗口（始终显示在最前面）
        /// </summary>
        public const int WS_EX_TOPMOST = 0x00000008;
    }

}
