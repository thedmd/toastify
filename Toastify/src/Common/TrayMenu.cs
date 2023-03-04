using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;

namespace Toastify.Common
{
    public class TrayMenu : ContextMenuStrip
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy,
            uint uFlags);

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            var wmax = SystemParameters.VirtualScreenWidth;
            var hmax = SystemParameters.VirtualScreenHeight;

            var pos = Cursor.Position;
            x = pos.X;
            y = pos.Y;

            if (x + width > wmax) x -= width;
            if (y + height > hmax) y -= height;

            SetWindowPos(Handle, IntPtr.Zero, x, y, width, height, 0);
        }
    }
}