using Google.Protobuf;
using nng;
using RS.Tools.Common.Utils;
using RS.WechatFerry.model;
using RS.WechatFerry.pb;
using RS.WechatFerry.utils;
using System;
using System.Collections.Concurrent;
using System.Data;


namespace RS.WechatFerry
{
    internal class WcfClient
    {
        private readonly string _host = "127.0.0.1";
        private readonly int _port = 6666;
        private bool _isRestart = false;


        private IPairSocket? _cmdSocket;
        private IPairSocket? _msgSocket;
        IAPIFactory<INngMsg> Factory;


        public WcfClient(int port = 6666, string ip = "127.0.0.1", bool isRestart = false)
        {
            _host = ip;
            _port = port;
            _isRestart = isRestart;
        }

        public bool Init()
        {
            try
            {
                InitMsgQueue();

                var managedAssemblyPath = Path.GetDirectoryName(typeof(WcfClient).Assembly.Location);
                var alc = new NngLoadContext(managedAssemblyPath);
                Factory = NngLoadContext.Init(alc, "nng.Factories.Latest.Factory");
                _cmdSocket = Factory.PairOpen().ThenDial($"tcp://{_host}:{_port}").Unwrap();

                var flag = false;
                do
                {
                    if (IsLogin()) break;
                    Thread.Sleep(3000);
                    if (!flag)
                    {
                        LoggerStatic.WriteInfo(configs.SysConfigs.LOG_FILE, "init", "正在等待微信客户端登录");
                        flag = true;
                    }
                } while (true);

                return true;
            }
            catch (Exception ex)
            {
                LoggerStatic.WriteException(configs.SysConfigs.LOG_FILE, ex, "wcfclient_init");
            }
            return false;
        }


        /// <summary>
        /// RPC连接状态
        /// </summary>
        /// <returns></returns>
        public bool ConnectionStatus() => _cmdSocket.IsValid();

