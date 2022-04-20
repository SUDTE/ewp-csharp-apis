using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EwpApi.Validators.ValidationSteps
{
    public interface IValidationStep
    {
        public Task<Boolean> Validate(HttpRequest request);
    }
}
