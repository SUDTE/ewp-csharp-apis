using EwpApi.Dto;
using EwpApi.Helper;
using EwpApi.Service;
using EwpApi.Service.Builders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EwpApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstitutionsController : ControllerBase
    {
        private readonly ILogger<InstitutionsController> _logger;
        private readonly ResponseBuilderFactory _responseBuilderFactory;
                
        public InstitutionsController(ILogger<InstitutionsController> logger, ResponseBuilderFactory builderFactory)
        {
            _logger = logger;
            _responseBuilderFactory = builderFactory;
        }

        /// <summary>
        /// Institutions API Get Method
        /// </summary>
        /// <param name="hei_id">Hei Id is id of one of the universities ofwhich data stores in the server</param>
        /// <returns>XML format of InstitutionResponse</returns>
        /// <remarks>
        /// The method returns institution information 
        /// <br />
        /// <para>
        /// Sample url: <br />  
        ///     https://host:port/api/Institutions?hei_id=iyte.edu.tr
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
        /// <response code="200">Ok</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [HttpGet]
        [Produces("application/xml")]
        public async Task<IActionResult> Get([FromQuery] List<string> hei_id)
        {
            Dictionary<string, List<String>> queryParameters = new Dictionary<string, List<string>>();
            queryParameters.Add(Constants.InstitutionsParameters.Hei.ToString(), hei_id);

            ResponseBuilder builder = _responseBuilderFactory.Create(ResponseBuilderFactory.ResponseBuilderType.Institutions);
            IResponse reponseObject = builder.Build(Request, Response, queryParameters);
            await reponseObject.WriteXmlBody(Response);
            return Ok();
        }
        /// <summary>
        /// Institutions API Post Method
        /// </summary>
        /// <returns>XML format of InstitutionResponse</returns>
        /// <remarks>
        /// The method returns institution information 
        /// <br />
        /// <para>
        /// Sample url: <br />  
        ///     https://host:port/api/Institutions
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
        ///     hei_id=iyte.edu.tr
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
            List<string> hei_ids = BodyReader.ReadParameterValues(Request, "hei_id").Result;
            Dictionary<string, List<String>> queryParameters = new Dictionary<string, List<string>>();
            queryParameters.Add(Constants.InstitutionsParameters.Hei.ToString(), hei_ids);

            ResponseBuilder builder = _responseBuilderFactory.Create(ResponseBuilderFactory.ResponseBuilderType.Institutions);
            IResponse reponseObject = builder.Build(Request, Response, queryParameters);
            await reponseObject.WriteXmlBody(Response);
            return Ok();
        }
       
    }
}
