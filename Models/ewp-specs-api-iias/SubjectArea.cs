using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EwpApi.Dto.ewp_specs_api_iias
{
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://github.com/erasmus-without-paper/ewp-specs-api-iias/blob/stable-v6/endpoints/get-response.xsd")]
    public class SubjectArea
    {
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("isced-f-code")]
        public string IscedFcode { get; set; }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("isced-clarification")]
        public string IscedClarification { get; set; }
    }
}
