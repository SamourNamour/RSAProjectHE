#region Name Space(s)
using System;
using System.Collections.Generic;
using System.Text;
#endregion 

namespace MTV.Library.Core.TriggerInterface
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.harris.com")]
    //[System.Xml.Serialization.XmlRootAttribute("UDPmsg", Namespace = "http://www.harris.com", IsNullable = false)]
    [System.Xml.Serialization.XmlRootAttribute("UDPmsg",IsNullable = false)]
    public class UDPmsg_t
    {
        #region Attribut(s)
        private object[] itemsField;
        private ItemsChoiceType[] itemsElementNameField;
        private decimal schemaVersionField;
        private bool schemaVersionFieldSpecified;
        private string msgNumberField;
        #endregion 

        #region Property(ies)
        [System.Xml.Serialization.XmlElementAttribute("ACK", typeof(empty_t), Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlElementAttribute("NAK", typeof(empty_t), Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlElementAttribute("event", typeof(event_t), Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
        public object[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }
        
        [System.Xml.Serialization.XmlElementAttribute("ItemsElementName")]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemsChoiceType[] ItemsElementName
        {
            get
            {
                return this.itemsElementNameField;
            }
            set
            {
                this.itemsElementNameField = value;
            }
        }
        
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal schemaVersion
        {
            get
            {
                return this.schemaVersionField;
            }
            set
            {
                this.schemaVersionField = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool schemaVersionSpecified
        {
            get
            {
                return this.schemaVersionFieldSpecified;
            }
            set
            {
                this.schemaVersionFieldSpecified = value;
            }
        }
        
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string msgNumber
        {
            get
            {
                return this.msgNumberField;
            }
            set
            {
                this.msgNumberField = value;
            }
        }
        #endregion 
    }
}
