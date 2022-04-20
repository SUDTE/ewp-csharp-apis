using EwpApi.Dto;
using EwpApi.Helper;
using EwpApi.Service.Builders.Adaptees;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EwpApi.Service.Builders
{
    public class ResponseBuilderAdapter : ResponseBuilder
    {
        private IReadAdaptee _adaptee;
        public ResponseBuilderAdapter(IReadAdaptee adaptee)
        {
            _adaptee = adaptee;
        }
        public override IResponse Build(HttpRequest apiRequest, HttpResponse apiResponse, Dictionary<string, List<string>> parameters)
        {            
            HeaderParser parser = new HeaderParser();
            AuthRequest authRequest = parser.ParseHeader(apiRequest);
           
            IResponse institutionResponse = _adaptee.GenerateSampleData(parameters);

            return Build(authRequest, apiResponse, institutionResponse);
        }
    }
}
