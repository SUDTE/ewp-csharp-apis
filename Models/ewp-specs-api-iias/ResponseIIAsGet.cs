using EwpApi.Dto.ewp_specs_types_contact;
using EwpApi.Helper;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace EwpApi.Dto.ewp_specs_api_iias
{
    
    [System.Xml.Serialization.XmlRootAttribute("iias-get-response", Namespace = "https://github.com/erasmus-without-paper/ewp-specs-api-iias/blob/stable-v6/endpoints/get-response.xsd", IsNullable = false)]
    public class ResponseIIAsGet : XmlBodyGenerator, IResponse
    {
        [XmlNamespaceDeclarations]
        public XmlSerializerNamespaces xmlns = new XmlSerializerNamespaces();

        /// <summary>
        /// This describes the format of the response returned by the `get` endpoint of
        /// EWP Interinstitutional Agreements API.
        /// </summary>
        public ResponseIIAsGet()
        {
            xmlns.Add("xml", "http://www.w3.org/XML/1998/namespace");
            xmlns.Add("xs", "http://www.w3.org/2001/XMLSchema");
            xmlns.Add("ewp", "https://github.com/erasmus-without-paper/ewp-specs-architecture/blob/stable-v1/common-types.xsd");
            xmlns.Add("a", "https://github.com/erasmus-without-paper/ewp-specs-types-address/tree/stable-v1");
            xmlns.Add("p", "https://github.com/erasmus-without-paper/ewp-specs-types-phonenumber/tree/stable-v1");
            xmlns.Add("c", "https://github.com/erasmus-without-paper/ewp-specs-types-contact/tree/stable-v1");
            xmlns.Add("trm", "https://github.com/erasmus-without-paper/ewp-specs-types-academic-term/tree/stable-v1");
            xmlns.Add("tns", "https://github.com/erasmus-without-paper/ewp-specs-api-iias/blob/stable-v2/endpoints/get-response.xsd");
        }

        public async Task WriteXmlBody(HttpResponse response)
        {
            await WriteXmlBodyIntoResponse<ResponseIIAsGet>(this, response);
        }

        public string ToXml()
        {
            return GenerateXml<ResponseIIAsGet>(this);
        }


        private List<IIA> iiaField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("iia")]
        public List<IIA> iia
        {
            get
            {
                return this.iiaField;
            }
            set
            {
                this.iiaField = value;
            }
        }
    }

    /// <summary>
    /// This represents a single IIA. Servers will produce one such element for
    /// each of the `iia_id` or `iia_code` values passed in the Institutions API call.
    /// </summary>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://github.com/erasmus-without-paper/ewp-specs-api-iias/blob/stable-v6/endpoints/get-response.xsd")]
    public partial class IIA
    {

        private List<IIAPartner> partnerField;

        private bool ineffectField;

        private IIACooperationConditions cooperationconditionsField;

        private string conditionshashField;

        private string pdfField;

        ///<summary>
        ///A list of HEIs participating in this IIA.
        ///There MUST be two partner HEI. 

        ///This list includes the local HEI which MUST be the first element on this list.
        ///In other words:        
        ///      * The value of `hei-id` of the first `partner` MUST match the value passed in
        ///  the `hei_id` request parameter,
        ///
        ///      * Both `iia-id` and `iia-code` elements MUST be present in the first `partner`
        ///  element (even though it's not required by the schema itself), and one of them
        ///
        ///  MUST match one of the values passed in the `iia_id` or `iia_code` request
        ///  parameter.
        ///
        ///      * The server will usually fill much more data for the first `partner`.
        ///</summary>
        [System.Xml.Serialization.XmlElementAttribute("partner")]
        public List<IIAPartner> Partner
        {
            get
            {
                return this.partnerField;
            }
            set
            {
                this.partnerField = value;
            }
        }

        ///<summary>
        ///Boolean. True, if this IIA *is* or *once was* in effect - that is, it has been 
        ///signed, and the partners are(or were) following its rules.False, if this IIA 
        ///is a draft or proposal only(and it hasn't been agreed on).
        ///</summary>
        [System.Xml.Serialization.XmlElementAttribute("in-effect")]
        public bool ineffect
        {
            get
            {
                return this.ineffectField;
            }
            set
            {
                this.ineffectField = value;
            }
        }

        /// <summary>
        /// List of all cooperation conditions defined in this agreement.
        ///
        ///If you are sending conditions-hash, be consistent in ordering cooperation conditions.
        ///The hash SHOULD NOT change if the cooperation conditions are not really changing.
        ///An exception to this are the `sending-contact` and `receiving-contact` subelements
        ///that are not taken into account when calculating hash.
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("cooperation-conditions")]
        public IIACooperationConditions cooperationconditions
        {
            get
            {
                return this.cooperationconditionsField;
            }
            set
            {
                this.cooperationconditionsField = value;
            }
        }

        /// <summary>
        /// The SHA-256 digest of the cooperation-conditions element but *excluding*
        /// `sending-contact` and `receiving-contact` subelements.Before
        /// calculating the hash, the cooperation-conditions element should be normalized
        /// using Exclusive XML Canonicalization.
        ///
        /// Please be aware that XML Canonicalization does not imply that spaces are removed:
        /// https://www.w3.org/TR/xml-c14n2/#sec-Requirements-Robustness
        ///
        /// This element is not required. However, if it is not present, your partner
        /// will not be able to approve your version of the agreement using EWP IIAs Approval API.
        /// If you want to get approval of your agreements, then you should always send
        /// the conditions hash.
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("conditions-hash")]
        public string conditionshash
        {
            get
            {
                return this.conditionshashField;
            }
            set
            {
                this.conditionshashField = value;
            }
        }

        /// <summary>
        ///  PDF version of the agreement. SHOULD be skipped if the `send_pdf` request parameter
        /// value is `false`.
        /// 
        /// Notes for client implementers:
        /// 
        /// The pdf element can be missing even if the `send_pdf` parameter was set to true.
        /// Some servers MAY not support PDFs at all or the PDF version can be not ready yet.
        /// 
        /// For security reasons, you may consider checking the content type of the file
        /// before displaying it in the browser.
        /// </summary>
        public string pdf
        {
            get
            {
                return this.pdfField;
            }
            set
            {
                this.pdfField = value;
            }
        }
    }

    /// <summary>
    /// A list of HEIs participating in this IIA.
    /// 
    /// This list includes the local HEI which MUST be the first element on this list.
    /// In other words:
    /// 
    /// * The value of `hei-id` of the first `partner` MUST match the value passed in
    ///   the `hei_id` request parameter,
    /// 
    /// 
    /// * Both `iia-id` and `iia-code` elements MUST be present in the first `partner`
    /// 
    ///   element (even though it's not required by the schema itself), and one of them
    /// 
    ///   MUST match one of the values passed in the `iia_id` or `iia_code` request
    ///   parameter.
    /// 
    /// * The server will usually fill much more data for the first `partner`.
    /// </summary>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://github.com/erasmus-without-paper/ewp-specs-api-iias/blob/stable-v6/endpoints/get-response.xsd")]
    public partial class IIAPartner
    {

        private string heiidField;

        private string ounitidField;

        private string iiaidField;

        private string iiacodeField;

        private Contact signingcontactField;

        private System.DateTime signingdateField;        

        private Contact[] contactField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("hei-id")]
        public string HeiId
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
        /// Optional organizational unit surrogate ID. If given, then it refers to the unit
        /// within the partner HEI's organizational structure, which is the actual partner
        /// of this agreement.Agreements can be signed between units, not only between
        /// HEIs: https://github.com/erasmus-without-paper/ewp-specs-api-iias/issues/11
        ///
        /// If provided, then it MUST have the value of the "official" ounit-id, exactly as
        /// it has been assigned by the *partner HEI* in its Organizational Units API. If
        /// this official ID is not known by the server (and it often isn't), then this
        /// element MUST be skipped. This is a surrogate ID, so it SHOULD NOT be displayed
        /// to the user (use `ounit-code` for that).
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("ounit-id")]
        public string OUnitId
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

        /// <summary>
        /// The partner's unique surrogate ID of this IIA. This is a surrogate ID, so it
        /// SHOULD NOT be displayed to the user(use `iia-code` for that).
        ///
        /// Since IIA IDs are local(unique within a single HEI, but not within the world),
        /// each partner is allowed to have his own ID for the same IIA.If this server is
        /// aware of the IDs used by the other partners, then it MUST output it here
        /// (otherwise, a copy of the agreement in our system could be treated
        /// as a separate different agreement in partner's system).
        ///
        /// Server implementers MUST use immutable surrogate keys for their work with EWP.
        /// https://github.com/erasmus-without-paper/ewp-specs-architecture#ids
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("iia-id")]
        public string IIAId
        {
            get
            {
                return this.iiaidField;
            }
            set
            {
                this.iiaidField = value;
            }
        }

        /// <summary>
        /// The partner's "human readable" ID of this IIA (aka an agreement number). If
        /// this server is aware of the agreement numbers used by the other partners, then
        /// it SHOULD output it here.
        ///
        /// Since `iia-id` values should contain surrogate identifiers(and, as such,
        /// should not be displayed to the user), we require additional "human readable"
        /// agreement codes/numbers to be provided here.These codes SHOULD be displayed to
        /// the user, and they MAY be used for searching, but they are* not used* to
        /// directly identify entities in EWP network.
        ///
        /// Related links:
        /// https://github.com/erasmus-without-paper/ewp-specs-architecture#ids
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("iia-code")]
        public string IIACode
        {
            get
            {
                return this.iiacodeField;
            }
            set
            {
                this.iiacodeField = value;
            }
        }

        /// <summary>
        /// This describes that person who is a partner's signer of this IIA (if known).
        /// Most often it is an institutional coordinator for this IIA,
        /// but it could also be, for example, the dean of the faculty.
       /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("signing-contact")]
        public Contact SigningContact
        {
            get
            {
                return this.signingcontactField;
            }
            set
            {
                this.signingcontactField = value;
            }
        }

        /// <summary>
        /// The date when the agreement has been first signed by the *partner's*
        /// institutional coordinator(if known).
        ///
        /// Note, that agreements are often modified* after* they were signed.These
        /// modifications don't usually require official signatures, though. This date
        /// refers to the "first signing" only.
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("signing-date", DataType = "date")]
        public System.DateTime SigningDate
        {
            get
            {
                return this.signingdateField;
            }
            set
            {
                this.signingdateField = value;
            }
        }

        /// <summary>
        /// A list of other partner contacts related to this IIA (or to mobilities related to this IIA).
        ///  Only some servers provide these, even for the *local* partner. Many HEIs
        /// (especially the smaller ones) don't granulate their contacts in such a detailed
        /// way.Instead, they have a fixed set of contacts described in their Institutions API.
        ///
        ///
        /// These contacts take precedence over contacts defined in the Institutions API
        ///
        /// and Organization Units API for the partner HEI / unit.Clients are advised to
        ///
        /// display these contacts in a separate section above other contacts - so that the
        ///
        /// users will notice them first(before scrolling down to other, more generic
        ///
        /// contacts).
        /// </summary>
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
    }

    ///// <remarks/>
    //[System.SerializableAttribute()]
    //[System.ComponentModel.DesignerCategoryAttribute("code")]
    //[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://github.com/erasmus-without-paper/ewp-specs-api-iias/blob/stable-v6/endpoints/get-response.xsd")]
    //public partial class IIAPartnerSigningContact
    //{

    //    private string contactnameField;

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlElementAttribute("contact-name", Namespace = "https://github.com/erasmus-without-paper/ewp-specs-types-contact/tree/stable-v1")]
    //    public string contactname
    //    {
    //        get
    //        {
    //            return this.contactnameField;
    //        }
    //        set
    //        {
    //            this.contactnameField = value;
    //        }
    //    }
    //}

    ///// <remarks/>
    //[System.SerializableAttribute()]
    //[System.ComponentModel.DesignerCategoryAttribute("code")]
    //[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://github.com/erasmus-without-paper/ewp-specs-types-contact/tree/stable-v1")]
    //[System.Xml.Serialization.XmlRootAttribute(Namespace = "https://github.com/erasmus-without-paper/ewp-specs-types-contact/tree/stable-v1", IsNullable = false)]
    //public partial class contact
    //{

    //    private string contactnameField;

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlElementAttribute("contact-name")]
    //    public string contactname
    //    {
    //        get
    //        {
    //            return this.contactnameField;
    //        }
    //        set
    //        {
    //            this.contactnameField = value;
    //        }
    //    }
    //}

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://github.com/erasmus-without-paper/ewp-specs-api-iias/blob/stable-v6/endpoints/get-response.xsd")]
    public partial class IIACooperationConditions
    {

        private List<StudentStudiesMobility> studentstudiesmobilityspecField;

        private List<StudentTraineeshipMobility> studenttraineeshipmobilityspecField;

        private List<StaffTeacherMobility> staffteachermobilityspecField;

        private List<StaffTrainingMobility> stafftrainingmobilityspecField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("student-studies-mobility-spec")]
        public List<StudentStudiesMobility> studentstudiesmobilityspec
        {
            get
            {
                return this.studentstudiesmobilityspecField;
            }
            set
            {
                this.studentstudiesmobilityspecField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("student-traineeship-mobility-spec")]
        public List<StudentTraineeshipMobility> studenttraineeshipmobilityspec
        {
            get
            {
                return this.studenttraineeshipmobilityspecField;
            }
            set
            {
                this.studenttraineeshipmobilityspecField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("staff-teacher-mobility-spec")]
        public List<StaffTeacherMobility> staffteachermobilityspec
        {
            get
            {
                return this.staffteachermobilityspecField;
            }
            set
            {
                this.staffteachermobilityspecField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("staff-training-mobility-spec")]
        public List<StaffTrainingMobility> stafftrainingmobilityspec
        {
            get
            {
                return this.stafftrainingmobilityspecField;
            }
            set
            {
                this.stafftrainingmobilityspecField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://github.com/erasmus-without-paper/ewp-specs-api-iias/blob/stable-v6/endpoints/get-response.xsd")]
    public partial class StudentStudiesMobility: StudentMobilitySpecification
    {

        //private string sendingheiidField;

        //private string receivingheiidField;

        //private string receivingacademicyearidField;

        //private decimal totalmonthsperyearField;

        //private bool blendedField;

        ///// <remarks/>
        //[System.Xml.Serialization.XmlElementAttribute("sending-hei-id")]
        //public string SendingHeiId
        //{
        //    get
        //    {
        //        return this.sendingheiidField;
        //    }
        //    set
        //    {
        //        this.sendingheiidField = value;
        //    }
        //}

        ///// <remarks/>
        //[System.Xml.Serialization.XmlElementAttribute("receiving-hei-id")]
        //public string ReceivinghHeiId
        //{
        //    get
        //    {
        //        return this.receivingheiidField;
        //    }
        //    set
        //    {
        //        this.receivingheiidField = value;
        //    }
        //}

        ///// <remarks/>
        //[System.Xml.Serialization.XmlElementAttribute("receiving-academic-year-id")]
        //public string receivingacademicyearid
        //{
        //    get
        //    {
        //        return this.receivingacademicyearidField;
        //    }
        //    set
        //    {
        //        this.receivingacademicyearidField = value;
        //    }
        //}

        ///// <remarks/>
        //[System.Xml.Serialization.XmlElementAttribute("total-months-per-year")]
        //public decimal totalmonthsperyear
        //{
        //    get
        //    {
        //        return this.totalmonthsperyearField;
        //    }
        //    set
        //    {
        //        this.totalmonthsperyearField = value;
        //    }
        //}

        ///// <remarks/>
        //public bool blended
        //{
        //    get
        //    {
        //        return this.blendedField;
        //    }
        //    set
        //    {
        //        this.blendedField = value;
        //    }
        //}
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://github.com/erasmus-without-paper/ewp-specs-api-iias/blob/stable-v6/endpoints/get-response.xsd")]
    public partial class StudentTraineeshipMobility
    {

        private string sendingheiidField;

        private string receivingheiidField;

        private string receivingacademicyearidField;

        private decimal totalmonthsperyearField;

        private bool blendedField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("sending-hei-id")]
        public string sendingheiid
        {
            get
            {
                return this.sendingheiidField;
            }
            set
            {
                this.sendingheiidField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("receiving-hei-id")]
        public string receivingheiid
        {
            get
            {
                return this.receivingheiidField;
            }
            set
            {
                this.receivingheiidField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("receiving-academic-year-id")]
        public string receivingacademicyearid
        {
            get
            {
                return this.receivingacademicyearidField;
            }
            set
            {
                this.receivingacademicyearidField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("total-months-per-year")]
        public decimal totalmonthsperyear
        {
            get
            {
                return this.totalmonthsperyearField;
            }
            set
            {
                this.totalmonthsperyearField = value;
            }
        }

        /// <remarks/>
        public bool blended
        {
            get
            {
                return this.blendedField;
            }
            set
            {
                this.blendedField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://github.com/erasmus-without-paper/ewp-specs-api-iias/blob/stable-v6/endpoints/get-response.xsd")]
    public partial class StaffTeacherMobility
    {

        private string sendingheiidField;

        private string receivingheiidField;

        private string receivingacademicyearidField;

        private decimal totaldaysperyearField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("sending-hei-id")]
        public string sendingheiid
        {
            get
            {
                return this.sendingheiidField;
            }
            set
            {
                this.sendingheiidField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("receiving-hei-id")]
        public string receivingheiid
        {
            get
            {
                return this.receivingheiidField;
            }
            set
            {
                this.receivingheiidField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("receiving-academic-year-id")]
        public string receivingacademicyearid
        {
            get
            {
                return this.receivingacademicyearidField;
            }
            set
            {
                this.receivingacademicyearidField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("total-days-per-year")]
        public decimal totaldaysperyear
        {
            get
            {
                return this.totaldaysperyearField;
            }
            set
            {
                this.totaldaysperyearField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://github.com/erasmus-without-paper/ewp-specs-api-iias/blob/stable-v6/endpoints/get-response.xsd")]
    public partial class StaffTrainingMobility
    {

        private string sendingheiidField;

        private string receivingheiidField;

        private string receivingacademicyearidField;

        private decimal totaldaysperyearField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("sending-hei-id")]
        public string sendingheiid
        {
            get
            {
                return this.sendingheiidField;
            }
            set
            {
                this.sendingheiidField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("receiving-hei-id")]
        public string receivingheiid
        {
            get
            {
                return this.receivingheiidField;
            }
            set
            {
                this.receivingheiidField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("receiving-academic-year-id")]
        public string receivingacademicyearid
        {
            get
            {
                return this.receivingacademicyearidField;
            }
            set
            {
                this.receivingacademicyearidField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("total-days-per-year")]
        public decimal totaldaysperyear
        {
            get
            {
                return this.totaldaysperyearField;
            }
            set
            {
                this.totaldaysperyearField = value;
            }
        }
    }

    ///// <remarks/>
    //[System.SerializableAttribute()]
    //[System.ComponentModel.DesignerCategoryAttribute("code")]
    //[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://github.com/erasmus-without-paper/ewp-specs-api-iias/blob/stable-v6/endpoints/get-response.xsd")]
    //public partial class MobilitySpecification
    //{
    //    private string sendingheiidField;

    //    private string sendingheiounitidField;

    //    private List<Contact> sendingcontactField;

    //    private string receivingheiidField;

    //    private string receivingheiounitidField;

    //    private List<Contact> receivingcontactField;

    //    private List<string> receivingacademicyearidField;

    //    private int mobilitiesperyearField;

    //    //private decimal totaldaysperyearField;

    //    private List<string> recommendedlanguageskillField;

    //    private List<string> subjectareaField;

    //    private string otherinfotermsField;

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlElementAttribute("sending-hei-id")]
    //    public string sendingheiid
    //    {
    //        get
    //        {
    //            return this.sendingheiidField;
    //        }
    //        set
    //        {
    //            this.sendingheiidField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlElementAttribute("sending-ounit-id")]
    //    public string SendingHeiOunitId
    //    {
    //        get
    //        {
    //            return this.sendingheiounitidField;
    //        }
    //        set
    //        {
    //            this.sendingheiounitidField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlElementAttribute("sending-contact")]
    //    public List<Contact> SendingContact
    //    {
    //        get
    //        {
    //            return this.sendingcontactField;
    //        }
    //        set
    //        {
    //            this.sendingcontactField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlElementAttribute("receiving-hei-id")]
    //    public string receivingheiid
    //    {
    //        get
    //        {
    //            return this.receivingheiidField;
    //        }
    //        set
    //        {
    //            this.receivingheiidField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlElementAttribute("sending-hei-id")]
    //    public string sendingheiid
    //    {
    //        get
    //        {
    //            return this.sendingheiidField;
    //        }
    //        set
    //        {
    //            this.sendingheiidField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlElementAttribute("receiving-ounit-id")]
    //    public string ReceivingHeiOunitId
    //    {
    //        get
    //        {
    //            return this.sendingheiounitidField;
    //        }
    //        set
    //        {
    //            this.sendingheiounitidField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlElementAttribute("receiving-contact")]
    //    public List<Contact> SendingContact
    //    {
    //        get
    //        {
    //            return this.sendingcontactField;
    //        }
    //        set
    //        {
    //            this.sendingcontactField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlElementAttribute("receiving-academic-year-id")]
    //    public string receivingacademicyearid
    //    {
    //        get
    //        {
    //            return this.receivingacademicyearidField;
    //        }
    //        set
    //        {
    //            this.receivingacademicyearidField = value;
    //        }
    //    }
    //}
}



