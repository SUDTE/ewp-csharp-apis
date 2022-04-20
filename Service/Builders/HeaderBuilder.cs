using EwpApi.Constants;
using EwpApi.Helper;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EwpApi.Service
{
    public class HeaderBuilder
    {
        public string GenerateSignature(string headersField, Dictionary<string, string> headers)
        {
            string[] headerNames = headersField.Split(" ");
            string SigningString = "";
            foreach (string headerName in headerNames)
            {
                SigningString += headerName.ToLower(System.Globalization.CultureInfo.InvariantCulture) + ": " + headers[headerName] + "\n";
            }

            SigningString = SigningString.Substring(0, SigningString.Length - 1);
            
            string textStr = SigningString.Replace("\n", "|\n");
            Log.Information("Text to be sign is : \n" + textStr);

            byte[] SigningStringInBytes = Encoding.Default.GetBytes(SigningString);
            string utf8SigningString = Encoding.UTF8.GetString(SigningStringInBytes);

            return (new RsaHelper()).Sign(utf8SigningString, HttpSigSettings.GetInstance().GetPrivateKey("iyte.edu.tr"));
        }
    }
}
