using EwpApi.Dto.ewp_specs_api_institutions;
using EwpApi.Helper;
using EwpApi.Service.Exception;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EwpApi.Validators.APIRequests
{
    public class InstitutionsAPIRequestValidator: RequestValidator
    {
        public override async Task<bool> VerifyHttpSignatureRequest(HttpRequest request)
        {
            await base.VerifyHttpSignatureRequest(request);

            if (request.Method.ToUpper().Equals("GET")) {
                if (request.Query == null || request.Query.Count == 0)
                    throw new EwpSecWebApplicationException("hei_id is missing", System.Net.HttpStatusCode.BadRequest);

                if (!request.Query.ContainsKey("hei_id"))
                    throw new EwpSecWebApplicationException("hei_id is missing", System.Net.HttpStatusCode.BadRequest);

                if(request.Query["hei_id"].Count() > 1)
                    throw new EwpSecWebApplicationException("Number of hei_id is more than max_hei_id", System.Net.HttpStatusCode.BadRequest);
                                
            }
            else if (request.Method.ToUpper().Equals("POST"))
            {
                List<string> bodyString = await BodyReader.ReadParameterValues(request, "hei_id");

                if (bodyString.Count() == 0)
                    throw new EwpSecWebApplicationException("hei_id is not valid in body", System.Net.HttpStatusCode.BadRequest);

                if (bodyString.Count() > 1)
                    throw new EwpSecWebApplicationException("Number of hei_id is more than max_hei_id", System.Net.HttpStatusCode.BadRequest);
                             
            }

                return true;
        }
    }
}
