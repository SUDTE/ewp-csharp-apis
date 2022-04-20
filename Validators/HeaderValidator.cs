using EwpApi.Dto;
using EwpApi.Helper;
using EwpApi.Service;
using EwpApi.Service.Exception;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EwpApi.Validators
{
    public class HeaderValidator
    {
        

        #region date validation methods
        public Boolean isDateWithinTimeThreshold(String dateString)
        {
            try
            {
                DateTime requestDate = GetDatetimeOfHeader(dateString);
                return ((DateTime.UtcNow - requestDate).Minutes < 5);
            }
            catch (Exception e)
            {
                Log.Error("Error occured when parsing date: " + dateString, e);
            }
            return false;
        }

        public Boolean isOriginalDateWithinTimeThreshold(String dateString, String originalDateString)
        {
            try
            {
                DateTime requestDate = GetDatetimeOfHeader(dateString);
                DateTime requestOriginalDate = GetDatetimeOfHeader(originalDateString);
                return ((requestDate - requestOriginalDate).Minutes < 20);
            }
            catch (Exception e)
            {
                Log.Error("Error occured when parsing date: " + dateString, e);
            }
            return false;
        }

        private DateTime GetDatetimeOfHeader(String headerDateString)
        {
            string acceptableDate = headerDateString.Substring(0, headerDateString.IndexOf(",") + 1);
            headerDateString = headerDateString.Substring(headerDateString.IndexOf(",") + 1);
            if (headerDateString.StartsWith(" "))
                headerDateString = headerDateString.Substring(1);

            if (headerDateString.IndexOf(" ") == 1)
                headerDateString = "0" + headerDateString;
            acceptableDate = acceptableDate + " " + headerDateString;

            DateTime requestDate = (DateTime.ParseExact(acceptableDate, "ddd, dd MMM yyyy HH:mm:ss 'GMT'", CultureInfo.InvariantCulture));
            return requestDate;
        }

        #endregion

        public Boolean CheckRequiredSignedHeaders(AuthRequest authRequest, string[] authHeaders)
        {

            if (String.IsNullOrEmpty(authRequest.Headers))
                throw new EwpSecWebApplicationException("Missing headers filed in Authorization header", HttpStatusCode.BadRequest);

            string[] headers = authRequest.Headers.Split(" ".ToCharArray());
            foreach (string current in authHeaders)
            {
                string[] str = null;
                if (current.Contains("|"))
                {
                    str = Array.FindAll(headers, s => (s.StartsWith(current.Substring(0, current.IndexOf("|"))) || s.StartsWith(current.Substring(current.IndexOf("|") + 1))));
                }
                else
                {
                    str = Array.FindAll(headers, s => s.StartsWith(current));
                }

                if (str == null || str.Count() == 0)
                {
                    throw new EwpSecWebApplicationException("Missing required signed header '" + current + "'", HttpStatusCode.BadRequest);
                }
            }

            return true;
        }

        #region digest validation methods
        public bool VerifyDigest(string body, string requestDigest)
        {
            string acceptedDigest = GetSHA256Digest(requestDigest);

            if (String.IsNullOrEmpty(acceptedDigest))
                return false;

            String digestCalculated = "SHA-256=" + RsaHelper.ComputeSha256Hash(body);
            return acceptedDigest.Equals(digestCalculated);
        }

        private String GetSHA256Digest(string digestHeader)
        {
            string[] fields = digestHeader.Replace(" ", "").Split(",");
            foreach (string innerField in fields)
            {
                if (innerField.ToUpper().StartsWith("SHA-256"))
                {
                    string innerFieldName = innerField.Substring(0, innerField.IndexOf("="));
                    string acceptedInnerField = innerField.Replace(innerFieldName, innerFieldName.ToUpper());
                    return acceptedInnerField;
                }
            }
            return null;
        }

        #endregion

        #region signature validation methods
        public bool VerifySignature(AuthRequest authRequest, String publicKeyString)
        {
            return RsaHelper.VerifySign(GetStringForSign(authRequest), publicKeyString, authRequest.Signature);
        }

        public string GetStringForSign(AuthRequest authRequest)
        {
            string[] headers = authRequest.Headers.Split(" ".ToCharArray());
            string rawString = "";
            foreach (string current in headers)
            {
                if (rawString.Length > 0)
                    rawString += "\n";
                if (current.Contains("date"))
                {
                    rawString += current + ": " + authRequest.Date;
                }
                else
                {
                    rawString += current + ": " + authRequest.GetHeaderValue(current);
                }
            }
            Log.Information("Signing String = '" + rawString + "'");
            return rawString;
        }

        #endregion
    }
}
