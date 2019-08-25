using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

namespace ACExternal.GFX
{
    public class Overlay
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowLong(IntPtr hWnd, int nIndex);

        // Sets the opacity and transparency color key of a layered window.
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);

        // Changes an attribute of the specified window.
        // The function also sets the 32-bit (long) value at the specified offset into the extra window memory.
        [DllImport("user32.dll", SetLastError = true)]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        // Extends the window frame into the client area.
        [DllImport("dwmapi.dll", SetLastError = true)]
        public static extern void DwmExtendFrameIntoClientArea(IntPtr hWnd, ref Margins pMargins);

        // Sets a new extended window style
        private const int GWL_EXSTYLE = -20;
        // Use bAlpha to determine the opacity of the layered window.
        private const int LWA_ALPHA = 0x2;
        // The window is a layered window.
        // This style cannot be used if the window has a class style of either CS_OWNDC or CS_CLASSDC.
        private const int WS_EX_LAYERED = 0x80000;
        // The window should not be painted until siblings beneath the window (that were created by the same thread) have been painted.
        // The window appears transparent because the bits of underlying sibling windows have already been painted.
        // To achieve transparency without these restrictions, use the SetWindowRgn function.
        private const int WS_EX_TRANSPARENT = 0x20;

        public Form Window { get; set; }

        public Overlay()
        {
            Window = new Form
            {
                Name = "Overlay Window",
                Text = "Overlay Window",
                MinimizeBox = false,
                MaximizeBox = false,
                FormBorderStyle = FormBorderStyle.None,
                TopMost = true,
                Width = 16,
                Height = 16,
                Left = -32000,
                Top = -32000,
                StartPosition = FormStartPosition.Manual
            };

            Window.Load += (sender, args) =>
            {
                var exStyle = GetWindowLong(Window.Handle, GWL_EXSTYLE);
                exStyle |= WS_EX_LAYERED;
                exStyle |= WS_EX_TRANSPARENT;

                // make the window's border completely transparent
                SetWindowLong(Window.Handle, GWL_EXSTYLE, (IntPtr)exStyle);

                // set the alpha on the whole window to 255 (solid)
                SetLayeredWindowAttributes(Window.Handle, 0, 255, LWA_ALPHA);
            };

            Window.SizeChanged += (sender, args) => ExtendFrameIntoClientArea();
            Window.LocationChanged += (sender, args) => ExtendFrameIntoClientArea();
            //Window.Closed += (sender, args) => System.Windows.Application.Current.Shutdown();

            Window.Visible = false;
            Window.Show();
        }

        // Extend the window frame into the client area.
        private void ExtendFrameIntoClientArea()
        {
            var margins = new Margins
            {
                Left = 0,
                Right = 0,
                Top = 0,
                Bottom = 0,
            };
            DwmExtendFrameIntoClientArea(Window.Handle, ref margins);
        }

        public void drawRect(Rectangle x)
        {
                Window.BackColor = Color.Blue; // TODO: temporary

                if (Window.Location != x.Location || Window.Size != x.Size)
                {
                    if (x.Width > 0 && x.Height > 0)
                    {
                        // valid
                        Window.Location = x.Location;
                        Window.Size = x.Size;
                    }
                    else
                    {
                        // invalid
                        Window.Location = new Point(-32000, -32000);
                        Window.Size = new Size(16, 16);
                    }
                }
            
        }

    }
}
