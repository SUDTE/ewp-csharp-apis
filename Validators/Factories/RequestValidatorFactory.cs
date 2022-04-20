using EwpApi.Validators.APIRequests;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EwpApi.Validators.Factories
{
    public class RequestValidatorFactory
    {
        public RequestValidator GetValidator(String controllerName)
        {
            switch (controllerName)
            {
                case "Institutions":
                    return new InstitutionsAPIRequestValidator();
                case "Organizations":
                    return new OrganizationsAPIRequestValidator();
                case "IIAs":
                    return new IIAsAPIRequestValidator();
                default:
                    return new RequestValidator();
                    
            }
        }
    }
}
