using EwpApi.Dto.ewp_specs_types_contact;
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
    public abstract class MobilitySpecification
    {
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("sending-hei-id")]
        public string SendingHeiId { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("sending-ounit-id")]
        public string SendingHeiOunitId { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("sending-contact", Namespace = "https://github.com/erasmus-without-paper/ewp-specs-types-contact/tree/stable-v1")]
        public List<Contact> SendingContact { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("receiving-hei-id")]
        public string ReceivingHeiId { get; set; }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("receiving-ounit-id")]
        public string ReceivingHeiOunitId { get; set; }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("receiving-contact")]
        public List<Contact> ReceivingContact { get; set; }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("receiving-academic-year-id")]
        public List<string> ReceivingAcademicYearId { get; set; }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("mobilities-per-year")]
        public int MobilitiesPerYear { get; set; }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("recommended-language-skill")]
        public List<string> RecommendedLanguageSkill { get; set; }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("subject-area")]
        public List<SubjectArea> SubjectArea { get; set; }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("other-info-terms")]
        public string OtherInfoTerms { get; set; }
    }
}
