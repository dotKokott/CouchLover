using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;

namespace CouchLoverServer
{
    class CursorUtils
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(long dwFlags, long dx, long dy, long cButtons, long dwExtraInfo);

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        public static void Move(int x, int y)
        {
            Cursor.Position = new Point(x, y);
        }

        public static void MoveBy(int x, int y)
        {
            Move(Cursor.Position.X + x, Cursor.Position.Y + y);
        }

        public static void MouseClick(MouseButtons button, int x, int y)
        {
            Move(x, y);

            if(button == MouseButtons.Left)
                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, x, y, 0, 0);

            if(button == MouseButtons.Right)
                mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, x, y, 0, 0);
        }
    }

    public enum MouseButton
    {
        Left,
        Right
    }
}
