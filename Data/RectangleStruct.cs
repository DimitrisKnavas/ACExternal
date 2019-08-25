using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ACExternal.Data
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RectangleStruct
    {
        public int Left, Top, Right, Bottom;
    }
}
