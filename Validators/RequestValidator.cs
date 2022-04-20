using EwpApi.Dto;
using EwpApi.Helper;
using EwpApi.Service;
using EwpApi.Service.Exception;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Primitives;
using System.Threading.Tasks;
using Serilog;

namespace EwpApi.Validators
{
    public class RequestValidator: HeaderValidator
    {

        public RequestValidator()
        {

        }
        public virtual async Task<bool> VerifyHttpSignatureRequest(HttpRequest request)
        {
            HeaderParser parser = new HeaderParser();
            AuthRequest authRequest = parser.ParseHeader(request);
            String body = await BodyReader.ReadRequestBody(request);

            IHeaderDictionary reqHeaders = request.Headers;
            StringValues authorization;
            if (!reqHeaders.TryGetValue("Authorization", out authorization))
                throw new EwpSecWebApplicationException("Authorization header is missing", HttpStatusCode.Unauthorized);
            Log.Information("Authorization header is found");

            if (!authorization.ToString().ToLower().StartsWith("signature"))
                throw new EwpSecWebApplicationException("Signature in Authorization header is missing", HttpStatusCode.Unauthorized);
            Log.Information("signature header is found");

            if (string.IsNullOrEmpty(authRequest.Algorithm))
                throw new EwpSecWebApplicationException("Algorithm field is missing", HttpStatusCode.BadRequest);
            if (!authRequest.Algorithm.ToLower().Contains("rsa-sha256"))
                throw new EwpSecWebApplicationException("Only signature algorithm rsa-sha-256 is supported", HttpStatusCode.Unauthorized);
            Log.Information("Algorithm header test is successful");

            string[] authHeaders = { "(request-target)", "host", "date|original-date", "digest", "x-request-id" };
            if (!CheckRequiredSignedHeaders(authRequest, authHeaders))
                throw new EwpSecWebApplicationException("Missing required signed headers", HttpStatusCode.BadRequest);
            Log.Information("Signed headers test is successful");

            if (string.IsNullOrEmpty(authRequest.Host))
                throw new EwpSecWebApplicationException("Host header is missing", HttpStatusCode.BadRequest);
            if (!authRequest.Host.Equals(request.Host.Host))
                throw new EwpSecWebApplicationException("Host does not match", HttpStatusCode.BadRequest);
            Log.Information("Host header test is successful");

            if (string.IsNullOrEmpty(authRequest.XRequestId))
                throw new EwpSecWebApplicationException("X-Request-Id header is missing", HttpStatusCode.BadRequest);

            Regex rgx = new Regex("[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}");
            if (!rgx.IsMatch(authRequest.XRequestId))
                throw new EwpSecWebApplicationException("Authentication with non-canonical X-Request-ID", HttpStatusCode.BadRequest);
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

            RegistryService _service = new RegistryService();

            String publicKey = _service.GetCertificateByRSAKey(authRequest.KeyId);
            if (String.IsNullOrEmpty(publicKey))
                throw new EwpSecWebApplicationException("Key not found for fingerprint: " + authRequest.KeyId, HttpStatusCode.Forbidden);
            Log.Information("Public cert of KeyId is found");


            if (_service.CheckIsServerKey(authRequest.KeyId))
                throw new EwpSecWebApplicationException("Request must be sign with client key not server key: " + authRequest.KeyId, HttpStatusCode.Forbidden);
            Log.Information("Request signed with client key");


            if (!VerifySignature(authRequest, publicKey))
                throw new EwpSecWebApplicationException("Signature does not match", HttpStatusCode.Unauthorized);
            Log.Information("Signature is verified successfully");

            return true;
        }

        

    }
}
