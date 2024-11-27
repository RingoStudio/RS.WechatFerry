using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS.WechatFerry.model
{
    internal enum SendMessageType
    {
        TEXT = 99,
        /// <summary>
        /// 私聊文本
        /// </summary>
        PRIVATE_TEXT = 0,
        /// <summary>
        /// 群聊文本
        /// </summary>
        GROUP_TEXT = 1,
        /// <summary>
        /// 群聊AT
        /// </summary>
        GROUP_AT = 2,
        CARD = 3,
        IMAGE = 4,
        FILE = 5,
        ARTICAL = 6,
        APP = 7,
        FORWARD = 8,
        XML = 9,
        EMOTION = 10,
    }
}
