using EwpApi.Helper;
using EwpApi.Service.Exception;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EwpApi.Validators.ValidationSteps
{
    public class IIAGetParamsValidation : IValidationStep
    {
        public async Task<bool> Validate(HttpRequest request)
        {
            if (request.Method.ToUpper().Equals("GET"))
            {
                if (!request.Query.ContainsKey("iia_id") && !request.Query.ContainsKey("iia_code"))
                    throw new EwpSecWebApplicationException("Provide either a list of iia_id values or a list of iia_code values, but not both", System.Net.HttpStatusCode.BadRequest);

                if (request.Query.ContainsKey("iia_id") && request.Query.ContainsKey("iia_code"))
                    throw new EwpSecWebApplicationException("Provide either a list of iia_id values or a list of iia_code values, but not both", System.Net.HttpStatusCode.BadRequest);

                if (request.Query.ContainsKey("iia_id") && request.Query["iia_id"].Count > 1)
                    throw new EwpSecWebApplicationException("iia_id parameter must not be more than one", System.Net.HttpStatusCode.BadRequest);

                if (request.Query.ContainsKey("iia_code") && request.Query["iia_code"].Count > 1)
                    throw new EwpSecWebApplicationException("iia_code parameter must not be more than one", System.Net.HttpStatusCode.BadRequest);

                if (request.Query.ContainsKey("send_pdf") && request.Query["send_pdf"].Count > 1)
                    throw new EwpSecWebApplicationException("send_pdf parameter must not be more than one", System.Net.HttpStatusCode.BadRequest);

                if (request.Query.ContainsKey("send_pdf") && !(request.Query["send_pdf"][0].ToLower().Equals("false") || request.Query["send_pdf"][0].ToLower().Equals("true")))
                    throw new EwpSecWebApplicationException("send_pdf parameter must be true or false", System.Net.HttpStatusCode.BadRequest);
            }
            else if (request.Method.ToUpper().Equals("POST"))
            {
                string bodyString = await BodyReader.ReadRequestBody(request);

                List<string> iiaIds = await BodyReader.ReadParameterValues(bodyString, "iia_id");
                List<string> iiaCodes = await BodyReader.ReadParameterValues(bodyString, "iia_code");
                List<string> sendPdf = await BodyReader.ReadParameterValues(bodyString, "send_pdf");

                if (iiaIds.Count == 0 && iiaCodes.Count == 0)
                    throw new EwpSecWebApplicationException("Provide either a list of iia_id values or a list of iia_code values, but not both", System.Net.HttpStatusCode.BadRequest);

                if (iiaIds.Count > 0 && iiaCodes.Count > 0)
                    throw new EwpSecWebApplicationException("Provide either a list of iia_id values or a list of iia_code values, but not both", System.Net.HttpStatusCode.BadRequest);

                if (iiaIds.Count > 1)
                    throw new EwpSecWebApplicationException("iia_id parameter must not be more than one", System.Net.HttpStatusCode.BadRequest);

                if (iiaCodes.Count > 1)
                    throw new EwpSecWebApplicationException("iia_code parameter must not be more than one", System.Net.HttpStatusCode.BadRequest);

                if (sendPdf.Count > 1)
                    throw new EwpSecWebApplicationException("send_pdf parameter must not be more than one", System.Net.HttpStatusCode.BadRequest);

                if (sendPdf.Count == 1 && !(sendPdf[0].ToLower().Equals("false") || sendPdf[0].ToLower().Equals("true")))
                    throw new EwpSecWebApplicationException("send_pdf parameter must be true or false", System.Net.HttpStatusCode.BadRequest);
            }

            return true;
        }
    }
}