        #region RECV MSG
        /// <summary>
        /// 开启消息接收服务
        /// </summary>
        /// <param name="func">接受消息回调</param>
        /// <param name="pyq">是否接收朋友圈消息</param>
        /// <returns>int32 0 为成功，其他失败</returns>
        public int EnableRecvTxt(Action<RecvMsg> func, bool pyq = false)
        {
            try
            {
                Request request1 = new Request() { Func = Functions.FuncEnableRecvTxt, Flag = pyq };
                _cmdSocket?.Send(request1.ToByteArray());

                var recvMsg = _cmdSocket?.RecvMsg().Unwrap();
                var recvData = recvMsg?.AsSpan().ToArray();
                var response = Response.Parser.ParseFrom(recvData);

                if (_msgSocket != null)
                {
                    _msgSocket.Dispose();
                    _msgSocket = null;
                }

                Task.Run(() =>
                {

                    _msgSocket = Factory.PairOpen().ThenDial($"tcp://{_host}:{_port + 1}").Unwrap();
                    //Request request1 = new Request() { Func = Functions.FuncIsLogin };
                    //socket.Send(request1.ToByteArray());
                    try
                    {
                        while (_msgSocket != null)
                        {
                            var recvMsg = _msgSocket.RecvMsg().Unwrap();
                            var recvData = recvMsg.AsSpan().ToArray();
                            var response = Response.Parser.ParseFrom(recvData);
                            try
                            {
                                if (func is not null) func(new RecvMsg(response.Wxmsg));
                            }
                            catch (Exception ex)
                            {
                                LoggerStatic.WriteException(configs.SysConfigs.LOG_FILE, ex, "wcfclient_enablerecvtxt_01_callback");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LoggerStatic.WriteException(configs.SysConfigs.LOG_FILE, ex, "wcfclient_enablerecvtxt_02_task");
                    }
                });
                return response.Status;
            }
            catch (Exception ex)
            {
                LoggerStatic.WriteException(configs.SysConfigs.LOG_FILE, ex, "wcfclient_enablerecvtxt_03");
                return -1;
            }
        }
        /// <summary>
        /// 停止消息接收服务
        /// </summary>
        /// <returns>int32 0 为成功，其他失败</returns>
        public int DisableRecvTxt()
        {
            try
            {
                Request request1 = new Request() { Func = Functions.FuncDisableRecvTxt };
                _cmdSocket?.Send(request1.ToByteArray());

                var recvMsg = _cmdSocket?.RecvMsg().Unwrap();
                var recvData = recvMsg?.AsSpan().ToArray();
                var response = Response.Parser.ParseFrom(recvData);

                return response.Status;
            }
            catch (Exception ex)
            {
                LoggerStatic.WriteException(configs.SysConfigs.LOG_FILE, ex, "wcfclient_disablerecvtxt");
                return -1;
            }

        }
        /// <summary>
        /// 下载附件（图片、视频、文件）
        /// </summary>
        /// <param name="id">消息中 id</param>
        /// <param name="thumb">消息中的 thumb</param>
        /// <param name="extra"> 消息中的 extra</param>
        /// <returns></returns>
        private int DownloadAttach(ulong id, string thumb, string extra)
        {
            try
            {
                Request request = new()
                {
                    Func = Functions.FuncDownloadAttach,
                    Att = new()
                    {
                        Id = id,
                        Thumb = thumb,
                        Extra = extra
                    }
                };
                _cmdSocket?.Send(request.ToByteArray());

                var recvMsg = _cmdSocket?.RecvMsg().Unwrap();
                var recvData = recvMsg?.AsSpan().ToArray();
                var response = Response.Parser.ParseFrom(recvData);

                return response.Status;
            }
            catch (Exception ex)
            {
                LoggerStatic.WriteException(configs.SysConfigs.LOG_FILE, ex, "wcfclient_downloadattach");
                return -1;
            }
        }
        /// <summary>
        /// 解密图片
        /// </summary>
        /// <param name="src">加密的图片路径</param>
        /// <param name="dir">保存图片的目录</param>
        /// <returns></returns>
        private string DecryptImage(string src, string dir)
        {
            try
            {
                Request request = new()
                {
                    Func = Functions.FuncDecryptImage,
                    Dec = new()
                    {
                        Src = src,
                        Dst = dir,
                    }
                };
                _cmdSocket?.Send(request.ToByteArray());

                var recvMsg = _cmdSocket?.RecvMsg().Unwrap();
                var recvData = recvMsg?.AsSpan().ToArray();
                var response = Response.Parser.ParseFrom(recvData);

                return response.Str ?? "";
            }
            catch (Exception ex)
            {
                LoggerStatic.WriteException(configs.SysConfigs.LOG_FILE, ex, "wcfclient_decryptimage");
                return "";
            }
        }
        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="msgId">消息中 id</param>
        /// <param name="extra">消息中的 extra</param>
        /// <param name="dir">存放图片的目录（目录不存在会出错）</param>
        /// <param name="timeout">超时时间（秒）</param>
        /// <returns></returns>
        public string DownloadImage(ulong msgId, string extra, string dir, int timeout = 30)
        {
            try
            {
                //if (DownloadAttach(msgId, "", extra) != 0)
                //{
                //    LoggerStatic.WriteWarning(configs.SysConfigs.LOG_FILE, $"未下载图片 id:{msgId}, path:{extra}", "wcfclient_downloadimage");
                //    return "";
                //}
                DownloadAttach(msgId, "", extra);

                int cnt = 0;
                string path;
                while (cnt < timeout)
                {
                    if (System.IO.File.Exists(extra))
                    {
                        path = DecryptImage(extra, dir);
                        if (!string.IsNullOrEmpty(path)) return path;
                    }
                    Thread.Sleep(1000);
                    cnt++;
                }
                LoggerStatic.WriteWarning(configs.SysConfigs.LOG_FILE, $"下载图片超时 id:{msgId}, path:{extra}", "wcfclient_downloadimage");

            }
            catch (Exception ex)
            {
                LoggerStatic.WriteException(configs.SysConfigs.LOG_FILE, ex, "wcfclient_downloadimage");
            }

            return "";
        }
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="msgId"></param>
        /// <param name="extra"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public string DownloadFile(ulong msgId, string extra, int timeout = 30)
        {
            try
            {
                //if (DownloadAttach(msgId, "", extra) != 0)
                //{
                //    LoggerStatic.WriteWarning(configs.SysConfigs.LOG_FILE, $"未下载文件 id:{msgId}, path:{extra}", "wcfclient_downloadfile");
                //    return "";
                //}
                DownloadAttach(msgId, "", extra);

                int cnt = 0;
                while (cnt < timeout)
                {
                    if (System.IO.File.Exists(extra))
                    {
                        return extra;
                    }
                    Thread.Sleep(1000);
                    cnt++;
                }
                LoggerStatic.WriteWarning(configs.SysConfigs.LOG_FILE, $"下载文件超时 id:{msgId}, path:{extra}", "wcfclient_downloadfile");

            }
            catch (Exception ex)
            {
                LoggerStatic.WriteException(configs.SysConfigs.LOG_FILE, ex, "wcfclient_downloadfile");
            }

            return "";
        }
        /// <summary>
        /// 下载语音信息
        /// </summary>
        /// <param name="msgId"></param>
        /// <param name="dir"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public string GetAudioMsg(ulong msgId, string dir, int timeout = 30)
        {
            try
            {
                var func = (() =>
                {
                    var request1 = new Request()
                    {
                        Func = Functions.FuncGetAudioMsg,
                        Am = new AudioMsg()
                        {
                            Id = msgId,
                            Dir = dir,
                        }
                    };
                    _cmdSocket?.Send(request1.ToByteArray());

                    var recvMsg = _cmdSocket?.RecvMsg().Unwrap();
                    var recvData = recvMsg?.AsSpan().ToArray();
                    var response = Response.Parser.ParseFrom(recvData);
                    return response.Str;
                });

                if (timeout == 0) return func();

                int cnt = 0;
                while (cnt < timeout)
                {
                    var ret = func();
                    if (!string.IsNullOrEmpty(ret) && System.IO.File.Exists(ret)) return ret;
                    Thread.Sleep(1000);
                    cnt++;
                }
                LoggerStatic.WriteWarning(configs.SysConfigs.LOG_FILE, $"下语音消息超时 id:{msgId}", "wcfclient_getaudiomsg");
                return "";
            }
            catch (Exception ex)
            {
                LoggerStatic.WriteException(configs.SysConfigs.LOG_FILE, ex, "wcfclient_getaudiomsg");
                return "";
            }
        }

        #endregion


        #region SEND MSG
        /// <summary>
        /// 发送文本
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="receiver"></param>
        /// <param name="at">要@的人列表，逗号分隔</param>
        /// <returns></returns>
        public int SendText(string msg, string receiver, string at = "", bool sendNow = false)
        {
            try
            {
                if (!sendNow)
                {
                    MsgEnqueu(new SendMsg(SendMessageType.TEXT, receiver, msg, at));
                    return 0;
                }

                var request1 = new Request()
                {
                    Func = Functions.FuncSendTxt,
                    Txt = new TextMsg()
                    {
                        Msg = msg,
                        Receiver = receiver,
                        Aters = at
                    }
                };
                _cmdSocket?.Send(request1.ToByteArray());

                var recvMsg = _cmdSocket?.RecvMsg().Unwrap();
                var recvData = recvMsg?.AsSpan().ToArray();
                var response = Response.Parser.ParseFrom(recvData);
                return response.Status;
            }
            catch (Exception ex)
            {
                LoggerStatic.WriteException(configs.SysConfigs.LOG_FILE, ex, "wcfclient_sendtext");
                return -1;
            }
        }
        /// <summary>
        /// 发送图片
        /// </summary>
        /// <param name="imgpath"></param>
        /// <param name="receiver"></param>
        /// <returns></returns>
        public int SendImg(string imgpath, string receiver, bool sendNow = false)
        {
            try
            {
                if (!sendNow)
                {
                    MsgEnqueu(new SendMsg(type: SendMessageType.IMAGE,
                                          message: "",
                                          target: receiver,
                                          path: imgpath
                                          //@params: new Dictionary<string, string>()
                                          //{
                                          //    {"path", imgpath }
                                          //}
                                          ));
                    return 0;
                }

                var request1 = new Request() { Func = Functions.FuncSendImg, File = new PathMsg() { Path = imgpath, Receiver = receiver } };
                _cmdSocket?.Send(request1.ToByteArray());

                var recvMsg = _cmdSocket?.RecvMsg().Unwrap();
                var recvData = recvMsg?.AsSpan().ToArray();
                var response = Response.Parser.ParseFrom(recvData);

                return response.Status;
            }
            catch (Exception ex)
            {
                LoggerStatic.WriteException(configs.SysConfigs.LOG_FILE, ex, "wcfclient_sendimg");
                return -1;
            }
        }
        /// <summary>
        /// 发送文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="receiver"></param>
        /// <returns></returns>
        public int SendFile(string filePath, string receiver, bool sendNow = false)
        {
            try
            {
                if (!sendNow)
                {
                    MsgEnqueu(new SendMsg(type: SendMessageType.FILE,
                                          message: "",
                                          target: receiver,
                                          path: filePath
                                          //@params: new Dictionary<string, string>()
                                          //{
                                          //    {"path", filePath }
                                          //}
                                          ));
                    return 0;
                }

                var request1 = new Request() { Func = Functions.FuncSendFile, File = new PathMsg() { Path = filePath, Receiver = receiver } };
                _cmdSocket?.Send(request1.ToByteArray());

                var recvMsg = _cmdSocket?.RecvMsg().Unwrap();
                var recvData = recvMsg?.AsSpan().ToArray();
                var response = Response.Parser.ParseFrom(recvData);

                return response.Status;
            }
            catch (Exception ex)
            {
                LoggerStatic.WriteException(configs.SysConfigs.LOG_FILE, ex, "wcfclient_sendfile");
                return -1;
            }
        }
        /// <summary>
        /// 发送表情包
        /// </summary>
        /// <param name="imgpath"></param>
        /// <param name="receiver"></param>
        /// <returns></returns>
        public int SendEmotion(string imgpath, string receiver, bool sendNow = false)
        {
            try
            {
                if (!sendNow)
                {
                    MsgEnqueu(new SendMsg(type: SendMessageType.EMOTION,
                                          message: "",
                                          target: receiver,
                                          @params: new Dictionary<string, string>()
                                          {
                                              {"path", imgpath }
                                          }
                                          ));
                    return 0;
                }


                var request1 = new Request() { Func = Functions.FuncSendEmotion, File = new PathMsg { Path = imgpath, Receiver = receiver } };
                _cmdSocket?.Send(request1.ToByteArray());

                var recvMsg = _cmdSocket?.RecvMsg().Unwrap();
                var recvData = recvMsg?.AsSpan().ToArray();
                var response = Response.Parser.ParseFrom(recvData);

                return response.Status;
            }
            catch (Exception ex)
            {
                LoggerStatic.WriteException(configs.SysConfigs.LOG_FILE, ex, "wcfclient_sendemotion");
                return -1;
            }
        }
        ///// <summary>
        ///// 转发消息
        ///// </summary>
        ///// <param name="msgid"></param>
        ///// <param name="receiver"></param>
        ///// <param name="count"></param>
        ///// <returns></returns>
        //private int ForwardMsg(ulong msgid, string receiver, int count = 3, bool sendNow = false)
        //{
        //    try
        //    {
        //        var request1 = new Request() { Func = Functions.FuncForwardMsg, Fm = new ForwardMsg { Id = msgid, Receiver = receiver } };
        //        _cmdSocket?.Send(request1.ToByteArray());

        //        var recvMsg = _cmdSocket?.RecvMsg().Unwrap();
        //        var recvData = recvMsg?.AsSpan().ToArray();
        //        var response = Response.Parser.ParseFrom(recvData);

        //        if (response.Status != 1 && count != 0)
        //        {
        //            return ForwardMsg(msgid, receiver, count - 1);
        //        }

        //        return response.Status;
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerStatic.WriteException(configs.SysConfigs.LOG_FILE, ex, "wcfclient_forwardmsg");
        //        return -1;
        //    }
        //}
        /// <summary>
        /// 发送卡片消息
        /// </summary>
        /// <param name="name">显示名字</param>
        /// <param name="account">公众号 id</param>
        /// <param name="title">标题</param>
        /// <param name="digest">摘要</param>
        /// <param name="url">url</param>
        /// <param name="thumburl">略缩图</param>
        /// <param name="receiver">接收人</param>
        /// <returns></returns>
        public int SendArtical(string name, string account, string title, string digest, string url, string thumburl, string receiver, bool sendNow = false)
        {
            try
            {
                if (!sendNow)
                {
                    MsgEnqueu(new SendMsg(type: SendMessageType.ARTICAL,
                                          message: "",
                                          target: receiver,
                                          @params: new Dictionary<string, string>()
                                          {
                                              {"name", name },
                                              {"account", account },
                                              {"title", title },
                                              {"digest", digest },
                                              {"url", url },
                                              {"thumb", thumburl },
                                          }
                                          ));
                    return 0;
                }

                var request1 = new Request() { Func = Functions.FuncSendRichTxt, Rt = new RichText() { Name = name, Account = account, Title = title, Digest = digest, Url = url, Thumburl = thumburl, Receiver = receiver } };
                _cmdSocket?.Send(request1.ToByteArray());

                var recvMsg = _cmdSocket?.RecvMsg().Unwrap();
                var recvData = recvMsg?.AsSpan().ToArray();
                var response = Response.Parser.ParseFrom(recvData);

                return response.Status;
            }
            catch (Exception ex)
            {
                LoggerStatic.WriteException(configs.SysConfigs.LOG_FILE, ex, "wcfclient_sendrichtext");
                return -1;
            }
        }
        /// <summary>
        /// 发送拍一拍
        /// </summary>
        /// <param name="roomID"></param>
        /// <param name="wxid"></param>
        /// <returns></returns>
        public int SendPat(string roomID, string wxid)
        {
            try
            {
                var request1 = new Request()
                {
                    Func = Functions.FuncSendPatMsg,
                    Pm = new PatMsg()
                    {
                        Roomid = roomID,
                        Wxid = wxid,
                    }
                };
                _cmdSocket?.Send(request1.ToByteArray());

                var recvMsg = _cmdSocket?.RecvMsg().Unwrap();
                var recvData = recvMsg?.AsSpan().ToArray();
                var response = Response.Parser.ParseFrom(recvData);

                return response.Status;
            }
            catch (Exception ex)
            {
                LoggerStatic.WriteException(configs.SysConfigs.LOG_FILE, ex, "wcfclient_sendpat");
                return -1;
            }
        }
        /// <summary>
        /// 转发消息
        /// 可以转发文本、图片、表情、甚至各种 XML
        /// </summary>
        /// <param name="msgID"></param>
        /// <param name="receiver"></param>
        /// <returns></returns>
        public int ForwardMsg(ulong msgID, string receiver)
        {
            try
            {
                var request1 = new Request()
                {
                    Func = Functions.FuncForwardMsg,
                    Fm = new ForwardMsg()
                    {
                        Id = msgID,
                        Receiver = receiver,
                    }
                };
                _cmdSocket?.Send(request1.ToByteArray());

                var recvMsg = _cmdSocket?.RecvMsg().Unwrap();
                var recvData = recvMsg?.AsSpan().ToArray();
                var response = Response.Parser.ParseFrom(recvData);

                return response.Status;
            }
            catch (Exception ex)
            {
                LoggerStatic.WriteException(configs.SysConfigs.LOG_FILE, ex, "wcfclient_forwardmsg");
                return -1;
            }
        }
        /// <summary>
        /// 撤回消息
        /// </summary>
        /// <param name="msgId">待撤回消息的 id</param>
        /// <returns></returns>
        public int RevokeMsg(ulong msgId)
        {
            try
            {
                var request = new Request()
                {
                    Func = Functions.FuncRevokeMsg,
                    Ui64 = msgId,
                };

                _cmdSocket?.Send(request.ToByteArray());

                var recvMsg = _cmdSocket?.RecvMsg().Unwrap();
                var recvData = recvMsg?.AsSpan().ToArray();
                var response = Response.Parser.ParseFrom(recvData);

                return response.Status;
            }
            catch (Exception ex)
            {
                LoggerStatic.WriteException(configs.SysConfigs.LOG_FILE, ex, "wcfclient_revokemsg");
                return -1;
            }
        }
        #endregion

        #region GROUP MANAGEMENT
        /// <summary>
        /// 获取群聊列表
        /// </summary>
        /// <returns></returns>
        public List<RpcContact> GetChatrooms()
        {
            var result = new List<RpcContact>();
            var data = GetContacts();
            if (data != null)
            {
                foreach (var item in data)
                {
                    if (CmdHelper.ContactType(item.Wxid) == "群聊")
                    {
                        result.Add(item);
                    }
                }

            }
            return result;
        }

        /// <summary>
        /// 从数据库文件中获取群信息
        /// </summary>
        /// <returns></returns>
        public List<IDNameMsg> GetChatroomsByDB()
        {
            var result = new List<IDNameMsg>();
            var sessionlist = DbSqlQuery("MicroMsg.db", "SELECT strUsrName,strNickName FROM Session;");

            for (int i = 0; i < (sessionlist?.Rows?.Count ?? 0); i++)
            {
                var id = sessionlist?.Rows?[i]?["strUsrName"]?.ToString() ?? "";

                if ((id ?? "").EndsWith("@chatroom") && !result.Any(p => p.Id == id))
                {
                    result.Add(new IDNameMsg() { Id = id ?? "", Name = sessionlist?.Rows?[i]?["strNickName"]?.ToString() ?? "" });
                }
            }
            return result;
        }
        /// <summary>
        /// 获取群成员信息
        /// </summary>
        /// <param name="roomId">群的 id</param>
        /// <returns>群成员列表: {wxid1: 昵称1, wxid2: 昵称2, ...}</returns>
        public Dictionary<string, Dictionary<string, string>> GetChatroomMemberNames(string roomId)
        {
            try
            {
                var ret = new Dictionary<string, Dictionary<string, string>>();
                var allRooms = GetChatrooms().Select(a => a.Wxid).ToList();
                if (!string.IsNullOrEmpty(roomId))
                {
                    if (!allRooms.Contains(roomId)) return ret;
                    ret.Add(roomId, new());
                }
                else
                {
                    foreach (var id in allRooms) ret.Add(id, new());
                }


                var dt = DbSqlQuery("MicroMsg.db", "SELECT UserName, NickName FROM Contact;");
                var contacts = new Dictionary<string, string>();
                if (dt is not null)
                {
                    foreach (dynamic item in dt.Rows)
                    {
                        contacts[item.ItemArray[0]] = item.ItemArray[1];
                    }
                }

                foreach (var id in ret.Keys.ToArray())
                {
                    dt = DbSqlQuery("MicroMsg.db", $"SELECT RoomData FROM ChatRoom WHERE ChatRoomName = '{id}'");
                    if (dt is null || dt.Rows.Count <= 0 || dt.Rows[0][0] is null) return ret;

                    var data = Convert.FromBase64String(dt.Rows[0][0]?.ToString() ?? "");
                    var roomData = RoomData.Parser.ParseFrom(data);

                    foreach (var item in roomData.Members)
                    {
                        if (string.IsNullOrEmpty(item.Name) && contacts.ContainsKey(item.Wxid))
                        {
                            ret[id][item.Wxid] = contacts[item.Wxid];
                        }
                        else
                        {
                            ret[id][item.Wxid] = item.Name;
                        }
                    }
                }

                return ret;
            }
            catch (Exception ex)
            {
                LoggerStatic.WriteException(configs.SysConfigs.LOG_FILE, ex, "wcfclient_getchatroommembers");
                return new();
            }
        }

        /// <summary>
        /// 添加群成员
        /// </summary>
        /// <param name="roomId">待加群的 id</param>
        /// <param name="wxids">要加到群里的 wxid，多个用逗号分隔</param>
        /// <returns></returns>
        public int AddChatroomMembers(string roomId, string wxids)
        {
            try
            {
                var request = new Request()
                {
                    Func = Functions.FuncAddRoomMembers,
                    M = new()
                    {
                        Roomid = roomId,
                        Wxids = wxids,
                    }
                };
                _cmdSocket?.Send(request.ToByteArray());

                var recvMsg = _cmdSocket?.RecvMsg().Unwrap();
                var recvData = recvMsg?.AsSpan().ToArray();
                var response = Response.Parser.ParseFrom(recvData);

                return response.Status;
            }
            catch (Exception ex)
            {
                LoggerStatic.WriteException(configs.SysConfigs.LOG_FILE, ex, "wcfclient_addchatroommembers");
                return -1;
            }
        }
        /// <summary>
        /// 删除群成员
        /// </summary>
        /// <param name="roomId">群的 id</param>
        /// <param name="wxids">要删除成员的 wxid，多个用逗号分隔</param>
        /// <returns>1 为成功，其他失败</returns>
        public int DelChatroomMembers(string roomId, string wxids)
        {
            try
            {
                var request = new Request()
                {
                    Func = Functions.FuncDelRoomMembers,
                    M = new()
                    {
                        Roomid = roomId,
                        Wxids = wxids,
                    }
                };
                _cmdSocket?.Send(request.ToByteArray());

                var recvMsg = _cmdSocket?.RecvMsg().Unwrap();
                var recvData = recvMsg?.AsSpan().ToArray();
                var response = Response.Parser.ParseFrom(recvData);

                return response.Status;
            }
            catch (Exception ex)
            {
                LoggerStatic.WriteException(configs.SysConfigs.LOG_FILE, ex, "wcfclient_delchatroommembers");
                return -1;
            }

        }
        /// <summary>
        /// 邀请群成员
        /// </summary>
        /// <param name="roomId">群的 id</param>
        /// <param name="wxids">要邀请成员的 wxid, 多个用逗号`,`分隔</param>
        /// <returns>1 为成功，其他失败</returns>
        public int InviteChatroomMembers(string roomId, string wxids)
        {
            try
            {
                var request = new Request()
                {
                    Func = Functions.FuncInvRoomMembers,
                    M = new()
                    {
                        Roomid = roomId,
                        Wxids = wxids,
                    }
                };
                _cmdSocket?.Send(request.ToByteArray());

                var recvMsg = _cmdSocket?.RecvMsg().Unwrap();
                var recvData = recvMsg?.AsSpan().ToArray();
                var response = Response.Parser.ParseFrom(recvData);

                return response.Status;
            }
            catch (Exception ex)
            {
                LoggerStatic.WriteException(configs.SysConfigs.LOG_FILE, ex, "wcfclient_invchatroommembers");
                return -1;
            }
        }
        #endregion

        #region CONTACTS MANAGEMENT
        /// <summary>
        /// 获取昵称 
        /// </summary>
        /// <param name="wxid"></param>
        /// <returns></returns>
        public string GetAlias(string wxid)
        {
            var nickName = wxid;

            var userlist = DbSqlQuery("MicroMsg.db", "SELECT NickName FROM Contact WHERE UserName = '" + wxid + "';");
            if (userlist is not null && userlist.Rows.Count > 0)
            {
                nickName = userlist.Rows[0]["NickName"].ToString() ?? wxid;
            }
            return nickName;
        }
        /// <summary>
        /// 获取通讯录
        /// </summary>
        /// <returns></returns>
        public List<RpcContact> GetContacts()
        {
            try
            {
                Request request1 = new() { Func = Functions.FuncGetContacts };
                _cmdSocket?.Send(request1.ToByteArray());

                var recvMsg = _cmdSocket?.RecvMsg().Unwrap();
                var recvData = recvMsg?.AsSpan().ToArray();
                var response = Response.Parser.ParseFrom(recvData);

                return response?.Contacts?.Contacts?.ToList() ?? new();
            }
            catch (Exception ex)
            {
                LoggerStatic.WriteException(configs.SysConfigs.LOG_FILE, ex, "wcfclient_getcontacts");
                return new();
            }
        }

        /// <summary>
        /// 获取好友列表
        /// </summary>
        /// <returns></returns>
        public List<RpcContact> GetFriends()
        {
            var result = new List<RpcContact>();
            var data = GetContacts();
            if (data != null)
            {
                foreach (var item in data)
                {
                    if (CmdHelper.ContactType(item.Wxid) == "好友")
                    {
                        result.Add(item);
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 通过wxid获取用户信息
        /// </summary>
        /// <param name="wxid"></param>
        /// <returns></returns>
        public RpcContact? GetInfoByWxid(string wxid)
        {
            try
            {
                Request request1 = new Request() { Func = Functions.FuncGetContactInfo, Str = wxid };
                _cmdSocket?.Send(request1.ToByteArray());

                var recvMsg = _cmdSocket?.RecvMsg().Unwrap();
                var recvData = recvMsg?.AsSpan().ToArray();
                var response = Response.Parser.ParseFrom(recvData);

                if (response.Contacts?.Contacts?.Count > 0)
                {
                    return response.Contacts.Contacts[0];
                }
            }
            catch (Exception ex)
            {
                LoggerStatic.WriteException(configs.SysConfigs.LOG_FILE, ex, "wcfclient_getinfobywxid");
            }

            return null;
        }
        /// <summary>
        /// 通过好友申请
        /// </summary>
        /// <param name="v3">加密用户名 (好友申请消息里 v3 开头的字符串)</param>
        /// <param name="v4">Ticket (好友申请消息里 v4 开头的字符串)</param>
        /// <param name="scene">申请方式 (好友申请消息里的 scene); 为了兼容旧接口，默认为扫码添加 (30)</param>
        /// <returns></returns>
        public int AcceptNewFriends(string v3, string v4, int scene = 30)
        {
            try
            {
                Request request1 = new()
                {
                    Func = Functions.FuncAcceptFriend,
                    V = new()
                    {
                        V3 = v3,
                        V4 = v4,
                        Scene = scene,
                    }
                };
                _cmdSocket?.Send(request1.ToByteArray());

                var recvMsg = _cmdSocket?.RecvMsg().Unwrap();
                var recvData = recvMsg?.AsSpan().ToArray();
                var response = Response.Parser.ParseFrom(recvData);

                return response.Status;
            }
            catch (Exception ex)
            {
                LoggerStatic.WriteException(configs.SysConfigs.LOG_FILE, ex, "wcfclient_acceptnewfriends");
            }
            return -1;
        }
        #endregion

        #region SYS INFO
        /// <summary>
        /// 检查登录状态
        /// </summary>
        /// <returns></returns>
        public bool IsLogin()
        {
            try
            {
                Request request1 = new Request() { Func = Functions.FuncIsLogin };
                _cmdSocket?.Send(request1.ToByteArray());
                var recvMsg = _cmdSocket?.RecvMsg().Unwrap();
                var recvData = recvMsg?.AsSpan().ToArray();
                var response = Response.Parser.ParseFrom(recvData);
                return response.Status == 1;
            }
            catch (Exception ex)
            {
                LoggerStatic.WriteException(configs.SysConfigs.LOG_FILE, ex, "wcfclient_islogin");
                return false;
            }

        }
        /// <summary>
        /// 获取自己的wxid
        /// </summary>
        /// <returns></returns>
        public string GetSelfWxid()
        {
            try
            {
                Request request1 = new Request() { Func = Functions.FuncGetSelfWxid };
                _cmdSocket?.Send(request1.ToByteArray());
                var recvMsg = _cmdSocket?.RecvMsg().Unwrap();
                var recvData = recvMsg?.AsSpan().ToArray();
                var response = Response.Parser.ParseFrom(recvData);
                return response.Str ?? "";
            }
            catch (Exception ex)
            {
                LoggerStatic.WriteException(configs.SysConfigs.LOG_FILE, ex, "wcfclient_getselfwxid");
            }
            return "";
        }
        /// <summary>
        /// 获取自己信息
        /// </summary>
        /// <returns></returns>
        public model.UserInfo? GetSelfInfo()
        {
            try
            {
                Request request1 = new Request() { Func = Functions.FuncGetUserInfo };
                _cmdSocket?.Send(request1.ToByteArray());
                var recvMsg = _cmdSocket?.RecvMsg().Unwrap();
                var recvData = recvMsg?.AsSpan().ToArray();
                var response = Response.Parser.ParseFrom(recvData);
                if (response.Ui is null) return null;
                return new model.UserInfo(response.Ui);
            }
            catch (Exception ex)
            {
                LoggerStatic.WriteException(configs.SysConfigs.LOG_FILE, ex, "wcfclient_getselfinfo");
            }
            return null;
        }
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="db"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable? DbSqlQuery(string db, string sql)
        {
            try
            {
                Request request1 = new Request() { Func = Functions.FuncExecDbQuery, Query = new DbQuery() { Db = db, Sql = sql } };
                _cmdSocket?.Send(request1.ToByteArray());

                var recvMsg = _cmdSocket?.RecvMsg().Unwrap();
                var recvData = recvMsg?.AsSpan().ToArray();
                var response = Response.Parser.ParseFrom(recvData);

                DataTable dt = new();

                if (response.Rows is not null)
                {
                    foreach (DbRow row in response.Rows.Rows)
                    {
                        var nrow = dt.NewRow();
                        foreach (var item in row.Fields)
                        {
                            if (!dt.Columns.Contains(item.Column))
                            {
                                dt.Columns.Add(item.Column);
                            }
                            if (item.Type == 4)
                            {
                                nrow[item.Column] = item.Content.ToBase64();
                            }
                            else
                            {
                                nrow[item.Column] = item.Content.ToStringUtf8();
                            }
                        }
                        dt.Rows.Add(nrow);
                    }

                }

                return dt;
            }
            catch (Exception ex)
            {
                LoggerStatic.WriteException(configs.SysConfigs.LOG_FILE, ex, "wcfclient_dbsqlquery");
            }

            return null;
        }
        #endregion

        #region MESSAGE QUEUE
        private ConcurrentQueue<SendMsg> _msgQueue;
        private long _lastSendLog;
        private int _curInterval = configs.SysConfigs.MinMessageSendInterval;
        private Random _randInterval = new();
        private Task _sendingTask;

        private void InitMsgQueue()
        {
            _msgQueue = new();
            _lastSendLog = 0;

        }
        private void MsgEnqueu(SendMsg msg)
        {
            // 长消息 切割
            if (msg.IsNeedSplit())
            {
                foreach (var slice in msg.Split())
                {
                    _msgQueue.Enqueue(slice);
                }
                goto ToSend;
            }
            else
            {
                // 短消息合并
                foreach (var item in _msgQueue)
                {
                    if (item.IsMatch(msg))
                    {
                        item.AppendOne(msg);
                        goto ToSend;
                    }
                }
            }

            _msgQueue.Enqueue(msg);

ToSend:
            if (_sendingTask is null || _sendingTask.IsCompleted) _sendingTask = Task.Run(SendingMsg);
        }

        private void SendingMsg()
        {
            do
            {
                if (_msgQueue.Count <= 0) return;

                // 检查上一次发送的时间间隔
                if (_lastSendLog + configs.SysConfigs.MaxMessageSendInterval < RS.Tools.Common.Utils.TimeHelper.ToTimeStampMills())
                {
                    _msgQueue.TryDequeue(out var msg);
                    if (msg is null) return;
                    SendMsgNow(msg);
                    System.Threading.Thread.Sleep(200);
                }
            } while (true);
        }

        private void SendMsgNow(SendMsg msg)
        {
            switch (msg.MessageType)
            {
                case SendMessageType.PRIVATE_TEXT:
                case SendMessageType.GROUP_TEXT:
                case SendMessageType.TEXT:
                case SendMessageType.GROUP_AT:
                    SendText(msg.Message, msg.Target, msg.Ats, true);
                    break;
                case SendMessageType.IMAGE:
                    if (msg.Paths is not null && msg.Paths.Count > 0)
                    {
                        foreach (var path in msg.Paths)
                        {
                            SendImg(path, msg.Target, true);
                        }
                    }
                    break;
                case SendMessageType.FILE:
                    if (msg.Paths is not null && msg.Paths.Count > 0)
                    {
                        foreach (var path in msg.Paths)
                        {
                            SendFile(path, msg.Target, true);
                        }
                    }
                    break;
                case SendMessageType.EMOTION:
                    SendEmotion(msg.GetParam("path"), msg.Target, true);
                    break;
                case SendMessageType.ARTICAL:
                    SendArtical(msg.GetParam("name"),
                                 msg.GetParam("account"),
                                 msg.GetParam("title"),
                                 msg.GetParam("digest"),
                                 msg.GetParam("url"),
                                 msg.GetParam("thumb"),
                                 msg.Target, true);
                    break;
            }
            UpdateSendMessageInterval();
        }
        private void UpdateSendMessageInterval()
        {
            _curInterval = _randInterval.Next(configs.SysConfigs.MinMessageSendInterval, configs.SysConfigs.MaxMessageSendInterval);
            _lastSendLog = RS.Tools.Common.Utils.TimeHelper.ToTimeStampMills();
        }

        /// <summary>
        /// 清除消息队列
        /// </summary>
        /// <returns></returns>
        public bool ClearMessageQueue()
        {
            try
            {
                _msgQueue.Clear();
                return true;
            }
            catch (Exception ex)
            {
                Logger.Instance.WriteException(ex, "ClearMessageQueue");
                return false;
            }
        }

        #endregion

    }
}
