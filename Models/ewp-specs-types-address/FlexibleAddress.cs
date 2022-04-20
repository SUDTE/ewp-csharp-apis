using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace EwpApi.Dto.ewp_specs_types_address
{
    public partial class FlexibleAddress 
    {
        /// <summary>A collection of <see cref="System.String" /></summary>
        /// <remarks>
        ///   <br/>
        ///       The name of the addressed entity (usually a person). In case of postal<br/>
        ///       addresses, this name MAY be formatted in some custom way (preferred by this<br/>
        ///       entity to be printed on an envelope).<br/>
        ///   <br/>
        ///       Note, that depending on the context inside which FlexibleAddress is being<br/>
        ///       embedded, this field might feel redundant (as its value could be inferred from<br/>
        ///       other properties of the parent element). Still, we believe that in some cases,<br/>
        ///       both server and client developers might want to make use of it. See discussion<br/>
        ///       here:<br/>
        ///   <br/>
        ///       https://github.com/erasmus-without-paper/ewp-specs-types-address/issues/3<br/>
        ///   
        /// </remarks>
        [System.Xml.Serialization.XmlElementAttribute("recipientName", Namespace = "https://github.com/erasmus-without-paper/ewp-specs-types-address/tree/stable-v1")]
        public List<System.String> RecipientNames { get; set; } = new List<System.String>();

        /// <summary>A collection of <see cref="System.String" /></summary>
        /// <remarks>
        ///   <br/>
        ///       OPTION 1. The simple format. A denormalized sequence of addressLines.<br/>
        ///       Each line is formatted with all of its pieces in their proper place. This<br/>
        ///       includes all of the necessary punctuation.<br/>
        ///   <br/>
        ///       This form of address is not intended to be parsed, it is used for delivery by<br/>
        ///       the postal service. They usually contain information such as street name,<br/>
        ///       building number, building name (such as the name of a faculty), a post-office<br/>
        ///       box number, etc. They SHOULD NOT contain values defined in `locality`,<br/>
        ///       `postalCode`, `region` and `country` (you should provide those in their<br/>
        ///       respective elements if you can).<br/>
        ///   <br/>
        ///       It is recommended for server developers to provide this data in a form of<br/>
        ///       multiple lines. However, it is also allowed to have it all in a single line<br/>
        ///       (comma-separated). Same goes for client developers - if their database doesn't<br/>
        ///       accept multiple lines for some reason, they MAY combine the lines with commas<br/>
        ///       before storing them. (Such concatenated strings might not look as pretty, but<br/>
        ///       they will still be usable for mailing.)<br/>
        ///   
        /// </remarks>
        [System.Xml.Serialization.XmlElementAttribute("addressLine", Namespace = "https://github.com/erasmus-without-paper/ewp-specs-types-address/tree/stable-v1")]
        public List<System.String> AddressLines { get; set; } = new List<System.String>();

        /// <summary>A <see cref="System.String" />, Optional : null when not set</summary>
        /// <remarks>
        ///   <br/>
        ///       The number of the building or house on the street that identifies where to<br/>
        ///       deliver mail.<br/>
        ///   
        /// </remarks>
        [System.Xml.Serialization.XmlElementAttribute("buildingNumber", Namespace = "https://github.com/erasmus-without-paper/ewp-specs-types-address/tree/stable-v1")]
        public System.String BuildingNumber { get; set; }

        /// <summary>A <see cref="System.String" />, Optional : null when not set</summary>
        /// <remarks>
        ///   <br/>
        ///       The building or house name on the street that identifies where to deliver mail.<br/>
        ///   <br/>
        ///       In some areas of the world, including many remote areas, houses are not<br/>
        ///       numbered but named. Some buildings also have both - a number and a name (in<br/>
        ///       this cases it is usually enough to submit only one of them).<br/>
        ///   
        /// </remarks>
        [System.Xml.Serialization.XmlElementAttribute("buildingName", Namespace = "https://github.com/erasmus-without-paper/ewp-specs-types-address/tree/stable-v1")]
        public System.String BuildingName { get; set; }

        /// <summary>A <see cref="System.String" />, Optional : null when not set</summary>
        /// <remarks>
        ///   <br/>
        ///       The street name (or any other thoroughfare name) where the building/house is<br/>
        ///       located.<br/>
        ///   
        /// </remarks>
        [System.Xml.Serialization.XmlElementAttribute("streetName", Namespace = "https://github.com/erasmus-without-paper/ewp-specs-types-address/tree/stable-v1")]
        public System.String StreetName { get; set; }

        /// <summary>A <see cref="System.String" />, Optional : null when not set</summary>
        /// <remarks>Identifies the apartment number or office suite.</remarks>
        [System.Xml.Serialization.XmlElementAttribute("unit", Namespace = "https://github.com/erasmus-without-paper/ewp-specs-types-address/tree/stable-v1")]
        public System.String Unit { get; set; }

        /// <summary>A <see cref="System.String" />, Optional : null when not set</summary>
        /// <remarks>The floor where the housename is located.</remarks>
        [System.Xml.Serialization.XmlElementAttribute("floor", Namespace = "https://github.com/erasmus-without-paper/ewp-specs-types-address/tree/stable-v1")]
        public System.String Floor { get; set; }

        /// <summary>A <see cref="System.String" />, Optional : null when not set</summary>
        /// <remarks>Identifies a Post Office Box number.</remarks>
        [System.Xml.Serialization.XmlElementAttribute("postOfficeBox", Namespace = "https://github.com/erasmus-without-paper/ewp-specs-types-address/tree/stable-v1")]
        public System.String PostOfficeBox { get; set; }

        /// <summary>A collection of <see cref="XElement" /></summary>
        /// <remarks>
        ///   <br/>
        ///       Identifies the Dock or the Mail Stop or Lane or any other specific Delivery<br/>
        ///       Point.<br/>
        ///   
        /// </remarks>
        [System.Xml.Serialization.XmlElementAttribute("deliveryPointCode", Namespace = "https://github.com/erasmus-without-paper/ewp-specs-types-address/tree/stable-v1")]
        public List<string> DeliveryPointCodes { get; } = new List<string>();

        /// <summary>A <see cref="System.String" />, Optional : null when not set</summary>
        /// <remarks>
        ///   <br/>
        ///       The postal code. This is often (but not always) required if the address is<br/>
        ///       supposed to be a postal address. It may include dashes and other formatting<br/>
        ///       characters. Note, that in some countries it can be as long as 10 characters.<br/>
        ///   <br/>
        ///       Why minOccurs="0"? Read on here:<br/>
        ///       https://github.com/erasmus-without-paper/ewp-specs-types-address/issues/2<br/>
        ///   
        /// </remarks>
        [System.Xml.Serialization.XmlElementAttribute("postalCode", Namespace = "https://github.com/erasmus-without-paper/ewp-specs-types-address/tree/stable-v1")]
        public System.String PostalCode { get; set; }

        /// <summary>A <see cref="System.String" />, Optional : null when not set</summary>
        /// <remarks>
        ///   <br/>
        ///       The name of the settlement (city/town/village). Both postal and map localities<br/>
        ///       are allowed.<br/>
        ///   <br/>
        ///       If this address is supposed to be postal address, then it is recommended to use<br/>
        ///       a postal locality here (postal authority often situated in a nearby large town).<br/>
        ///       Note however, that a map locality is also acceptable, as the postal code will<br/>
        ///       usually resolve any problems here, to allow correct delivery even if the<br/>
        ///       official postal locality is not used.<br/>
        ///   <br/>
        ///       It is HIGHLY RECOMMENDED (but not required) for this element to exist (see<br/>
        ///       examples in README.md).<br/>
        ///   
        /// </remarks>
        [System.Xml.Serialization.XmlElementAttribute("locality", Namespace = "https://github.com/erasmus-without-paper/ewp-specs-types-address/tree/stable-v1")]
        public System.String Locality { get; set; }

        /// <summary>A <see cref="System.String" />, Optional : null when not set</summary>
        /// <remarks>
        ///   <br/>
        ///       An optional name of the region. Usually not required in most countries, it can<br/>
        ///       be any kind of administrative or postal division (such as state, province,<br/>
        ///       voivodeship, etc.)<br/>
        ///   
        /// </remarks>
        [System.Xml.Serialization.XmlElementAttribute("region", Namespace = "https://github.com/erasmus-without-paper/ewp-specs-types-address/tree/stable-v1")]
        public System.String Region { get; set; }

        /// <summary>A <see cref="System.String" />, Optional : null when not set</summary>
        /// <remarks>
        ///   <br/>
        ///       Country identifier.<br/>
        ///   <br/>
        ///       It is HIGHLY RECOMMENDED (but not required) for this element to exist (see<br/>
        ///       examples in README.md).<br/>
        ///   
        /// </remarks>
        /// LxValueType.Value, XsdType.XsdString, MinOccurs = 0, MaxOccurs = 1, Pattern = "[A-Z][A-Z]"
        [System.Xml.Serialization.XmlElementAttribute("country", Namespace = "https://github.com/erasmus-without-paper/ewp-specs-types-address/tree/stable-v1")]
        public System.String Country { get; set; }

    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://github.com/erasmus-without-paper/ewp-specs-types-address/tree/stable-v1")]
    [System.Xml.Serialization.XmlRootAttribute("street-address", Namespace = "https://github.com/erasmus-without-paper/ewp-specs-types-address/tree/stable-v1", IsNullable = false)]
    public partial class streetaddress : FlexibleAddress
    {
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://github.com/erasmus-without-paper/ewp-specs-types-address/tree/stable-v1")]
    [System.Xml.Serialization.XmlRootAttribute("mailing-address", Namespace = "https://github.com/erasmus-without-paper/ewp-specs-types-address/tree/stable-v1", IsNullable = false)]
    public partial class mailingaddress : FlexibleAddress
    {
    }
}
