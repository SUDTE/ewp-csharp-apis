using EwpApi.Dto.ewp_specs_architecture;
using EwpApi.Dto.ewp_specs_types_address;
using EwpApi.Dto.ewp_specs_types_contact;
using EwpApi.Helper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EwpApi.Dto.ewp_specs_api_ounits
{


	[XmlRoot(ElementName = "ounits-response", Namespace = "https://github.com/erasmus-without-paper/ewp-specs-api-ounits/tree/stable-v2")]
	public class ResponseOrganization: XmlBodyGenerator, IResponse
	{
		[XmlNamespaceDeclarations]
		public XmlSerializerNamespaces xmlns = new XmlSerializerNamespaces();

		public ResponseOrganization()
		{
			//xmlns.Add("xs", "http://www.w3.org/2001/XMLSchema");
			//xmlns.Add("xml", "http://www.w3.org/XML/1998/namespace");
			//xmlns.Add("a", "https://github.com/erasmus-without-paper/ewp-specs-types-address/tree/stable-v1");
			//xmlns.Add("c", "https://github.com/erasmus-without-paper/ewp-specs-types-contact/tree/stable-v1");
			//xmlns.Add("p", "https://github.com/erasmus-without-paper/ewp-specs-types-phonenumber/tree/stable-v1");
			//xmlns.Add("ewp", "https://github.com/erasmus-without-paper/ewp-specs-architecture/blob/stable-v1/common-types.xsd");
		}

		private List<Ounit> ounitField;

		/// <remarks/>
		[System.Xml.Serialization.XmlElementAttribute("ounit")]
		public List<Ounit> ounit
		{
			get
			{
				return this.ounitField;
			}
			set
			{
				this.ounitField = value;
			}
		}

		public string ToXml()
		{
			return GenerateXml<ResponseOrganization>(this);
		}

		public async Task WriteXmlBody(HttpResponse response)
		{
			await WriteXmlBodyIntoResponse<ResponseOrganization>(this, response);
		}
	}


	[XmlRoot(ElementName = "ounit", Namespace = "https://github.com/erasmus-without-paper/ewp-specs-api-ounits/tree/stable-v2")]
	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://github.com/erasmus-without-paper/ewp-specs-api-ounits/tree/stable-v2")]

	public class Ounit
	{		
		[System.Xml.Serialization.XmlElementAttribute("ounit-id")]
		public string Ounitid { get; set; }

		
		[System.Xml.Serialization.XmlElementAttribute("ounit-code")] 
		public string Ounitcode { get; set; }

		[System.Xml.Serialization.XmlElementAttribute("name")]
		public List<StringWithOptionalLang> Name { get; set; }

		[System.Xml.Serialization.XmlElementAttribute("abbreviation")]
		public string Abbreviation { get; set; }

		[System.Xml.Serialization.XmlElementAttribute("parent-ounit-id")]
		public string Parentounitid { get; set; }


		[System.Xml.Serialization.XmlElementAttribute("street-address", Namespace = "https://github.com/erasmus-without-paper/ewp-specs-types-address/tree/stable-v1")]
		public streetaddress Streetaddress { get; set; }


		[System.Xml.Serialization.XmlElementAttribute("mailing-address", Namespace = "https://github.com/erasmus-without-paper/ewp-specs-types-address/tree/stable-v1")]
		public mailingaddress Mailingaddress { get; set; }


		[System.Xml.Serialization.XmlElementAttribute("website-url")]		
		public List<HTTPWithOptionalLang> Websiteurl { get; set; }


		[System.Xml.Serialization.XmlElementAttribute("logo-url")]
		public string Logourl { get; set; }


		[System.Xml.Serialization.XmlElementAttribute("mobility-factsheet-url")]		
		public List<HTTPWithOptionalLang> Mobilityfactsheeturl { get; set; }


		[System.Xml.Serialization.XmlElementAttribute("contact", Namespace = "https://github.com/erasmus-without-paper/ewp-specs-types-contact/tree/stable-v1")]
		public List<Contact> Contact { get; set; }
	}

}
