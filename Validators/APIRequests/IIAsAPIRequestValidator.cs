using EwpApi.Helper;
using EwpApi.Service.Exception;
using EwpApi.Validators.ValidationSteps;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EwpApi.Validators.APIRequests
{
    public class IIAsAPIRequestValidator : RequestValidator
    {
        List<IValidationStep> steps;

        public override async Task<bool> VerifyHttpSignatureRequest(HttpRequest request)
        {
            await base.VerifyHttpSignatureRequest(request);

            if (request.RouteValues["action"].Equals("Index"))
            {
                steps = new List<IValidationStep>();
                steps.Add(new HeiValidation());
                steps.Add(new IIAIndexParamsValidation());
            }
            else
            {
                steps = new List<IValidationStep>();
                steps.Add(new HeiValidation());
                steps.Add(new IIAGetParamsValidation());
            }

           
            foreach (IValidationStep step in steps)
                await step.Validate(request);

            return true;
        }
               
    }
}
