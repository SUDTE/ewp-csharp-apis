using EwpApi.Dto;
using EwpApi.Dto.ewp_specs_api_iias;
using EwpApi.Helper;
using EwpApi.Service;
using EwpApi.Service.Builders;
using EwpApi.Service.Builders.Adaptees;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EwpApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IIAsController : ControllerBase
    {
        private readonly ResponseBuilderFactory _responseBuilderFactory;

        public IIAsController(ResponseBuilderFactory builderFactory)
        {
            _responseBuilderFactory = builderFactory;
        }

        /// <summary>
        /// IIA Index API Get Method
        /// </summary>        
        /// <remarks>
        /// The method returns IIA id list 
        /// <br />
        /// <para>
        /// Sample url: <br />  
        ///     https://host:port/api/IIA/Index?hei_id=iyte.edu.tr&amp;partner_hei_id=aa.edu
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
        /// <param name="hei_id"></param>
        /// <param name="partner_hei_id"></param>
        /// <param name="receiving_academic_year_id"></param>
        /// <param name="modified_since"></param>
        /// <returns></returns>
        [HttpGet]
        [Produces("application/xml")]
        [Route("Index")]
        public async Task<IActionResult> Index([FromQuery] List<string> hei_id, 
            [FromQuery] List<string> partner_hei_id, 
            [FromQuery] List<string> receiving_academic_year_id, 
            [FromQuery] List<string> modified_since)
        {           
            Dictionary<string, List<String>> queryParameters = new Dictionary<string, List<string>>();
            queryParameters.Add(Constants.IIAsIndexParameters.HeiId.ToString(), hei_id);
            queryParameters.Add(Constants.IIAsIndexParameters.PartnerHeiId.ToString(), partner_hei_id);
            queryParameters.Add(Constants.IIAsIndexParameters.RecievingAcademicYearId.ToString(), receiving_academic_year_id);
            queryParameters.Add(Constants.IIAsIndexParameters.ModifiedSince.ToString(), modified_since);

            ResponseBuilder builder = _responseBuilderFactory.Create(ResponseBuilderFactory.ResponseBuilderType.IIAindex);
            IResponse reponseObject = builder.Build(Request, Response, queryParameters);
            await reponseObject.WriteXmlBody(Response);
            return Ok();
        }

        /// <summary>
        /// IIA Index API Post Method
        /// </summary>        
        /// <remarks>
        /// The method returns IIA id list 
        /// <br />
        /// <para>
        /// Sample url: <br />  
        ///     https://host:port/api/IIA/Index
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
        ///     hei_id=iyte.edu.tr&amp;partner_hei_id=aaa.edu
        ///     hei_id=iyte.edu.tr&amp;partner_hei_id=aaa.edu&amp;receiving_academic_year_id=2020/2021&amp;receiving_academic_year_id=2021/2022
        ///     
        /// </remarks>     
        /// <response code="200">Ok</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>       
        /// <returns></returns>
        [HttpPost]
        [Produces("application/xml")]
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            List<string> heiIds = BodyReader.ReadParameterValues(Request, "hei_id").Result;
            List<string> partnerHeiIds = BodyReader.ReadParameterValues(Request, "partner_hei_id").Result;
            List<string> receivingAcademicYearIds = BodyReader.ReadParameterValues(Request, "receiving_academic_year_id").Result;
            List<string> modifiedSince = BodyReader.ReadParameterValues(Request, "modified_since").Result;

            Dictionary<string, List<String>> queryParameters = new Dictionary<string, List<string>>();
            queryParameters.Add(Constants.IIAsIndexParameters.HeiId.ToString(), heiIds);
            queryParameters.Add(Constants.IIAsIndexParameters.PartnerHeiId.ToString(), partnerHeiIds);
            queryParameters.Add(Constants.IIAsIndexParameters.RecievingAcademicYearId.ToString(), receivingAcademicYearIds);
            queryParameters.Add(Constants.IIAsIndexParameters.ModifiedSince.ToString(), modifiedSince);

            ResponseBuilder builder = _responseBuilderFactory.Create(ResponseBuilderFactory.ResponseBuilderType.IIAindex);
            IResponse reponseObject = builder.Build(Request, Response, queryParameters);
            await reponseObject.WriteXmlBody(Response);
            return Ok();
        }

        /// <summary>
        /// IIA Get API Get Method
        /// </summary>        
        /// <remarks>
        /// The method returns IIA list 
        /// <br />
        /// <para>
        /// Sample url: <br />  
        ///     https://host:port/api/IIA/Get?hei_id=iyte.edu.tr&amp;iia_id=787878
        ///     https://host:port/api/IIA/Get?hei_id=iyte.edu.tr&amp;iia_code=prt2322323
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
        /// <returns></returns>
        [HttpGet]
        [Produces("application/xml")]
        [Route("Get")]
        public async Task<IActionResult> Get([FromQuery] IIAGetParameters parameters)            
        {
            List<string> heiIds = parameters.hei_id;
            List<string> iiaCodes = parameters.iia_code;
            List<string> iiaIds = parameters.iia_id;
            List<string> sendPdf = new List<string> { parameters.send_pdf };

            Dictionary<string, List<String>> queryParameters = new Dictionary<string, List<string>>();
            queryParameters.Add(Constants.IIAsGetParameters.HeiId.ToString(), heiIds);
            queryParameters.Add(Constants.IIAsGetParameters.IiaCode.ToString(), iiaCodes);
            queryParameters.Add(Constants.IIAsGetParameters.IiaId.ToString(), iiaIds);
            queryParameters.Add(Constants.IIAsGetParameters.SendPdf.ToString(), sendPdf);

            ResponseBuilder builder = _responseBuilderFactory.Create(ResponseBuilderFactory.ResponseBuilderType.IIAget);
            IResponse reponseObject = builder.Build(Request, Response, queryParameters);
            await reponseObject.WriteXmlBody(Response);
            return Ok();
        }
    }
}
