using RS.Tools.Common.Enums;
using RS.Tools.Common.Utils;
using RS.WechatFerry.model;
using RS.WechatFerry.native;
using RS.WechatFerry.pb;
using System.Runtime.InteropServices;

namespace RS.WechatFerry
{
    public class Wechat
    {
        #region FIELDS
        private WcfClient _client;
        private Action<RecvMsg> _recvCallback;
        private bool _isRstart;
        private string _selfWXID;
        #endregion

        #region INIT
        public Wechat(Action<RecvMsg> receiveCallback, bool isRestart = false)
        {
            _recvCallback = receiveCallback;
            _isRstart = isRestart;
            _client = new WcfClient(6666);
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="receiveCallback"></param>
        /// <returns></returns>
        public bool Init()
        {
            try
            {
                _selfWXID = "";

                if (native.SDKMethods.WxInitSDK(false, 6666) != 0)
                {
                    LoggerStatic.WriteWarning(configs.SysConfigs.LOG_FILE, "启动RPC失败", "wechat_init");
                    return false;
                }

                if (!_client.Init())
                {
                    LoggerStatic.WriteWarning(configs.SysConfigs.LOG_FILE, "初始化RPC连接失败", "wechat_init");
                    return false;
                }

                //if (_client.EnableRecvTxt(_recvCallback) != 0)
                //{
                //    LoggerStatic.WriteWarning(configs.SysConfigs.LOG_FILE, "启动消息监听失败", "wechat_init");
                //    return false;
                //}
                Console.WriteLine("成功连接RPC");
                return true;
            }
            catch (Exception ex)
            {
                LoggerStatic.WriteException(configs.SysConfigs.LOG_FILE, ex, "wechat_init");
            }
            return false;
        }

        public void DestroyWCF() => native.SDKMethods.WxDestroySDK();
        #endregion

        #region OPERATIONS
        #region SYS INFO
        /// <summary>
        /// 微信登录状态
        /// </summary>
        /// <returns></returns>
        public bool IsLogin() => _client.IsLogin();
        /// <summary>
        /// 登陆账号的wxid
        /// </summary>
        /// <returns></returns>
        public string SelfWxid()
        {
            if (string.IsNullOrEmpty(_selfWXID)) _selfWXID = _client.GetSelfWxid();
            return _selfWXID;
        }

        /// <summary>
        /// 登陆账号的信息
        /// </summary>
        /// <returns></returns>
        public model.UserInfo? SelfInfo() => _client.GetSelfInfo();
        public void ClearMsgQ() => _client.ClearMessageQueue();
        #endregion

        #region RECEIVE
        /// <summary>
        /// 停止接收
        /// </summary>
        /// <returns></returns>
        public bool StopRecv() => _client.DisableRecvTxt() == 0;
        /// <summary>
        /// 启动接收
        /// </summary>
        /// <returns></returns>
        public bool StartRecv() => _client.EnableRecvTxt(_recvCallback) == 0;
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="extra"></param>
        /// <returns>文件PATH</returns>
        public string DownloadFile(ulong ID, string extra, int timeout = 30) => _client.DownloadFile(ID, extra, timeout);
        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="ID">消息中 id</param>
        /// <param name="extra">消息中的 extra</param>
        /// <param name="dir">存放图片的目录（目录不存在会出错）</param>
        /// <param name="timeout">超时时间（秒）</param>
        /// <returns>图片文件PATH</returns>
        public string DownloadImage(ulong ID, string extra, string dir, int timeout = 30) => _client.DownloadImage(ID, extra, dir, timeout);
        #endregion

        #region CONTACTS
        /// <summary>
        /// 获取昵称
        /// </summary>
        /// <param name="wxid"></param>
        /// <returns></returns>
        public string GetAlias(string wxid) => _client.GetAlias(wxid);
        /// <summary>
        /// 获取通讯录
        /// </summary>
        /// <returns></returns>
        public List<Contact> GetContacts() => _client.GetContacts().Select(a => new Contact(a)).ToList();
        /// <summary>
        /// 获取好友列表
        /// </summary>
        /// <returns></returns>
        public List<Contact> GetFriends() => _client.GetFriends().Select(a => new Contact(a)).ToList();
        /// <summary>
        /// 通过wxid获取用户信息
        /// </summary>
        /// <param name="wxid"></param>
        /// <returns></returns>
        public Contact? GetInfoByWxid(string wxid) => new Contact(_client.GetInfoByWxid(wxid));
        /// <summary>
        /// 通过好友申请
        /// </summary>
        /// <param name="v3">加密用户名 (好友申请消息里 v3 开头的字符串)</param>
        /// <param name="v4">Ticket (好友申请消息里 v4 开头的字符串)</param>
        /// <param name="scene">申请方式 (好友申请消息里的 scene); 为了兼容旧接口，默认为扫码添加 (30)</param>
        /// <returns></returns>
        public bool AcceptNewFriends(string v3, string v4, int scene = 30) => _client.AcceptNewFriends(v3, v4, scene) == 0;
        #endregion

        #region CHATROOMS
        /// <summary>
        /// 获取群聊列表
        /// </summary>
        /// <returns></returns>
        public List<Contact> GetChatrooms() => _client.GetChatrooms().Select(a => new Contact(a)).ToList();
        /// <summary>
        /// 从数据库文件中获取群信息
        /// </summary>
        /// <returns></returns>
        public List<(string ID, string name)> GetChatroomsIDName() => _client.GetChatroomsByDB().Select(a => (a.Id, a.Name)).ToList();

        /// <summary>
        /// 添加群成员
        /// </summary>
        /// <param name="roomID">待加群的 id</param>
        /// <param name="wxids">要加到群里的 wxid，多个用逗号分隔</param>
        /// <returns></returns>
        public bool AddChatroomMembers(string roomID, string wxids) => _client.AddChatroomMembers(roomID, wxids) == 0;
        /// <summary>
        /// 删除群成员
        /// </summary>
        /// <param name="roomID">群的 id</param>
        /// <param name="wxids">要删除成员的 wxid，多个用逗号分隔</param>
        /// <returns>1 为成功，其他失败</returns>
        public bool DelChatroomMembers(string roomID, string wxids) => _client.DelChatroomMembers(roomID, wxids) == 0;
        /// <summary>
        /// 邀请群成员
        /// </summary>
        /// <param name="roomID">群的 id</param>
        /// <param name="wxids">要邀请成员的 wxid, 多个用逗号`,`分隔</param>
        /// <returns>1 为成功，其他失败</returns>
        public bool InvChatroomMembers(string roomID, string wxids) => _client.InviteChatroomMembers(roomID, wxids) == 0;
        /// <summary>
        /// 获取群成员信息
        /// </summary>
        /// <param name="roomID"></param>
        /// <returns></returns>
        public Dictionary<string, Dictionary<string, string>> GetChatroomMemberNames(string roomID = "") => _client.GetChatroomMemberNames(roomID);
        #endregion

        #region SEND MSG
        /// <summary>
        /// 发送文本
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="receiver"></param>
        /// <param name="at">要@的人列表，逗号分隔</param>
        /// <returns></returns>
        public bool SendText(string content, string receiver, string at = "", bool sendNow = false) => _client.SendText(content, receiver, at, sendNow) == 0;
        /// <summary>
        /// 发送图片
        /// </summary>
        /// <param name="imgPath"></param>
        /// <param name="receiver"></param>
        /// <returns></returns>
        public bool SendImg(string imgPath, string receiver, bool sendNow = false) => _client.SendImg(imgPath, receiver, sendNow) == 0;
        /// <summary>
        /// 发送文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="receiver"></param>
        /// <returns></returns>
        public bool SendFile(string filePath, string receiver, bool sendNow = false) => _client.SendFile(filePath, receiver, sendNow) == 0;
        /// <summary>
        /// 发送表情包
        /// </summary>
        /// <param name="imgPath"></param>
        /// <param name="receiver"></param>
        /// <returns></returns>
        public bool SendEmotion(string imgPath, string receiver, bool sendNow = false) => _client.SendEmotion(imgPath, receiver, sendNow) == 0;
        /// <summary>
        /// 发送卡片消息
        /// </summary>
        /// <param name="name">显示名字</param>
        /// <param name="account">公众号 id</param>
        /// <param name="title">标题</param>
        /// <param name="digest">摘要</param>
        /// <param name="url">url</param>
        /// <param name="thumbUrl">略缩图</param>
        /// <param name="receiver">接收人</param>
        /// <returns></returns>
        public bool SendArtical(string name, string account, string title, string digest, string url, string thumbUrl, string receiver, bool sendNow = false) =>
                    _client.SendArtical(name, account, title, digest, url, thumbUrl, receiver, sendNow) == 0;
        /// <summary>
        /// 发送拍一拍
        /// </summary>
        /// <param name="roomID"></param>
        /// <param name="wxid"></param>
        /// <returns></returns>
        public bool SendPat(string roomID, string wxid) => _client.SendPat(roomID, wxid) == 0;
        /// <summary>
        /// 转发消息
        /// 可以转发文本、图片、表情、甚至各种 XML
        /// </summary>
        /// <param name="msgID"></param>
        /// <param name="receiver"></param>
        /// <returns></returns>
        public bool ForwardMsg(ulong msgID, string receiver) => _client.ForwardMsg(msgID, receiver) == 0;
        /// <summary>
        /// 撤回消息
        /// </summary>
        /// <param name="msgID">待撤回消息的 id</param>
        /// <returns></returns>
        public bool RevokeMsg(ulong msgID) => _client.RevokeMsg(msgID) == 0;
        #endregion
        #endregion
    }
}
