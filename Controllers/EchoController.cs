using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using EwpApi.Service;
using System.Text.RegularExpressions;
using System.Globalization;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Xml.Serialization;
using EwpApi.Dto;
using System.Xml;
using Microsoft.Extensions.Logging;
using EwpApi.Validators;
using System.Text.Json;
using EwpApi.Service.Exception;
using System.Net.Http;
using Serilog;
using EwpApi.Helper;
using EwpApi.Service.Builders;

namespace EwpApi.Controllers
{

    /// <remarks>
    /// EWP Echo API might seem trivial in itself, but it requires EWP developers 
    /// to design and test the authentication and security framework which they 
    /// will use throughout the development of all the other EWP features. 
    /// It is RECOMMENDED for all developers to implement it (and keep it updated) 
    /// at least in their development EWP Hosts, to avoid potential problems in the future.
    /// </remarks>
    [Route("api/[controller]")]
    [ApiController]
    public class EchoController : ControllerBase
    {
        private readonly ILogger<EchoController> _logger;
        private readonly ResponseBuilderFactory _responseBuilderFactory;

        public EchoController(ILogger<EchoController> logger, ResponseBuilderFactory builderFactory)
        {
            _logger = logger;
            _responseBuilderFactory = builderFactory;
        }


        /// <summary>
        /// Echo API Get Method
        /// </summary>
        /// <remarks>
        /// The method returns echo parameters given in the query string and hei ids found by using authorization header 
        /// <br />
        /// <para>
        /// Sample url: <br />  
        ///     https://host:port/api/Echo?echo=aa&amp;echo=bb
        ///   
        /// </para>
        /// Sample Header:
        /// 
        ///     Accept-Signature: rsa-sha256
        ///     Authorization: Signature keyId = "3eaa4e3a82de7abfa3bc7a0625f09dfc099f1bfa459f2af101accd692be2b527", signature = "AXe3YXQmnAvYdtO51sw9giMP7SwOUOun58+CIC4LngveVy/N2CP8MT+8BCRh5sGJLNKdZ1Uu+5PEWP8Tc4AY5IqIi3qgdcLrqd5HUaeyfvZM6qMM1mKRg7PPLCO5zgS8ATsdpW+CTI6NKJk0CDSGtkGk8MSwtKItC7nrJllTlY8Fn3oMBKfRdZqshQN0XEXvi1V4Sgrt4BmmhvNwKd69/r7sNLEETUBqvN1YFO3SJE0ZDX6YLo9wVQnpVXBEc85PCfkwNhoqNfZcJdqfqKOTreoRedyiQbvYR7K4WcIXnrTYMtFoaYyHeAnloAisMLBY4hcCOFgqdI4/r6vLg3YuEw==", headers = "(request-target) accept-signature date digest host want-digest x-request-id", algorithm = "rsa-sha256"
        ///     Date: Fri, 15 Apr 2022 08:54:36 GMT
        ///     Digest: SHA-256=47DEQpj8HBSa+/TImW+5JCeuQeRkm5NMpJWZG3hSuFU=
        ///     Host: test-app.iyte.edu.tr
        ///     Want-Digest: SHA-256
        ///     X-Request-Id: cce222de-6b13-4755-aa29-50e23c4bda6d       
        /// <br />    
        /// Sample Body: <br />
        ///     Body content is not important, it can be anything. It is used only to calculate digest
        ///     
        /// </remarks>     
        /// <param name="echo">echo should be given in query string and may be given more than once</param>        
        /// <response code="200">Ok</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [HttpGet]
        [Produces("application/xml")]
        public IActionResult Get([FromQuery] List<string> echo)
        {
            _logger.LogInformation("Echo is called");
            Dictionary<string, List<String>> queryParameters = new Dictionary<string, List<string>>();
            queryParameters.Add("echo", echo);

            ResponseBuilder builder = _responseBuilderFactory.Create(ResponseBuilderFactory.ResponseBuilderType.Echo);
            IResponse body = builder.Build(Request, Response, queryParameters);

            body.WriteXmlBody(Response);
            return Ok();

        }


        /// <summary>
        /// ECHO API Post Method
        /// </summary>
        /// <remarks>
        /// The method returns echo parameters given in the body and hei ids found by using authorization header 
        /// <br />
        /// <para>
        /// Sample url: <br />  
        ///     https://host:port/api/Echo
        ///   
        /// </para>
        /// Sample Header:
        /// 
        ///     Accept-Signature: rsa-sha256
        ///     Authorization: Signature keyId = "3eaa4e3a82de7abfa3bc7a0625f09dfc099f1bfa459f2af101accd692be2b527", signature = "AXe3YXQmnAvYdtO51sw9giMP7SwOUOun58+CIC4LngveVy/N2CP8MT+8BCRh5sGJLNKdZ1Uu+5PEWP8Tc4AY5IqIi3qgdcLrqd5HUaeyfvZM6qMM1mKRg7PPLCO5zgS8ATsdpW+CTI6NKJk0CDSGtkGk8MSwtKItC7nrJllTlY8Fn3oMBKfRdZqshQN0XEXvi1V4Sgrt4BmmhvNwKd69/r7sNLEETUBqvN1YFO3SJE0ZDX6YLo9wVQnpVXBEc85PCfkwNhoqNfZcJdqfqKOTreoRedyiQbvYR7K4WcIXnrTYMtFoaYyHeAnloAisMLBY4hcCOFgqdI4/r6vLg3YuEw==", headers = "(request-target) accept-signature date digest host want-digest x-request-id", algorithm = "rsa-sha256"
        ///     Date: Fri, 15 Apr 2022 08:54:36 GMT
        ///     Digest: SHA-256=47DEQpj8HBSa+/TImW+5JCeuQeRkm5NMpJWZG3hSuFU=
        ///     Host: test-app.iyte.edu.tr
        ///     Want-Digest: SHA-256
        ///     X-Request-Id: cce222de-6b13-4755-aa29-50e23c4bda6d       
        /// <br />    
        /// Sample Body: <br />
        /// <code>
        ///     echo=aa&amp;echo=bb 
        /// </code>
        ///     
        /// </remarks>  
        /// <response code="200">Ok</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [HttpPost]
        [Produces("application/xml")]
        public async Task<IActionResult> Post()
        {
            String requestBody = await BodyReader.ReadRequestBody(Request);
            List<string> echoList = await BodyReader.ReadParameterValues(requestBody, "echo");


            Dictionary<string, List<String>> queryParameters = new Dictionary<string, List<string>>();
            queryParameters.Add("echo", echoList);

            ResponseBuilder builder = _responseBuilderFactory.Create(ResponseBuilderFactory.ResponseBuilderType.Echo);
            IResponse body = builder.Build(Request, Response, queryParameters);

            await body.WriteXmlBody(Response);
            return Ok();
        }
    }
}
