using EwpApi.Dto.ewp_specs_api_institutions;
using EwpApi.Dto.ewp_specs_architecture;
using EwpApi.Dto.ewp_specs_types_address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EwpApi.Dto.ewp_specs_types_contact
{
    /// <summary>
    /// A recommended name for the elements with the Contact type content.
    /// 
    /// You might not want to use this element directly if your contact is supposed to
    /// 
    /// have a very specific role.In such cases, it might be better to name your
    /// element appropriately (e.g. "coordinator"), and reuse (or extend?) the Contact
    /// type.
    /// 
    /// </summary>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://github.com/erasmus-without-paper/ewp-specs-types-contact/tree/stable-v1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "https://github.com/erasmus-without-paper/ewp-specs-types-contact/tree/stable-v1", IsNullable = false)]
    public partial class Contact
    {

        private string[] contactnameField;

        private string[] persongivennamesField;

        private string[] personfamilynameField;

        private Gender persongenderField;

        //private bool persongenderFieldSpecified;

        private List<phonenumber> phonenumberField;

        private object[] faxnumberField;

        private string[] emailField;

        private mailingaddress mailingaddressField;

        private streetaddress streetaddressField;

        private MultilineStringWithOptionalLang roledescriptionField;

        /// <summary>
        /// The name of the contact, e.g. "John Doe", or "IRO Office", or "John Doe (Head
        /// of the IRO Office)". It is entirely up to the server implementers to decide on
        /// how this contact should be labeled. This name MAY refer to a specific person,
        /// an office, a place, a role, etc.The only requirement is that it MUST be
        /// descriptive for the human reader, in its context.
        ///
        /// Server implementers are required to specify a value for this element even when
        /// they provide `person-given-names` and `person-family-name`. It might seem
        /// redundant, but in some contexts it is useful - for example, server implementers
        /// are allowed to use `contact-name` to suggest how the name should be displayed -
        /// with proper titles, proper order of middle names, key role etc., while they are
        /// not allowed to do this in `person-given-names` and `person-family-name`.
        ///
        /// All `*-name` elements MAY be specified in multiple languages (and/or multiple
        /// scripts). It is RECOMMENDED that English (`en`) should come first, other
        /// latin-alphabet entries should follow(e.g. `ru-Latn`), and non-latin alphabets
        /// should come last(e.g. `ru-Cyrl`, `ru`).
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("contact-name")]
        public string[] contactname
        {
            get
            {
                return this.contactnameField;
            }
            set
            {
                this.contactnameField = value;
            }
        }

        /// <summary>
        /// If this contact is a person, then this element contains the given names of this
        /// person.
        /// 
        ///  It MAY be specified in multiple languages(and/or multiple alphabets). See
        /// annotations on `contact-name` for more information.
        /// 
        ///  Note, that it is allowed (but NOT RECOMMENDED) for a person to have a family
        /// name in certain language/alphabet, but also be missing the given names in this
        /// language/alphabet.Clients will need to cope with all such possible
        /// combinations.
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("person-given-names")]
        public string[] persongivennames
        {
            get
            {
                return this.persongivennamesField;
            }
            set
            {
                this.persongivennamesField = value;
            }
        }

        /// <summary>
        /// If this contact is a person, then this element contains the family name of this
        ///  person.
        /// 
        ///  It MAY be specified in multiple languages(and/or multiple alphabets). See
        /// annotations on `contact-name` for more information.
        /// 
        ///  Note, that it is allowed (but NOT RECOMMENDED) for a person to have a family
        /// name in certain language/alphabet, but also be missing the given names in this
        /// language/alphabet.Clients will need to cope with all such possible
        /// combinations.
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("person-family-name")]
        public string[] personfamilyname
        {
            get
            {
                return this.personfamilynameField;
            }
            set
            {
                this.personfamilynameField = value;
            }
        }

        /// <summary>
        /// If this contact is a person, then this element MAY contain the gender of this person.
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("person-gender")]
        public Gender persongender
        {
            get
            {
                return this.persongenderField;
            }
            set
            {
                this.persongenderField = value;
            }
        }

        /// <summary>
        /// A list of phone numbers at which this contact can be reached.
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("phone-number", Namespace = "https://github.com/erasmus-without-paper/ewp-specs-types-phonenumber/tree/stable-v1")]
        public List<phonenumber> phonenumber
        {
            get
            {
                return this.phonenumberField;
            }
            set
            {
                this.phonenumberField = value;
            }
        }

        /// <summary>
        /// A list of fax numbers at which this contact can be reached.
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("fax-number", Namespace = "https://github.com/erasmus-without-paper/ewp-specs-types-phonenumber/tree/stable-v1")]
        public object[] faxnumber
        {
            get
            {
                return this.faxnumberField;
            }
            set
            {
                this.faxnumberField = value;
            }
        }

        /// <summary>
        /// A list of email addresses at which this contact can be reached.
        ///
        ///  Servers SHOULD try to supply this list, even if it doesn't seem to be necessary
        /// in the particular context in which you are using the Abstract Contact data
        /// type.Some clients might need to uniquely identify this contact (not only a
        /// person, but also a broader contact entity), and currently email address seems
        /// to be the best(easiest) type of identifier for this.
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("email")]
        public string[] email
        {
            get
            {
                return this.emailField;
            }
            set
            {
                this.emailField = value;
            }
        }

        /// <summary>
        /// Street address of the place where the contact can be found (room number, floor, etc.)
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
        /// A postal address at which people should send paper documents for this contact.
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
        /// Optional description of the "roles" of this contact, provided in multiple
        /// languages.It MAY be quite long (multiple paragraphs). It MAY be auto-generated
        /// from the computer system, but it also MAY be provided by the contact-person
        /// himself/herself.
        ///
        /// It is RECOMMENDED to provide role-description in contexts where the role of the
        /// contact is not otherwise specified.This description should answer the
        /// following question: "When this person/office should be contacted?". Client
        /// developers may, for example, display this information in a tooltip, next to the
        /// contact name.
        ///
        /// Examples:
        /// "Responsible for handling incoming students from Spain."
        /// "Responsible for handling Interinstitutional Agreements with Norway and Sweden."
        /// "Dear students! Don't hesitate to contact me directly if..."
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("role-description", Namespace = "https://github.com/erasmus-without-paper/ewp-specs-types-contact/tree/stable-v1" /*"https://github.com/erasmus-without-paper/ewp-specs-types-contact/tree/stable-v1"*/)]
        public MultilineStringWithOptionalLang roledescription
        {
            get
            {
                return this.roledescriptionField;
            }
            set
            {
                this.roledescriptionField = value;
            }
        }
    }
}
