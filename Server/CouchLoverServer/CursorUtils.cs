using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;

namespace CouchLoverServer
{
    class Utils
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

        public static void SendKey(string Key)
        {
            SendKey(Key);
        }

        public static Bitmap GetCurrentScreen(int width, int height)
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            Bitmap screen = new Bitmap(bounds.Width, bounds.Height);

            using (Graphics g = Graphics.FromImage(screen))
            {
                g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
            }

            return ResizeImage(screen, width, height);
        }

        public static Bitmap ResizeImage(Image sourceImage, int width, int height)
        {
            Bitmap img = new Bitmap(width, height);
            using(Graphics g = Graphics.FromImage(img))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(sourceImage, new Rectangle(0,0, width,height));
            }

            return img;
        }

        public static Bitmap ResizeImage(Image sourceImage, int percent)
        {
            return ResizeImage(sourceImage, ((percent / 100) * sourceImage.Width), ((percent / 100) * sourceImage.Height));
        }
    }

    public enum MouseButton
    {
        Left,
        Right
    }
}
