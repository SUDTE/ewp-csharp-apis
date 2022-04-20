using EwpApi.Helper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EwpApi.Dto.ewp_specs_api_iias
{
    [XmlRoot(ElementName = "iias-index-response", Namespace = "https://github.com/erasmus-without-paper/ewp-specs-api-iias/blob/stable-v6/endpoints/index-response.xsd")]
    public class ResponseIIAsIndex : XmlBodyGenerator, IResponse
    {
        [XmlNamespaceDeclarations]
        public XmlSerializerNamespaces xmlns = new XmlSerializerNamespaces();
        public ResponseIIAsIndex()
        {
            xmlns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            
        }

        [XmlElement("iia-id")]
        public List<string> iiaIds { get; set; }

        public async Task WriteXmlBody(HttpResponse response)
        {
            await WriteXmlBodyIntoResponse<ResponseIIAsIndex>(this, response);
        }

        public string ToXml()
        {
            return GenerateXml<ResponseIIAsIndex>(this);
        }
    }
}
