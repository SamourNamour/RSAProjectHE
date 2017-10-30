using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Schema;
namespace MTV.Library.Core.TriggerInterface
{
    public class SchemaValidationError
    {
        #region private members
        private string msDescription;
        private XmlSeverityType meSeverityType;
        #endregion

        #region constructors
        public SchemaValidationError(XmlSeverityType eSeverityType, string sDescription)
        {
            msDescription = sDescription;
            meSeverityType = eSeverityType;
        }
        #endregion

        #region public properties

        public XmlSeverityType SeverityType
        {
            get { return meSeverityType; }
        }

        public string Description
        {
            get { return msDescription; }
        }
        #endregion
    }
}
