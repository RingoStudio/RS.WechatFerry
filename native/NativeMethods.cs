using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RS.WechatFerry.native
{
    /// <summary>
    /// WINDOWS注入调用
    /// </summary>
    public static class NativeMethods
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern nint LoadLibrary(string lpFileName);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool FreeLibrary(nint hModule);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern nint GetProcAddress(nint hModule, string lpProcName);

    }
}
