using RS.WechatFerry.pb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS.WechatFerry.model
{
    public class Contact
    {
        public string? Wxid { get; }
        public string? Code { get; }
        public string? Remark { get; }
        public string? Name { get; }
        public string? Country { get; }
        public string? Province { get; }
        public string? City { get; }
        public int Gender { get; }
        public Contact() { }
        public Contact(RpcContact? contact)
        {
            if (contact is null) return;
            this.Wxid = contact.Wxid;
            this.Code = contact.Code;
            this.Remark = contact.Remark;
            this.Name = contact.Name;
            this.Country = contact.Country;
            this.Province = contact.Province;
            this.City = contact.City;
            this.Gender = contact.Gender;
        }
    }
}
