using System;
using System.Runtime.InteropServices;
using JuliusSweetland.OptiKey.Native.Enums;
using JuliusSweetland.OptiKey.Native.Structs;

namespace JuliusSweetland.OptiKey.Native
{
    public class WindowsAPI 
    { 
        [DllImport("kernel32.Dll")] 
        public static extern short GetVersionEx(ref OSVERSIONINFO o); 

        [DllImport("user32.dll")] 
        public static extern int GetSystemMetrics(int smIndex); 
  
        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool OpenProcessToken(IntPtr ProcessHandle, UInt32 DesiredAccess, out IntPtr TokenHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr hObject);

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool GetTokenInformation(IntPtr TokenHandle, TOKEN_INFORMATION_CLASS TokenInformationClass, IntPtr TokenInformation, uint TokenInformationLength, out uint ReturnLength);
    }
} 
