using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS.WechatFerry.model
{
    /// <summary>
    /// 发送消息
    /// </summary>
    internal class SendMsg
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        public SendMessageType MessageType;
        /// <summary>
        /// 消息内容
        /// </summary>
        public string Message;
        /// <summary>
        /// 发送目标
        /// </summary>
        public string Target;
        /// <summary>
        /// At名单
        /// </summary>
        public string Ats = "";

        public ulong msgId;
        public List<string> Paths;

        public Dictionary<string, string> Params = null;
        /// <summary>
        /// 对接成功并产生新消息内容时，+1
        /// </summary>
        public int AppendTimes = 0;


        public SendMsg(SendMessageType type,
            string target,
            string message,
            string ats = null,
            ulong msgId = 0,
            string path = "",
            Dictionary<string, string> @params = null)
        {
            MessageType = type;
            Message = message;
            Target = target;
            Ats = ats;
            this.msgId = msgId;
            Params = @params;
            if (!string.IsNullOrEmpty(path)) Paths = new List<string>() { path };
        }

        public void AppendAts(string ats)
        {
            var arr1 = this.Ats.Split(",").ToList();
            var arr2 = ats.Split(",");
            foreach (var id in arr2)
            {
                if (!arr1.Contains(id)) arr1.Add(id);
            }
            this.Ats = string.Join(",", arr1);
        }
        public string GetParam(string key)
        {
            if (this.Params is null) return "";
            return this.Params.ContainsKey(key) ? this.Params[key] : "";
        }

        public bool IsNeedSplit()
        {
            switch (this.MessageType)
            {
                case SendMessageType.TEXT:
                case SendMessageType.PRIVATE_TEXT:
                case SendMessageType.GROUP_TEXT:
                case SendMessageType.GROUP_AT:
                    if (string.IsNullOrEmpty(this.Ats) && this.Message.Length > configs.SysConfigs.MaxMessageTextLength) return true;
                    break;
                default: return false;
            }
            return false;
        }
        /// <summary>
        /// 将本消息进行分割
        /// </summary>
        /// <returns></returns>
        public List<SendMsg> Split()
        {
            var ret = new List<SendMsg>();
            var texts = new List<string>();
            var raw = this.Message;
            int startPos = 0;
            while (true)
            {
                var text = raw.Substring(startPos, Math.Min(configs.SysConfigs.MaxMessageTextLength, raw.Length - startPos));
                ret.Add(new SendMsg(this.MessageType, text, this.Target, ats: this.Ats));
                startPos += configs.SysConfigs.MaxMessageTextLength;
                if (startPos >= raw.Length) break;
            }

            int index = 1;
            foreach (var message in ret)
            {
                message.Message += $"\n(长消息 {index}/{ret.Count})";
                index++;
            }

            this.Ats = "";

            return ret;
        }


        /// <summary>
        /// 判断另一个消息是否可以和本消息合并
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsMatch(SendMsg other)
        {
            if (other.MessageType != this.MessageType) return false;
            switch (other.MessageType)
            {
                case SendMessageType.TEXT:
                case SendMessageType.PRIVATE_TEXT:
                case SendMessageType.GROUP_TEXT:
                case SendMessageType.GROUP_AT:
                    if (this.Target != other.Target) return false;
                    if (this.Message.Length + other.Message.Length + 10 > configs.SysConfigs.MaxMessageTextLength) return false;
                    // if (other.MessageType == SendMessageType.GROUP_AT) return false;
                    // if (this.Ats != other.Ats) return false;
                    break;
                case SendMessageType.FILE:
                case SendMessageType.IMAGE:
                    if (this.Target != other.Target) return false;
                    if (Paths.Count >= 9) return false;
                    break;
                default: return false;
            }
            return true;
        }

        public void AppendOne(SendMsg other)
        {
            this.Message += configs.SysConfigs.MessageCombiner;
            this.Message += other.Message;
            if (this.Paths is not null && other.Paths is not null) this.Paths.AddRange(other.Paths);
            if (this.MessageType == SendMessageType.GROUP_AT) AppendAts(other.Ats);
            other.Ats = "";
        }
        /// <summary>
        /// 打印消息
        /// </summary>
        public void Print()
        {
            var desc = new List<string>
            {
                $">> 发送消息：" + this.MessageType switch
                {
                    SendMessageType.PRIVATE_TEXT => "私聊文本",
                    SendMessageType.GROUP_TEXT => "群组文本",
                    SendMessageType.GROUP_AT => "群组AT",
                    SendMessageType.CARD => "分享名片",
                    SendMessageType.IMAGE => "图片",
                    SendMessageType.FILE => "文件",
                    SendMessageType.ARTICAL => "文章",
                    SendMessageType.APP => "APP",
                    SendMessageType.FORWARD => "转发",
                    SendMessageType.XML => "XML",
                    SendMessageType.EMOTION => "表情",
                    _=> "其他消息",
                },
            };

            if (!string.IsNullOrEmpty(Ats)) desc.Add("   AT: " + Ats);
            desc.Add("   TO: " + Target);
            if (!string.IsNullOrEmpty(this.Message)) desc.Add("   MSG:\n" + this.Message);
            if (this.Params is not null && this.Params.Count > 0)
            {
                foreach (var param in this.Params)
                {
                    desc.Add($"   {param.Key}: {param.Value}");
                }
            }
            desc.Add("");

            Console.WriteLine(string.Join("\n", desc));
        }
    }
}
