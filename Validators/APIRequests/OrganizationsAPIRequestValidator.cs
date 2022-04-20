using EwpApi.Helper;
using EwpApi.Service.Exception;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EwpApi.Validators.APIRequests
{
    public class OrganizationsAPIRequestValidator: RequestValidator
    {
        public override async Task<bool> VerifyHttpSignatureRequest(HttpRequest request)
        {
            await base.VerifyHttpSignatureRequest(request);

            if (request.Method.ToUpper().Equals("GET"))
            {
                if (request.Query == null || request.Query.Count == 0)
                    throw new EwpSecWebApplicationException("hei_id is missing", System.Net.HttpStatusCode.BadRequest);

                if (!request.Query.ContainsKey("hei_id"))
                    throw new EwpSecWebApplicationException("hei_id is missing", System.Net.HttpStatusCode.BadRequest);

                if (request.Query["hei_id"].Count() > 1)
                    throw new EwpSecWebApplicationException("Number of hei_id is more than max_hei_id", System.Net.HttpStatusCode.BadRequest);

                if (!request.Query["hei_id"][0].Equals("iyte.edu.tr"))
                    throw new EwpSecWebApplicationException("Unknown hei_id is recieved", System.Net.HttpStatusCode.BadRequest);

                if (!request.Query.ContainsKey("ounit_id") && !request.Query.ContainsKey("ounit_code"))
                    throw new EwpSecWebApplicationException("Both ounit_id and ounit_code are missing. One of them should be given", System.Net.HttpStatusCode.BadRequest);

                if (request.Query.ContainsKey("ounit_id") && request.Query.ContainsKey("ounit_code"))
                    throw new EwpSecWebApplicationException("Provide either a list of ounit_id values, or a list of ounit_code values, but not both", System.Net.HttpStatusCode.BadRequest);

                if (request.Query["ounit_id"].Count() > 1)
                    throw new EwpSecWebApplicationException("Number of ounit_id is more than max_ounit_id", System.Net.HttpStatusCode.BadRequest);

                if (request.Query["ounit_code"].Count() > 1)
                    throw new EwpSecWebApplicationException("Number of ounit_code is more than max_ounit_code", System.Net.HttpStatusCode.BadRequest);

            }
            else if (request.Method.ToUpper().Equals("POST"))
            {
                if(request.ContentLength == 0)
                    throw new EwpSecWebApplicationException("hei_id is missing", System.Net.HttpStatusCode.BadRequest);

                string bodyString = await BodyReader.ReadRequestBody(request);
                List<string> heiIds = await BodyReader.ReadParameterValues(bodyString, "hei_id");
                List<string> ounitIds = await BodyReader.ReadParameterValues(bodyString, "ounit_id");
                List<string> ounitCodes = await BodyReader.ReadParameterValues(bodyString, "ounit_code");

                if (heiIds.Count() == 0)
                    throw new EwpSecWebApplicationException("hei_id is missing", System.Net.HttpStatusCode.BadRequest);

                if (ounitIds.Count() == 0 && ounitCodes.Count() == 0)
                    throw new EwpSecWebApplicationException("Provide either a list of ounit_id values, or a list of ounit_code values, but not both", System.Net.HttpStatusCode.BadRequest);

                if (heiIds.Count() > 1)
                    throw new EwpSecWebApplicationException("Number of hei_id is more than max_hei_id", System.Net.HttpStatusCode.BadRequest);

                if (!heiIds[0].Equals("iyte.edu.tr"))
                    throw new EwpSecWebApplicationException("Unknown hei_id is recieved", System.Net.HttpStatusCode.BadRequest);

                if (ounitIds.Count() > 1)
                    throw new EwpSecWebApplicationException("Number of ounit_id is more than max_ounit_id", System.Net.HttpStatusCode.BadRequest);

                if (ounitCodes.Count() > 1)
                    throw new EwpSecWebApplicationException("Number of ounit_code is more than max_ounit_code", System.Net.HttpStatusCode.BadRequest);

                if (ounitCodes.Count() > 0 && ounitIds.Count() > 0)
                    throw new EwpSecWebApplicationException("Provide either a list of ounit_id values, or a list of ounit_code values, but not both", System.Net.HttpStatusCode.BadRequest);
            }

            return true;
        }
    }
}
