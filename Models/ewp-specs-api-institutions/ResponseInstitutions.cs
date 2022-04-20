using EwpApi.Dto.ewp_specs_architecture;
using EwpApi.Dto.ewp_specs_types_address;
using EwpApi.Dto.ewp_specs_types_contact;
using EwpApi.Helper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EwpApi.Dto.ewp_specs_api_institutions
{


    /// <summary>
    /// This describes the format of the response returned by the EWP Institutions API.
    /// </summary>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://github.com/erasmus-without-paper/ewp-specs-api-institutions/tree/stable-v2")]
    [System.Xml.Serialization.XmlRootAttribute("institutions-response",  Namespace = "https://github.com/erasmus-without-paper/ewp-specs-api-institutions/tree/stable-v2", IsNullable = false)]
    public partial class ResponseInstitutions : XmlBodyGenerator, IResponse
    {

        [XmlNamespaceDeclarations]
        public XmlSerializerNamespaces xmlns = new XmlSerializerNamespaces();

        public ResponseInstitutions()
        {            
            xmlns.Add("a", "https://github.com/erasmus-without-paper/ewp-specs-types-address/tree/stable-v1");
            xmlns.Add("c", "https://github.com/erasmus-without-paper/ewp-specs-types-contact/tree/stable-v1");
            xmlns.Add("p", "https://github.com/erasmus-without-paper/ewp-specs-types-phonenumber/tree/stable-v1");
            xmlns.Add("trm", "https://github.com/erasmus-without-paper/ewp-specs-types-academic-term/tree/stable-v1");
            xmlns.Add("xsd", "http://www.w3.org/2001/XMLSchema");
            xmlns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");   
        }

        [XmlAttributeAttribute(AttributeName = "schemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string schemaLocation = "https://github.com/erasmus-without-paper/ewp-specs-api-institutions/tree/stable-v2\n"
        +"https://raw.githubusercontent.com/erasmus-without-paper/ewp-specs-api-institutions/stable-v2/response.xsd";

        private Hei[] heiField;

        /// <summary>
        /// This represents a single institution. Servers will produce one such element for
        /// each of the `hei_id` values passed in the Institutions API call.
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("hei")]
        public Hei[] hei
        {
            get
            {
                return this.heiField;
            }
            set
            {
                this.heiField = value;
            }
        }

        public string ToXml()
        {
            return GenerateXml<ResponseInstitutions>(this);
        }

        public async Task WriteXmlBody(HttpResponse response)
        {
            await WriteXmlBodyIntoResponse<ResponseInstitutions>(this, response);
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://github.com/erasmus-without-paper/ewp-specs-api-institutions/tree/stable-v2")]
    public partial class Hei
    {

        private string heiidField;

        private List<HeiOtherid> otheridField;

        private List<HeiName> nameField;

        private string abbreviationField;

        private streetaddress streetaddressField;

        private mailingaddress mailingaddressField;

        private List<HeiWebsiteurl> websiteurlField;

        private string logourlField;

        private HeiMobilityfactsheeturl[] mobilityfactsheeturlField;

        private HeiCoursecatalogueurl[] coursecatalogueurlField;

        private Contact[] contactField;

        private string rootounitidField;

        private string[] ounitidField;

        /// <summary>
        /// The SCHAC identifier of this HEI.
        /// https://github.com/erasmus-without-paper/ewp-specs-api-registry/#schac-identifiers
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("hei-id")]
        public string heiid
        {
            get
            {
                return this.heiidField;
            }
            set
            {
                this.heiidField = value;
            }
        }

        /// <summary>
        ///A collection of other HEI IDs.
        /// If this HEI is covered by the server, then it is recommended for this set to be
        /// exactly the same as the one provided to the Registry Service (via the manifest file). 
        ///
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("other-id")]
        public List<HeiOtherid> otherid
        {
            get
            {
                return this.otheridField;
            }
            set
            {
                this.otheridField = value;
            }
        }

        /// <summary>
        ///  A collection of institution names, in different languages.
        /// 
        /// If this HEI is covered by the server, then it is recommended for this set to be
        /// exactly the same as the one provided to the Registry Service (via the manifest file).
        /// 
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("name")]
        public List<HeiName> name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <summary>
        /// Optional abbreviation of the HEI's name (usually 2-6 upper case letters, but
        /// there are no strict rules here). If given, then this abbreviation SHOULD be
        /// * unique within the country* of this HEI.It also SHOULD be well recognized,
        /// e.g.the first Google result for query "Poland UW" is "University of Warsaw",
        /// so "UW" is a well-recognized abbreviation of University of Warsaw.
        /// 
        /// If multiple well-recognized abbreviations exist, then it is advised to use
        /// either the most well-recognized, or the "most international" one of those.
        /// 
        /// https://github.com/erasmus-without-paper/ewp-specs-api-institutions/issues/10
        /// </summary>
        public string abbreviation
        {
            get
            {
                return this.abbreviationField;
            }
            set
            {
                this.abbreviationField = value;
            }
        }

        /// <summary>
        /// The street address of the institution.
        ///
        /// This is the address which should work when, for example, the user pastes it
        /// into Google Maps.If this HEI has many campuses, then this address should refer
        /// to a "main" campus.If this HEI doesn't have a "main" campus, then the address
        /// should simply contain the name of the institution, a city, and a country.In
        /// extreme cases, even a single country entry is better than nothing.Also see
        /// a related discussion here:
        /// 
        /// https://github.com/erasmus-without-paper/ewp-specs-api-ounits/issues/2#issuecomment-266775582
        /// 
        /// This is the primary address.Note, that more addresses may be provided by using
        /// the "contact" element.
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("street-address", Namespace = "https://github.com/erasmus-without-paper/ewp-specs-types-address/tree/stable-v1")]
        public streetaddress streetaddress
        {
            get
            {
                return this.streetaddressField;
            }
            set
            {
                this.streetaddressField = value;
            }
        }

        /// <summary>
        /// The postal address of the institution. It MAY be the same as street-address,
        /// but doesn't have to be (e.g. it can be a PO box).
        /// 
        /// This is the primary address.Note, that more addresses may be provided by using
        /// the "contact" element.
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("mailing-address", Namespace = "https://github.com/erasmus-without-paper/ewp-specs-types-address/tree/stable-v1")]
        public mailingaddress mailingaddress
        {
            get
            {
                return this.mailingaddressField;
            }
            set
            {
                this.mailingaddressField = value;
            }
        }

        /// <summary>
        /// Primary website of the institution.
        ///
        /// The xml:lang attribute, if given, SHOULD represent the language of this page.
        /// Multiple URLs can be provided (with different xml:lang values). It is also
        /// perfectly okay to provide a single URL which dynamically detects the viewer's
        ///  language preferences (in this case, no xml:lang attribute should be given).
        /// 
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("website-url")]
        public List<HeiWebsiteurl> websiteurl
        {
            get
            {
                return this.websiteurlField;
            }
            set
            {
                this.websiteurlField = value;
            }
        }

        /// <summary>
        /// If given, this should be a HTTPS URL pointing to the institution's logo.
        /// It does not necessarily have to be hosted on the same domain as the API.
        /// 
        /// This URL MUST be publicly accessible to all requests made directly from
        /// students' browsers (as opposed to being accessible for EWP requesters only). It
        /// is RECOMMENDED that the server uses proper cache-busting techniques to ensure
        /// efficient propagation of updated content.
        /// 
        /// Preferably, this should be an SVG, PNG or JPEG file, in "squarish" (~1:1)
        /// dimensions ratio, and a resolution suitable for both display and printing, on
        /// white or transparent background, with no margins included(clients are advised
        /// to add proper margins before the logo is displayed).
        /// 
        /// See resources/logo-examples directory for some examples of valid logos.You
        /// will find it in the Institutions API specs page:
        /// 
        /// https://github.com/erasmus-without-paper/ewp-specs-api-institutions
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("logo-url")]
        public string logourl
        {
            get
            {
                return this.logourlField;
            }
            set
            {
                this.logourlField = value;
            }
        }

        /// <summary>
        /// This URL is the most important one for the incoming students (much more
        /// important than website-url above). Server developers SHOULD provide it(and
        ///keep it updated).
        ///
        /// This URL MUST be publicly accessible to all requests made directly from
        /// students' browsers (as opposed to being accessible for EWP requesters only). It
        /// is RECOMMENDED that the server uses proper cache-busting techniques to ensure
        /// efficient propagation of updated content.
        ///
        /// It SHOULD refer to either a PDF file, or a HTML website, or both(via two
        /// separate factsheet-url elements). Other formats are also permitted, but
        /// discouraged(discuss here:
        /// https://github.com/erasmus-without-paper/ewp-specs-api-institutions/issues/3).
        /// The document should contain all information the incoming student should know
        /// before he applies for the mobility on this HEI. (Note, that this information is
        /// NOT connected to any specific IIAs - this is an introductory document and its
        /// scope is very broad; it is designed to be of use to *any* incoming student.)
        ///
        /// Elements SHOULD have an xml:lang attribute, and at least one URL should refer
        /// to a factsheet written in English.
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("mobility-factsheet-url")]
        public HeiMobilityfactsheeturl[] mobilityfactsheeturl
        {
            get
            {
                return this.mobilityfactsheeturlField;
            }
            set
            {
                this.mobilityfactsheeturlField = value;
            }
        }

        /// <summary>
        /// Course catalogue of the institution.
        ///
        /// The xml:lang attribute, if given, SHOULD represent the language of this page.
        /// Multiple URLs can be provided (with different xml:lang values). It is also
        /// perfectly okay to provide a single URL which dynamically detects the viewer's
        /// language preferences (in this case, no xml:lang attribute should be given).
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("course-catalogue-url")]
        public HeiCoursecatalogueurl[] coursecatalogueurl
        {
            get
            {
                return this.coursecatalogueurlField;
            }
            set
            {
                this.coursecatalogueurlField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("contact", Namespace = "https://github.com/erasmus-without-paper/ewp-specs-types-contact/tree/stable-v1")]
        public Contact[] contact
        {
            get
            {
                return this.contactField;
            }
            set
            {
                this.contactField = value;
            }
        }

        /// <summary>
        /// If this HEI implements Organizational Units API in a tree-like format, then
        /// this element should contain the ID of the root unit(the unit which represents
        /// the entire institution).
        ///
        /// Note that it is NOT required to expose units as a tree-like structure.Clients
        /// MUST be able to handle both cases (e.g.convert flat list of units into a
        /// "fake" tree).
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("root-ounit-id")]
        public string rootounitid
        {
            get
            {
                return this.rootounitidField;
            }
            set
            {
                this.rootounitidField = value;
            }
        }

        /// <summary>
        /// A list of IDs of all significant organizational units. Clients can fetch more
        /// information on these via the Organizational Units API.
        /// 
        /// If this HEI implements Organizational Units API in a tree-like format, then
        /// 
        /// this list should contain IDs of all *exposed* nodes in the tree (including the 
        /// root node).
        /// 
        /// It is not required to expose all units.Servers may choose which units are
        /// relevant for EWP data exchange (and, in fact, they often SHOULD limit what they
        /// expose, to avoid clutter in client user interfaces).
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("ounit-id")]
        public string[] ounitid
        {
            get
            {
                return this.ounitidField;
            }
            set
            {
                this.ounitidField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://github.com/erasmus-without-paper/ewp-specs-api-institutions/tree/stable-v" +
        "2")]
    public partial class HeiOtherid
    {

        private string typeField;

        private string valueField;

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

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://github.com/erasmus-without-paper/ewp-specs-api-institutions/tree/stable-v" +
        "2")]
    public partial class HeiName
    {

        private string langField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/XML/1998/namespace")]
        public string lang
        {
            get
            {
                return this.langField;
            }
            set
            {
                this.langField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://github.com/erasmus-without-paper/ewp-specs-api-institutions/tree/stable-v" +
        "2")]
    public partial class HeiWebsiteurl
    {

        private string langField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/XML/1998/namespace")]
        public string lang
        {
            get
            {
                return this.langField;
            }
            set
            {
                this.langField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://github.com/erasmus-without-paper/ewp-specs-api-institutions/tree/stable-v" +
        "2")]
    public partial class HeiMobilityfactsheeturl
    {

        private string langField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/XML/1998/namespace")]
        public string lang
        {
            get
            {
                return this.langField;
            }
            set
            {
                this.langField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://github.com/erasmus-without-paper/ewp-specs-api-institutions/tree/stable-v" +
        "2")]
    public partial class HeiCoursecatalogueurl
    {

        private string langField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/XML/1998/namespace")]
        public string lang
        {
            get
            {
                return this.langField;
            }
            set
            {
                this.langField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

   

    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://github.com/erasmus-without-paper/ewp-specs-types-phonenumber/tree/stable-" +
       "v1")]
    [System.Xml.Serialization.XmlRootAttribute("phone-number", Namespace = "https://github.com/erasmus-without-paper/ewp-specs-types-phonenumber/tree/stable-" +
       "v1", IsNullable = false)]
    public partial class phonenumber
    {

        private string e164Field;

        private string extField;

        private string otherformatField;

        /// <remarks/>
        public string e164
        {
            get
            {
                return this.e164Field;
            }
            set
            {

                this.e164Field = value;

                //Pattern = "\\+[0-9]{1,15}")

                //this.e164Field = value; string pattern = "+[0-9]{1,15}";
                //if (Regex.IsMatch(value, pattern))
                //    this.e164Field = value;
                //else
                //    throw new EwpApi.Service.Exception.EwpSecWebApplicationException("Phone number format is not acceptable", System.Net.HttpStatusCode.BadRequest);
            }
        }

        /// <remarks/>
        public string ext
        {
            get
            {
                return this.extField;
            }
            set
            {
                this.extField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("other-format")]
        public string otherformat
        {
            get
            {
                return this.otherformatField;
            }
            set
            {
                this.otherformatField = value;
            }
        }
    }
   
}
