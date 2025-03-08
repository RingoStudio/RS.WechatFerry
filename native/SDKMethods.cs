using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RS.WechatFerry.native
{
    /// <summary>
    /// WCFSDK调用
    /// </summary>
    internal class SDKMethods
    {
        // wcf v3.9.2.4 -- wechat v3.9.10.27
        // const string dllPath = "wcfbin\\v3.9.2.4\\sdk.dll";
        // wcf v3.9.3.5 -- wechat v3.9.11.25
        // const string dllPath = "wcfbin\\v3.9.3.5\\sdk.dll";
        // wcf v3.9.4.1 -- wechat v3.9.12.17
        const string dllPath = "wcfbin\\v3.9.4.1\\sdk.dll";
        [DllImport(dllPath, EntryPoint = "WxInitSDK")]
        public static extern int WxInitSDK(bool debug, int port);
        [DllImport(dllPath, EntryPoint = "WxDestroySDK")]
        public static extern int WxDestroySDK();
    }
}
