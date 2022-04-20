using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EwpApi.Dto.ewp_specs_api_registry
{
    public class OtherHeiId
    {
        //[XmlText(Type = typeof(MyValueType))]
        [XmlText(typeof(string))]
        public string InnerText;

        [XmlAttributeAttribute(AttributeName = "Type", Type = typeof(string))]
        public string Type;
    }

    public class OtherHeiIdType
    {
        public static string erasmus = "erasmus";
        public static string erasmus_charter = "erasmus-charter";
        public static string euc = "euc";
        public static string previous_schac = "previous-schac";
        public static string pic = "pic";
    }

}
