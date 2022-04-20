using EwpApi.Dto;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EwpApi.Service.Builders
{
    public class EchoResponseBuilder: ResponseBuilder
    {
        private ResponseEcho echoResponse;
        //public override IResponse Build(AuthRequest authRequest, HttpResponse apiResponse, IResponse response)
        //{
        //    echoResponse = (ResponseEcho)response;
        //    if(echoResponse.hei_id == null || (echoResponse.hei_id!=null && echoResponse.hei_id.Count == 0))
        //    {
        //        RegistryService client = new RegistryService();
        //        echoResponse.hei_id = client.GetHeiIdsByRSAKey(authRequest.KeyId);
        //    }

        //    response = base.Build(authRequest, apiResponse, response);
            

        //    if (echoResponse.echo != null && echoResponse.echo.Count == 0)
        //        echoResponse.echo = null;

        //    return echoResponse;
        //}

        public override IResponse Build(HttpRequest apiRequest, HttpResponse apiResponse, Dictionary<string, List<string>> parameters)
        {
            Helper.HeaderParser parser = new Helper.HeaderParser();
            AuthRequest authRequest = parser.ParseHeader(apiRequest);

            RegistryService client = new RegistryService();
            authRequest.HeiIds = client.GetHeiIdsByRSAKey(authRequest.KeyId);

            ResponseEcho echoResponse = new ResponseEcho();
            echoResponse.echo = parameters.GetValueOrDefault("echo");            
            echoResponse.hei_id = authRequest.HeiIds;

            IResponse ir = (IResponse)echoResponse;
            ir = Build(authRequest, apiResponse, ir);

            return ir;
        }

    }
}
