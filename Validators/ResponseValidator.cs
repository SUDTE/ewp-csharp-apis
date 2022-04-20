using EwpApi.Dto;
using EwpApi.Helper;
using EwpApi.Service;
using EwpApi.Service.Exception;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EwpApi.Validators
{
    public class ResponseValidator : HeaderValidator
    {
        public async Task<bool> Verify(WebResponse response, string requestSignature, string requestId)
        {
            AuthRequest authRequest = ParseHeader(response);
            String body = await BodyReader.ReadResponseBody(response);

            if (String.IsNullOrEmpty(response.Headers.Get("Signature")))
                throw new EwpSecWebApplicationException("Signature header is missing", HttpStatusCode.Unauthorized);
            Log.Information("Signature header is found");

            if (string.IsNullOrEmpty(authRequest.Algorithm))
                throw new EwpSecWebApplicationException("Algorithm field is missing", HttpStatusCode.BadRequest);
            if (!authRequest.Algorithm.ToLower().Contains("rsa-sha256"))
                throw new EwpSecWebApplicationException("Only signature algorithm rsa-sha-256 is supported", HttpStatusCode.Unauthorized);
            Log.Information("Algorithm header test is successful");

            string[] authHeaders = { "x-request-signature", "date|original-date", "digest", "x-request-id" };
            if (!CheckRequiredSignedHeaders(authRequest, authHeaders))
                throw new EwpSecWebApplicationException("Missing required signed headers", HttpStatusCode.BadRequest);
            Log.Information("Signed headers test is successful");

            // bu kısma bakılmalı!!!
            //if (string.IsNullOrEmpty(authRequest.Host))
            //    throw new EwpSecWebApplicationException("Host header is missing", HttpStatusCode.BadRequest);
            //if (!authRequest.Host.Equals(response.Host.Host))
            //    throw new EwpSecWebApplicationException("Host does not match", HttpStatusCode.BadRequest);
            //Log.Information("Host header test is successful");

            if (string.IsNullOrEmpty(authRequest.XRequestId))
                throw new EwpSecWebApplicationException("X-Request-Id header is missing", HttpStatusCode.BadRequest);

            Regex rgx = new Regex("[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}");
            if (!rgx.IsMatch(authRequest.XRequestId))
                throw new EwpSecWebApplicationException("Authentication with non-canonical X-Request-ID", HttpStatusCode.BadRequest);
            if (!authRequest.XRequestId.Equals(requestId))
                throw new EwpSecWebApplicationException("X-Request-ID does not match", HttpStatusCode.BadRequest);
            Log.Information("XRequestId header test is successful");

            if (String.IsNullOrEmpty(authRequest.Date))
                throw new EwpSecWebApplicationException("The date cannot be parsed or the date does not match your server clock within a certain treshold of datetime", HttpStatusCode.BadRequest);
            if (!isDateWithinTimeThreshold(authRequest.Date))
                throw new EwpSecWebApplicationException("The date cannot be parsed or the date does not match your server clock within a certain treshold of datetime", HttpStatusCode.BadRequest);
            Log.Information("Date header test is successful");

            if (!String.IsNullOrEmpty(authRequest.GetHeaderValue("date"))
                && !String.IsNullOrEmpty(authRequest.GetHeaderValue("original-date"))
                && !isOriginalDateWithinTimeThreshold(authRequest.GetHeaderValue("date"), authRequest.GetHeaderValue("original-date")))
                throw new EwpSecWebApplicationException("The date is unsynchronized with original date", HttpStatusCode.BadRequest);

            if (String.IsNullOrEmpty(authRequest.Signature))
                throw new EwpSecWebApplicationException("Signature in Authorization header is missing", HttpStatusCode.BadRequest);
            Log.Information("Signature is found");

            if (String.IsNullOrEmpty(authRequest.KeyId))
                throw new EwpSecWebApplicationException("keyId in Authorization header is missing", HttpStatusCode.BadRequest);

            if (!VerifyDigest(body, authRequest.Digest))
                throw new EwpSecWebApplicationException("Digest does not match", HttpStatusCode.BadRequest);
            Log.Information("Digest matches");


            if (String.IsNullOrEmpty(authRequest.GetHeaderValue("x-request-signature")))
                throw new EwpSecWebApplicationException("X-Request-Signature does not match", HttpStatusCode.BadRequest);
            Log.Information("X-Request-Signature matches");

            RegistryService _service = new RegistryService();

            String publicKey = _service.GetCertificateByRSAKey(authRequest.KeyId);
            if (String.IsNullOrEmpty(publicKey))
                throw new EwpSecWebApplicationException("Key not found for fingerprint: " + authRequest.KeyId, HttpStatusCode.Forbidden);
            Log.Information("Public cert of KeyId is found");


            if (!_service.CheckIsServerKey(authRequest.KeyId))
                throw new EwpSecWebApplicationException("Response must be sign with server key not client key: " + authRequest.KeyId, HttpStatusCode.Forbidden);
            Log.Information("Response signed with server key");


            if (!VerifySignature(authRequest, publicKey))
                throw new EwpSecWebApplicationException("Signature does not match", HttpStatusCode.Unauthorized);
            Log.Information("Signature is verified successfully");

            if (((HttpWebResponse)response).StatusCode == HttpStatusCode.Unauthorized)
            {
                if (String.IsNullOrEmpty(authRequest.GetHeaderValue("want-digest")))
                    throw new EwpSecWebApplicationException("Want-Digest header is missing", HttpStatusCode.BadRequest);
                if (!authRequest.GetHeaderValue("want-digest").ToUpper().Equals("SHA-256"))
                    throw new EwpSecWebApplicationException("Want-Digest should be SHA-256", HttpStatusCode.BadRequest);
                Log.Information("Want-Diges matches");

                if (String.IsNullOrEmpty(authRequest.GetHeaderValue("www-authenticate")))
                    throw new EwpSecWebApplicationException("WWW-Authenticatet header is missing", HttpStatusCode.BadRequest);
                if (!authRequest.GetHeaderValue("www-authenticate").ToUpper().Equals("Signature realm=\"EWP\""))
                    throw new EwpSecWebApplicationException("WWW-Authenticate should be like that : Signature realm=\"EWP\"", HttpStatusCode.BadRequest);
                Log.Information("WWW-Authenticate matches");
            }

            return true;
        }

        public AuthRequest ParseHeader(WebResponse response)
        {
            var headerValues = "";
            Dictionary<string, string> allHeaders = new Dictionary<string, string>();
            foreach (var key in response.Headers.AllKeys)
            {
                headerValues += "\n" + key + ":" + response.Headers[key];
                allHeaders.Add(key, response.Headers[key]);
            }
            //Dictionary<string, string> ss = response.Headers.ToDictionary(a => a.Key, a => string.Join(";", a.Value));

            Log.Information("Recieved Response Header= \n" + headerValues);
            AuthRequest authRequest = new AuthRequest();
            authRequest.LoadHeadersWithValues(allHeaders);

            return authRequest;
        }
    }
}
