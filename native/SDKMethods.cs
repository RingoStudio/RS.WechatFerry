using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RS.WechatFerry.native
{
    internal class SDKMethods
    {
        const string dllPath = "wcfbin\\v3.9.2.4\\sdk.dll";
        [DllImport(dllPath, EntryPoint = "WxInitSDK")]
        public static extern int WxInitSDK(bool debug, int port);
        [DllImport(dllPath, EntryPoint = "WxDestroySDK")]
        public static extern int WxDestroySDK();
    }
}
