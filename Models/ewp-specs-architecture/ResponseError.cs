using EwpApi.Helper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EwpApi.Dto.ewp_specs_architecture
{
    [XmlRoot(ElementName = "error-response", Namespace = "https://github.com/erasmus-without-paper/ewp-specs-architecture/blob/stable-v1/common-types.xsd")]
    public class ResponseError : XmlBodyGenerator, IResponse
    {
        public ResponseError() { }
        public ResponseError(string developerMessage, string userMessage)
        {
            this.developerMessage = developerMessage;
            this.userMessage = userMessage;
        }

        [XmlElement("developer-message")]
        public string developerMessage { get; set; }

        [XmlElement("user-message")]
        public string userMessage { get; set; }

        public async Task WriteXmlBody(HttpResponse response)
        {
            await WriteXmlBodyIntoResponse<ResponseError>(this, response);
        }

        public string ToXml()
        {
            return GenerateXml<ResponseError>(this);
        }
    }
}
