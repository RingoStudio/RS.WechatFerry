using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS.WechatFerry.configs
{
    internal static class SysConfigs
    {
        public const string LOG_FILE = "WCF";


        #region MESSAGES
        public static int MaxMessageTextLength = 500;
        public static string MessageCombiner = "\n————————————————\n";
        public static int MinMessageSendInterval = 3000;
        public static int MaxMessageSendInterval = 6000;
        #endregion
    }
}
