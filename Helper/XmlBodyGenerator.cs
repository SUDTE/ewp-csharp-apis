using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EwpApi.Helper
{
    public class XmlBodyGenerator: XmlGenerator
    {
        /// <summary>
        /// Writes the xml form of given entity T into Response
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="responseObj"></param>
        /// <param name="httpResponse"></param>
        /// <returns></returns>
        public async Task WriteXmlBodyIntoResponse<T>(T responseObj, HttpResponse httpResponse)
        {
           //httpResponse.Headers.Add("Content-Encoding", "UTF-8");
            httpResponse.ContentType = "application/xml;charset=utf-8";
            string body = GenerateXml<T>(responseObj);
            httpResponse.ContentLength = body.Length;
            await httpResponse.WriteAsync(body);
            await httpResponse.CompleteAsync();
        }
        
    }
}
