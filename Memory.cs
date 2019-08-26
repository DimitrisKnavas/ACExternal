using ACExternal.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ACExternal
{
    static class Memory
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(uint procAccess, bool bInheritHandle, int procId);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadProcessMemory(
           IntPtr hProcess,
           IntPtr lpBaseAddress,
           [Out] byte[] lpBuffer,
           int dwSize,
           out IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteProcessMemory(
           IntPtr hProcess,
           IntPtr lpBaseAddress,
           [Out] byte[] lpBuffer,
           int dwSize,
           out IntPtr lpNumberOfBytesRead);

        //[DllImport("kernel32.dll", SetLastError = true)]
        //public static extern bool ReadProcessMemory(
        //    IntPtr hProcess,
        //    IntPtr lpBaseAddress,
        //    [Out, MarshalAs(UnmanagedType.AsAny)] object lpBuffer,
        //    int dwSize,
        //    out int lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern unsafe bool CloseHandle(IntPtr hObject);

        //private static uint PROCESS_ALL_ACCESS = 0x1F0FFF;
        private static uint PROCESS_VM_READ = 0x0010;
        private static uint PROCESS_VM_WRITE = 0x0020;
        private static uint PROCESS_VM_OPERATION = 0x0008;
        private static int errorCode;
        private static int dwSize = sizeof(int);
        private static IntPtr numberBytesRW = IntPtr.Zero;
        private static int pid;

        public static int Pid
        {
            get { return pid; }
            set { pid = value; }
        }


        public static int IsProcessRunning()
        {
            
            try
            {
                Process p = Process.GetProcessesByName("ac_client").First();
                Pid = p.Id;
            }
            catch (Exception)
            {
                Pid = 0;
                return Pid;
            }

            return Pid;
        }

        private static IntPtr HandleToP(uint access_right, int pid)
        {
            IntPtr getHandle = OpenProcess(access_right, false, pid);
            return getHandle;
        }

        private static void KillHandle(IntPtr getHandle)
        {
            CloseHandle(getHandle);
        }
        
        public static void Write(IntPtr lpBaseAddress, int value)
        {
            byte[] valueWrite = BitConverter.GetBytes(value);
            IntPtr getHandle = IntPtr.Zero;
            uint accessCode = PROCESS_VM_WRITE | PROCESS_VM_READ | PROCESS_VM_OPERATION;
            getHandle = HandleToP(accessCode,pid);
            if(getHandle == null)
            {
                errorCode = Marshal.GetLastWin32Error();
                Console.WriteLine($"OpenProcess failed. Win32 error {Marshal.GetLastWin32Error().ToString()}");
            }
            else
            {
                WriteProcessMemory(getHandle, lpBaseAddress, valueWrite, dwSize, out numberBytesRW);
                errorCode = Marshal.GetLastWin32Error();
                KillHandle(getHandle);
            }
        }

        public static int Read(IntPtr baseAddress)
        {
            byte[] valueRead = new byte[4];
            IntPtr getHandle = IntPtr.Zero;
            uint accessCode = PROCESS_VM_WRITE | PROCESS_VM_READ | PROCESS_VM_OPERATION;
            getHandle = HandleToP(accessCode, pid);

            if (getHandle == null)
            {
                errorCode = Marshal.GetLastWin32Error();
                Console.WriteLine($"OpenProcess failed. Win32 error {Marshal.GetLastWin32Error().ToString()}");
                int result = 0;
                return result;
            }
            else
            {
                ReadProcessMemory(getHandle, baseAddress, valueRead, dwSize, out numberBytesRW);
                int result = BitConverter.ToInt32(valueRead,0);
                KillHandle(getHandle);
                return result;
            }
        }

        //public static T Read<T>(IntPtr lpBaseAddress) where T : unmanaged
        //{
        //    //T[] buffer = new T[Marshal.SizeOf<T>()];
        //    var buffer = (object)default(T);
        //    var size = Marshal.SizeOf<T>();
        //    IntPtr getHandle = IntPtr.Zero;
        //    uint accessCode = PROCESS_VM_WRITE | PROCESS_VM_READ | PROCESS_VM_OPERATION;
        //    getHandle = HandleToP(accessCode, pid);

        //    if (getHandle == null)
        //    {
        //        errorCode = Marshal.GetLastWin32Error();
        //        Console.WriteLine($"OpenProcess failed. Win32 error {Marshal.GetLastWin32Error().ToString()}");
        //        Vector3 result = default;
        //        return default(T);
        //    }
        //    else
        //    {
        //        ReadProcessMemory(getHandle, lpBaseAddress, buffer, Marshal.SizeOf<T>(), out lpNumberOfBytesRead);
        //        KillHandle(getHandle);
        //        return (T)buffer;
        //    }
        //}

        public static Vector3 ReadVector(IntPtr lpBaseAddress)
        {
            byte[] buffer = new byte[3 * 4];
            IntPtr getHandle = IntPtr.Zero;
            uint accessCode = PROCESS_VM_WRITE | PROCESS_VM_READ | PROCESS_VM_OPERATION;
            getHandle = HandleToP(accessCode, pid);

            if (getHandle == null)
            {
                errorCode = Marshal.GetLastWin32Error();
                Console.WriteLine($"OpenProcess failed. Win32 error {Marshal.GetLastWin32Error().ToString()}");
                Vector3 result = default;
                return result;
            }
            else
            {
                ReadProcessMemory(getHandle, lpBaseAddress, buffer, buffer.Length, out numberBytesRW);
                KillHandle(getHandle);
                Vector3 vec = new Vector3();
                vec.X = BitConverter.ToSingle(buffer, (0 * 4));
                vec.Y = BitConverter.ToSingle(buffer, (1 * 4));
                vec.Z = BitConverter.ToSingle(buffer, (2 * 4));
                return vec;
            }
        }
    }
}
