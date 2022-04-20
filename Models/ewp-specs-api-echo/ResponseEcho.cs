using EwpApi.Helper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EwpApi.Dto
{


    [XmlRoot(ElementName = "response", Namespace = "https://github.com/erasmus-without-paper/ewp-specs-api-echo/tree/stable-v2")]
    public class ResponseEcho: XmlBodyGenerator, IResponse
    {
        public ResponseEcho()
        {

        }
        public ResponseEcho(List<string> echoList, List<string> heiList)
        {
            hei_id = heiList;
            if (echoList != null && echoList.Count > 0)
            {
                echo = echoList;
            }
            else
                echo = null;
        }

        //[XmlAttributeAttribute("schemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        //public string xsiSchemaLocation = "https://github.com/erasmus-without-paper/ewp-specs-api-echo/blob/stable-v2/response.xsd";

        [XmlElement("hei-id")]
        public List<string> hei_id  { get; set; }

        [XmlElement("echo")]
        public List<string> echo { get; set; }

        public async Task WriteXmlBody(HttpResponse response)
        {
            await WriteXmlBodyIntoResponse<ResponseEcho>(this, response);
        }

        public string ToXml()
        {
            return GenerateXml<ResponseEcho>(this);
        }
    }
}

