using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RS.WechatFerry.model
{
    [XmlRoot("sysmsg")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    internal partial class SysMsg
    {

        private SysMsgRevokeMsg revokemsgField;

        private string typeField;

        /// <remarks/>
        public SysMsgRevokeMsg revokemsg
        {
            get
            {
                return this.revokemsgField;
            }
            set
            {
                this.revokemsgField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    internal partial class SysMsgRevokeMsg
    {

        private string sessionField;

        private uint msgidField;

        private ulong newmsgidField;

        private string replacemsgField;

        private string announcement_idField;

        /// <remarks/>
        public string session
        {
            get
            {
                return this.sessionField;
            }
            set
            {
                this.sessionField = value;
            }
        }

        /// <remarks/>
        public uint msgid
        {
            get
            {
                return this.msgidField;
            }
            set
            {
                this.msgidField = value;
            }
        }

        /// <remarks/>
        public ulong newmsgid
        {
            get
            {
                return this.newmsgidField;
            }
            set
            {
                this.newmsgidField = value;
            }
        }

        /// <remarks/>
        public string replacemsg
        {
            get
            {
                return this.replacemsgField;
            }
            set
            {
                this.replacemsgField = value;
            }
        }

        /// <remarks/>
        public string announcement_id
        {
            get
            {
                return this.announcement_idField;
            }
            set
            {
                this.announcement_idField = value;
            }
        }
    }

}
