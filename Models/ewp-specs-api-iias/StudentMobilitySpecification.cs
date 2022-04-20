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
    public abstract class StudentMobilitySpecification: MobilitySpecification
    {
        /// <summary>
        /// Total number of mobility months per academic year.
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("total-months-per-year")] 
        public int totalMonthsPerYear { get; set; }
        /// <summary>
        /// Blended mobility option for students. By sending 'true', the partners
        /// confirm that they are willing to exchange students who wish to carry out their mobility
        /// in a blended format, a combination of a short-term physical mobility with a virtual component.
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("blended")]
        public Boolean Blended { get; set; }
        /// <summary>
        /// EQF Level. In EWP it is typically used to represent study levels (also known as study cycles), 
        /// i.e.Short is 5, Bachelor is 6, Master is 7, and Doctorate is 8.
        /// Descriptions of all the levels can be found here:
        ///   https://en.wikipedia.org/wiki/European_Qualifications_Framework
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("eqf-level")]
        public List<int> EqfLevel { get; set; }
    }
}
