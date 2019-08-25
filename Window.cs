using ACExternal.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;

namespace ACExternal
{
    public abstract class Window
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool ClientToScreen(IntPtr hWnd, out PointStruct lpPoint);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetClientRect(IntPtr hwind, out RectangleStruct lpRect);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        private static string windowName = "AssaultCube";
        public static bool isWindowActive = false;
        private static IntPtr windowHandle;

        public static IntPtr WindowHandle
        {
            get { return windowHandle; }
            set { windowHandle = value; }
        }

        public static Rectangle rect { get; set; }


        private static System.Drawing.Rectangle GetClientRectangle(IntPtr handle)
        {
            if(ClientToScreen(handle, out var point) && GetClientRect(handle, out var rect))
            {
                return new System.Drawing.Rectangle(point.x, point.y, rect.Right - rect.Left, rect.Bottom - rect.Top);
            }
            return default;
        }

        private static bool GameWindow()
        {
            WindowHandle = FindWindow(null, windowName);
            if(WindowHandle == null)
            {
                Console.WriteLine("Error on getting the window handle. Error code: " + Marshal.GetLastWin32Error().ToString());
                return false;
            }

            rect = GetClientRectangle(windowHandle);
            if(rect.Width <=0 || rect.Height <= 0)
            {
                return false;
            }

            isWindowActive = (windowHandle == GetForegroundWindow());

            return isWindowActive;
        }

        public static void printRect()
        {
            if (GameWindow())
            {
                Console.WriteLine($"0x{(int)WindowHandle:X8} {rect.X} {rect.Y} {rect.Width} {rect.Height}");
            }
            else
            {
                Console.WriteLine("Game window is not valid");
            }
        }
    }
}
