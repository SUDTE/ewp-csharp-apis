using EwpApi.Dto;
using EwpApi.Dto.ewp_specs_architecture;
using EwpApi.Helper;
using EwpApi.Validators;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EwpApi.Service.Builders
{
    public class ErrorResponseBuilder: ResponseBuilder
    {        
        //public override IResponse Build(AuthRequest authRequest, HttpResponse apiResponse, IResponse response)
        //{            
        //    this._apiResponse = apiResponse;

        //    string body = response.ToXml(); 
        //    Log.Information("Error body is :\n" + body);

        //    AddHeaders(authRequest, body);

        //    return response;
        //}

        public override IResponse Build(HttpRequest apiRequest, HttpResponse apiResponse, Dictionary<string, List<string>> parameters)
        {
            this._apiResponse = apiResponse;
            HeaderParser parser = new HeaderParser();
            AuthRequest authRequest = parser.ParseHeader(apiRequest);

            string developerMessage = (parameters.GetValueOrDefault(Constants.ErrorParameters.DeveloperMessage.ToString()) != null) ? parameters.GetValueOrDefault(Constants.ErrorParameters.DeveloperMessage.ToString())[0] : "";
            string userMessage = (parameters.GetValueOrDefault(Constants.ErrorParameters.UserMessage.ToString()) != null) ? parameters.GetValueOrDefault(Constants.ErrorParameters.UserMessage.ToString())[0] : "";
            ResponseError error = new ResponseError(developerMessage, userMessage);
            string body = error.ToXml();
            Log.Information("Error body is :\n" + body);

            AddHeaders(authRequest, body);


            return error;
        }

    }
}
