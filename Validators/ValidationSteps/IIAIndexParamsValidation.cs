using EwpApi.Helper;
using EwpApi.Service.Exception;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace EwpApi.Validators.ValidationSteps
{
    public class IIAIndexParamsValidation : IValidationStep
    {

        public async Task<bool> Validate(HttpRequest request)
        {
            if (request.Method.ToUpper().Equals("GET"))
            {          
                if (request.Query.ContainsKey("partner_hei_id") && request.Query["partner_hei_id"][0].Equals("iyte.edu.tr"))
                    throw new EwpSecWebApplicationException("hei_id must be different from partner_hei_id", System.Net.HttpStatusCode.BadRequest);

                if (request.Query.ContainsKey("receiving_academic_year_id") && !IsAcademicYearInTrueFormat(request.Query["receiving_academic_year_id"].ToList<string>()))
                    throw new EwpSecWebApplicationException("academic year format does not match", System.Net.HttpStatusCode.BadRequest);

                if (request.Query.ContainsKey("modified_since") && request.Query["modified_since"].Count > 1)
                    throw new EwpSecWebApplicationException("modified_since parameter must not be more than one", System.Net.HttpStatusCode.BadRequest);

                if (request.Query.ContainsKey("modified_since") && !IsModifiedSinceInTrueformat(request.Query["modified_since"][0]))
                    throw new EwpSecWebApplicationException("modified_since format does not match", System.Net.HttpStatusCode.BadRequest);

            }
            else if (request.Method.ToUpper().Equals("POST"))
            {
                string bodyString = await BodyReader.ReadRequestBody(request);
               
                List<string> partnerHeiIds = await BodyReader.ReadParameterValues(bodyString, "partner_hei_id");
                List<string> academicYears = await BodyReader.ReadParameterValues(bodyString, "receiving_academic_year_id");
                List<string> modifiedSinceParam = await BodyReader.ReadParameterValues(bodyString, "modified_since");


                if (partnerHeiIds.Count > 0 && partnerHeiIds.Where(a => a.Equals("iyte.edu.tr")).Count() > 0)
                    throw new EwpSecWebApplicationException("hei_id must be different from partner_hei_id", System.Net.HttpStatusCode.BadRequest);

                if (academicYears.Count > 0 && !IsAcademicYearInTrueFormat(academicYears))
                    throw new EwpSecWebApplicationException("academic year format does not match", System.Net.HttpStatusCode.BadRequest);

                if (modifiedSinceParam.Count > 1)
                    throw new EwpSecWebApplicationException("modified_since parameter must not be more than one", System.Net.HttpStatusCode.BadRequest);

                if (modifiedSinceParam.Count > 0 && !IsModifiedSinceInTrueformat(modifiedSinceParam[0]))
                    throw new EwpSecWebApplicationException("modified_since format does not match", System.Net.HttpStatusCode.BadRequest);
            }

            return true;
        }

        private bool IsAcademicYearInTrueFormat(List<string> academicYears)
        {
            //@"^(19|20)\d{2}/(19|20)\d{2}$"
            Regex rgx = new Regex(@"^\d{4}/\d{4}$");
            foreach (string currentYear in academicYears)
            {                
                if (!rgx.IsMatch(currentYear))
                    return false;
            }
            return true;
        }

        private bool IsModifiedSinceInTrueformat(string modifiedSince)
        {
            Regex rgx = new Regex(@"^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])T((([01][0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9])|(24:00:00))(Z|([-\/\+]))((0[0-9]|1[0-3]):[0-5][0-9]|14:00)$");

            Boolean matches = rgx.IsMatch(modifiedSince);
            return matches;
        }
    }
}
