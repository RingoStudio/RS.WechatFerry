using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;
using RS.Tools.Common.Enums;
using RS.Tools.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RS.WechatFerry.model
{
    /// <summary>
    /// 接受消息
    /// </summary>
    public class RecvMsg
    {
        #region FIELDS
        /// <summary>
        /// 是否是自己发出的消息
        /// </summary>
        public bool IsSelf { get; }
        /// <summary>
        /// 消息是否来自群聊
        /// </summary>
        public bool IsGroup { get; }
        /// <summary>
        /// 消息ID
        /// </summary>
        public ulong ID { get; }

        private WechatMessageType _messageType;
        /// <summary>
        /// 消息类型
        /// </summary>
        public WechatMessageType MessageType
        {
            get => _messageType;
            private set
            {
                _messageType = value;
                if (value != WechatMessageType.File) return;
                if (XmlJson is null) _messageType = WechatMessageType.Unknown;
                var subType = JSONHelper.ParseInt(XmlJson?["msg"]?["appmsg"]?["type"]);
                _messageType = subType switch
                {
                    6 => _messageType,
                    5 => WechatMessageType.AppCard,
                    19 => WechatMessageType.ChatHistory,
                    57 => WechatMessageType.QuoteMsg,
                    _ => WechatMessageType.Unknown,
                };
            }
        }
        /// <summary>
        /// 消息时间戳
        /// </summary>
        public ulong TimeStamp { get; }
        /// <summary>
        /// (如果消息来自群聊)群聊ID
        /// </summary>
        public string RoomID { get; }
        /// <summary>
        /// 消息内容
        /// </summary>
        public string Content { get; }
        /// <summary>
        /// 消息发送人
        /// </summary>
        public string Sender { get; }

        public string Sign { get; }
        public string Thumb { get; }
        public string Extra { get; }
        public string Xml { get; }
        private dynamic? _xmlJson;
        private bool _xmlJsonConverted = false;
        public dynamic? XmlJson
        {
            get
            {
                if (_xmlJsonConverted) return _xmlJson;
                try
                {
                    var str = Xml;
                    if (_messageType == WechatMessageType.File) str = Content;
                    if (string.IsNullOrEmpty(Xml)) _xmlJson = null;
                    else
                    {
                        var xml = new XmlDocument();
                        xml.LoadXml(str);
                        _xmlJson = JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeXmlNode(xml));
                    }
                }
                catch (Exception ex)
                {
                    LoggerStatic.WriteException(configs.SysConfigs.LOG_FILE, ex, "wechatmsg_get_xmljson");
                }
                _xmlJsonConverted = true;
                return _xmlJson;
            }

        }
        /// <summary>
        /// 消息包含的AT信息
        /// </summary>
        public List<string> AtWxids
        {
            get
            {
                var ret = new List<string>();
                try
                {
                    if (!IsGroup || MessageType != WechatMessageType.Text || XmlJson is null) return ret;
                    bool fromPC = Xml.Contains("[CDATA");

                    string wxids = (fromPC ? XmlJson["msgsource"]?["atuserlist"]["#cdata-section"] : XmlJson["msgsource"]?["atuserlist"])?.ToString() ?? "";
                    if (string.IsNullOrEmpty(wxids)) return ret;
                    return wxids.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList();

                }
                catch (Exception ex)
                {
                    LoggerStatic.WriteException(configs.SysConfigs.LOG_FILE, ex, "wechatmsg_get_atwxids");
                }
                return ret;
            }
        }


        #endregion

        #region CONSRUCT
        public RecvMsg(pb.WxMsg msg)
        {
            this.Content = msg.Content;
            this.Xml = msg.Xml;
            this.IsSelf = msg.IsSelf;
            this.IsGroup = msg.IsGroup;
            this.ID = msg.Id;
            this.MessageType = (WechatMessageType)msg.Type;
            this.TimeStamp = (ulong)msg.Ts;
            this.RoomID = msg.Roomid;
            this.Sender = msg.Sender;
            this.Sign = msg.Sign;
            this.Thumb = msg.Thumb;
            this.Extra = msg.Extra;
        }
        public RecvMsg(RecvMsg msg)
        {
            this.Content = msg.Content;
            this.Xml = msg.Xml;
            this.IsSelf = msg.IsSelf;
            this.IsGroup = msg.IsGroup;
            this.ID = msg.ID;
            this.MessageType = msg.MessageType;
            this.TimeStamp = msg.TimeStamp;
            this.RoomID = msg.RoomID;
            this.Sender = msg.Sender;
            this.Sign = msg.Sign;
            this.Thumb = msg.Thumb;
            this.Extra = msg.Extra;
        }
        public RecvMsg(bool isSelf, bool isGroup, ulong ID, WechatMessageType type, ulong timeStamp, string roomID, string content, string sender, string sign, string thumb, string extra, string xml)
        {
            this.Content = content;
            this.Xml = xml;
            this.IsSelf = isSelf;
            this.IsGroup = isGroup;
            this.ID = ID;
            this.MessageType = type;
            this.TimeStamp = TimeStamp;
            this.RoomID = roomID;
            this.Sender = sender;
            this.Sign = sign;
            this.Thumb = thumb;
            this.Extra = extra;
        }
        public RecvMsg Clone(RecvMsg msg) => new RecvMsg(msg);

        public string GetDesc()
        {
            var ret = new List<string>();
            ret.Add($"IsSelf:{IsSelf}");
            ret.Add($"IsGroup:{IsGroup}");
            ret.Add($"ID:{ID}");
            ret.Add($"Type:{MessageType}");
            ret.Add($"TimeStamp:{TimeStamp}");
            ret.Add($"RoomID:{Sender}");
            if (!string.IsNullOrEmpty(RoomID)) ret.Add($"RoomID:{RoomID}");
            if (!string.IsNullOrEmpty(Content)) ret.Add($"Content:{Content}");
            if (!string.IsNullOrEmpty(Sign)) ret.Add($"Sign:{Sign.Replace("\n", "")}");
            if (!string.IsNullOrEmpty(Thumb)) ret.Add($"Thumb:{Thumb.Replace("\n", "")}");
            if (!string.IsNullOrEmpty(Extra)) ret.Add($"Extra:{Extra.Replace("\n", "")}");
            if (!string.IsNullOrEmpty(Xml)) ret.Add($"Xml:{Xml.Replace("\n", "")}");
            return string.Join(" | ", ret);
        }
        #endregion
    }
}
