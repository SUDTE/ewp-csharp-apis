using EwpApi.Constants;
using EwpApi.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace EwpApi.Service
{
    public class RequestBuilder : HeaderBuilder
    {
        private string iytePublicKeyId = HttpSigSettings.GetInstance().GetKeyId("iyte.edu.tr");
        private string iytePrivateRsaKey = HttpSigSettings.GetInstance().GetPrivateKey("iyte.edu.tr");
        public string X_Request_Id { get; set; }
        public string X_Request_Signature { get; set; }

        public WebRequest Build(HttpMethod method, string url, string body)
        {
            Uri uri = new Uri(url);
            string requestTarget = method.ToString().ToLower() + " " + uri.PathAndQuery;

            WebRequest request = WebRequest.Create(url);
            request.Method = method.Method;
            request.ContentType = "application/xml";
            request.ContentLength = body.Length;


            string host = url.Replace(uri.PathAndQuery, "");
            host = (host.StartsWith("http")) ? host.Substring(host.IndexOf("//") + 2) : host;
            request.Headers.Add("Host", host);

            String originalDate = DateTime.UtcNow.ToString("ddd, dd MMM yyyy HH:mm:ss 'GMT'", CultureInfo.InvariantCulture);
            request.Headers.Add("Original-Date", originalDate);
            request.Headers.Add("Date", DateTime.UtcNow.ToString("ddd, dd MMM yyyy HH:mm:ss 'GMT'", CultureInfo.InvariantCulture));

            X_Request_Id = Guid.NewGuid().ToString();
            request.Headers.Add("X-Request-Id", X_Request_Id);

            string digest = "SHA-256=" + RsaHelper.ComputeSha256Hash(body);
            request.Headers.Add("Digest", digest);
            request.Headers.Add("Want-Digest", "SHA-256");
            request.Headers.Add("Accept-Signature", "rsa-sha256");

            Dictionary<string, string> headerDictionary = new Dictionary<string, string>();
            headerDictionary.Add("(request-target)", requestTarget);
            headerDictionary.Add("original-date", originalDate);
            headerDictionary.Add("x-request-id", X_Request_Id);
            headerDictionary.Add("digest", digest);
            headerDictionary.Add("host", host);


            string headersField = "(request-target) host original-date digest x-request-id";
            X_Request_Signature = GenerateSignature(headersField, headerDictionary);
            string signatureHeaderContent = String.Format("Signature keyId=\"{0}\", signature=\"{1}\", algorithm=\"rsa-sha256\", headers=\"{2}\"", iytePublicKeyId, X_Request_Signature, headersField);
            request.Headers.Add("Authorization", signatureHeaderContent);


            return request;
        }
    }
}
