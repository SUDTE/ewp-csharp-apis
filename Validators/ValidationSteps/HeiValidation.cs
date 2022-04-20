using EwpApi.Helper;
using EwpApi.Service.Exception;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EwpApi.Validators.ValidationSteps
{
    public class HeiValidation : IValidationStep
    {
        public async Task<Boolean> Validate(HttpRequest request)
        {
            if (request.Method.ToUpper().Equals("GET"))
            {
                if (request.Query == null || request.Query.Count == 0)
                    throw new EwpSecWebApplicationException("hei_id is missing", System.Net.HttpStatusCode.BadRequest);

                if (!request.Query.ContainsKey("hei_id"))
                    throw new EwpSecWebApplicationException("hei_id is missing", System.Net.HttpStatusCode.BadRequest);

                if (request.Query["hei_id"].Count() > 1)
                    throw new EwpSecWebApplicationException("Number of hei_id is more than one", System.Net.HttpStatusCode.BadRequest);

                if (!request.Query["hei_id"][0].Equals("iyte.edu.tr"))
                    throw new EwpSecWebApplicationException("Unknown hei_id is recieved", System.Net.HttpStatusCode.BadRequest);               
            }
            else if (request.Method.ToUpper().Equals("POST"))
            {
                if (request.ContentLength == 0)
                    throw new EwpSecWebApplicationException("hei_id is missing", System.Net.HttpStatusCode.BadRequest);

                string bodyString = await BodyReader.ReadRequestBody(request);
                List<string> heiIds = await BodyReader.ReadParameterValues(bodyString, "hei_id");
              


                if (heiIds.Count() == 0)
                    throw new EwpSecWebApplicationException("hei_id is missing", System.Net.HttpStatusCode.BadRequest);

                if (heiIds.Count() > 1)
                    throw new EwpSecWebApplicationException("Number of hei_id is more than one", System.Net.HttpStatusCode.BadRequest);

                if (!heiIds[0].Equals("iyte.edu.tr"))
                    throw new EwpSecWebApplicationException("Unknown hei_id is recieved", System.Net.HttpStatusCode.BadRequest);
            }

            return true;
        }
    }
}
