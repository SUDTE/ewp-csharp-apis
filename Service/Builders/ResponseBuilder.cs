using EwpApi.Constants;
using EwpApi.Dto;
using EwpApi.Helper;
using EwpApi.Validators;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace EwpApi.Service
{
    public abstract class ResponseBuilder: XmlGenerator
    {
        private string iytePublicKeyId = HttpSigSettings.GetInstance().GetKeyId("iyte.edu.tr");
        private string iytePrivateRsaKey = HttpSigSettings.GetInstance().GetKeyId("iyte.edu.tr");
        protected HttpResponse _apiResponse;
        public AuthRequest _authRequest { get; set; }


        public virtual IResponse Build(AuthRequest authRequest, HttpResponse apiResponse, IResponse response)
        {
            this._apiResponse = apiResponse;

            if(authRequest.HeiIds == null) { 
                RegistryService client = new RegistryService();
                authRequest.HeiIds = client.GetHeiIdsByRSAKey(authRequest.KeyId);
            }

            String body = response.ToXml();
            Log.Information("Response body is :\n" + body);

            AddHeaders(authRequest, body);

           
            return response;
        }

        public abstract IResponse Build(HttpRequest apiRequest, HttpResponse apiResponse, Dictionary<string, List<string>> parameters);
              

        protected void AddHeaders(AuthRequest authRequest, string body)
        {
            Boolean signResponse = false;

            string acceptSignature = authRequest.GetHeaderValue("accept-signature");
            if (!String.IsNullOrEmpty(acceptSignature) && acceptSignature.ToLower().Contains("rsa-sha256"))
                signResponse = true;

            String originalDate = DateTime.UtcNow.ToString("ddd, dd MMM yyyy HH:mm:ss 'GMT'", CultureInfo.InvariantCulture);
            _apiResponse.Headers.Add("Date", originalDate);
            _apiResponse.Headers.Add("Original-Date", originalDate);



            string digest = "SHA-256=" + RsaHelper.ComputeSha256Hash(body);
            _apiResponse.Headers.Add("Digest", digest);
            _apiResponse.Headers.Add("X-Request-Id", authRequest.XRequestId);
            _apiResponse.Headers.Add("X-Request-Signature", authRequest.Signature);
            _apiResponse.Headers.Add("Want-Digest", "SHA-256");

            if (_apiResponse.StatusCode == 401)
            {
                _apiResponse.Headers.Add("WWW-Authenticate", "Signature realm=\"EWP\"");

                if (signResponse) { 
                    string signingString = GenerateSignature(originalDate, digest, authRequest.XRequestId, authRequest.Signature, "Signature realm=\"EWP\"", "SHA-256");
                    string signatureHeaderContent = String.Format("keyId=\"{0}\", algorithm=\"rsa-sha256\", headers=\"original-date digest x-request-id x-request-signature www-authenticate want-digest\", signature=\"{1}\"", iytePublicKeyId, signingString);
                    _apiResponse.Headers.Add("Signature", signatureHeaderContent);
                }

            }
            else if (signResponse)
            {
                string signingString = GenerateSignature(originalDate, digest, authRequest.XRequestId, authRequest.Signature, null, "SHA-256");
                string signatureHeaderContent = String.Format("keyId=\"{0}\", algorithm=\"rsa-sha256\", headers=\"original-date digest x-request-id x-request-signature want-digest\", signature=\"{1}\"", iytePublicKeyId, signingString);
                _apiResponse.Headers.Add("Signature", signatureHeaderContent);
            }
        }

        public string GenerateSignatureHeader(HttpResponse response)
        {
            if (!response.Headers.ContainsKey("Signature"))
                return null; 

            bool is401Message = response.Headers.ContainsKey("WWW-Authenticate");
            string signingString = GenerateSignature(
                                    response.Headers["Original-Date"], 
                                    response.Headers["Digest"], 
                                    response.Headers["X-Request-Id"], 
                                    response.Headers["X-Request-Signature"],
                                    is401Message? "Signature realm=\"EWP\"":null, 
                                    "SHA-256");
            string signatureHeaderContent = String.Format("keyId=\"{0}\", algorithm=\"rsa-sha256\", headers=\"original-date digest x-request-id x-request-signature www-authenticate want-digest\", signature=\"{1}\"", iytePublicKeyId, signingString);
            return signatureHeaderContent;
        }
       

        private void LogHeadersGenerated()
        {
            var headerValues = "";
            foreach (var item in _apiResponse.Headers)
            {
                headerValues += "\n" + item.Key + ":" + item.Value;
            }

            Log.Information("Response Headers are: \n " + headerValues);
        }

        private string GenerateSignature(string date, string digest, string requestId, string requestSignature, string wwwAuthenticate, string wantDigest)
        {

            string SigningString = "original-date: " + date + "\n";
            SigningString += "digest: " + digest + "\n";
            SigningString += "x-request-id: " + requestId + "\n";
            SigningString += "x-request-signature: " + requestSignature;

            if (!String.IsNullOrEmpty(wwwAuthenticate))
            {
                SigningString += "\nwww-authenticate: " + wwwAuthenticate;
            }

            if (!String.IsNullOrEmpty(wantDigest))
            {
                SigningString += "\nwant-digest: " + wantDigest;
            }

            string textStr = SigningString.Replace("\n", "|\n");

            Log.Information("Text to be sign is : \n" + textStr);
            byte[] SigningStringInBytes = Encoding.Default.GetBytes(SigningString);
            string utf8SigningString = Encoding.UTF8.GetString(SigningStringInBytes);

            return (new RsaHelper()).Sign(utf8SigningString, iytePrivateRsaKey);
        }


    }
}
