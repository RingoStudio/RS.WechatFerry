using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS.WechatFerry.model
{
    public class UserInfo
    {
        public string? Wxid { get; }
        public string? Name { get; }
        public string? Mobile { get; }
        public string? Home { get; }
        public UserInfo(pb.UserInfo userInfo)
        {
            if (userInfo is null) return;
            this.Wxid = userInfo.Wxid;
            this.Name = userInfo.Name;
            this.Mobile = userInfo.Mobile;
            this.Home = userInfo.Home;
        }
    }
}
