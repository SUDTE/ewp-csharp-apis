using EwpApi.Dto;
using EwpApi.Dto.ewp_specs_api_ounits;
using EwpApi.Dto.ewp_specs_architecture;
using EwpApi.Dto.ewp_specs_types_address;
using EwpApi.Dto.ewp_specs_types_contact;
using EwpApi.Helper;
using EwpApi.Service;
using EwpApi.Service.Builders;
using EwpApi.Service.Builders.Adaptees;
using EwpApi.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EwpApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationsController : ControllerBase
    {
        private readonly ILogger<OrganizationsController> _logger;
        private readonly ResponseBuilderFactory _responseBuilderFactory;


        public OrganizationsController(ILogger<OrganizationsController> logger, ResponseBuilderFactory builderFactory)
        {
            _logger = logger;
            _responseBuilderFactory = builderFactory;
        }

        /// <summary>
        /// Organizations API Get Method
        /// </summary>
        /// <param name="hei_id"></param>
        /// <param name="ounit_id"></param>
        /// <param name="ounit_code"></param>
        /// <remarks>
        /// The method returns institution information 
        /// <br />
        /// <para>
        /// Sample url: <br />  
        ///     https://host:port/api/Organizations?hei_id=iyte.edu.tr&amp;ounit_id=74 <br /> 
        ///     https://host:port/api/Organizations?hei_id=iyte.edu.tr&amp;ounit_code=787897
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
        public async Task<IActionResult> Get([FromQuery] List<string> hei_id, [FromQuery] List<string> ounit_id, [FromQuery] List<string> ounit_code)
        {
            Dictionary<string, List<String>> queryParameters = new Dictionary<string, List<string>>();
            queryParameters.Add(Constants.OrganizationsParameters.HeiId.ToString(), hei_id);
            queryParameters.Add(Constants.OrganizationsParameters.OunitId.ToString(), ounit_id);
            queryParameters.Add(Constants.OrganizationsParameters.OunitCode.ToString(), ounit_code);

            ResponseBuilder builder = _responseBuilderFactory.Create(ResponseBuilderFactory.ResponseBuilderType.Organizations);
            IResponse reponseObject = builder.Build(Request, Response, queryParameters);
            await reponseObject.WriteXmlBody(Response);
            return Ok();
        }

        /// <summary>
        /// Organizations API Post Method
        /// </summary>       
        /// <remarks>
        /// The method returns institution information 
        /// <br />
        /// <para>
        /// Sample url: <br />  
        ///     https://host:port/api/Organizations
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
        ///     hei_id=iyte.edu.tr&amp;ounit_id=74 <br />
        ///     hei_id=iyte.edu.tr&amp;ounit_code=78787 <br />
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
            String body = await BodyReader.ReadRequestBody(Request);
            List<string> hei_ids = await BodyReader.ReadParameterValues(body, "hei_id");
            List<string> oUnitIds = await BodyReader.ReadParameterValues(body, "ounit_id");
            List<string> oUnitCodes = await BodyReader.ReadParameterValues(body, "ounit_code");

           
            Dictionary<string, List<String>> queryParameters = new Dictionary<string, List<string>>();
            queryParameters.Add(Constants.OrganizationsParameters.HeiId.ToString(), hei_ids);
            queryParameters.Add(Constants.OrganizationsParameters.OunitId.ToString(), oUnitIds);
            queryParameters.Add(Constants.OrganizationsParameters.OunitCode.ToString(), oUnitCodes);

            ResponseBuilder builder = _responseBuilderFactory.Create(ResponseBuilderFactory.ResponseBuilderType.Organizations);
            IResponse reponseObject = builder.Build(Request, Response, queryParameters);
            await reponseObject.WriteXmlBody(Response);
            return Ok();
        }
               
    }
}
