using EwpApi.Dto;
using EwpApi.Dto.ewp_specs_api_iias;
using System.Collections.Generic;

namespace EwpApi.Service.Builders.Adaptees
{
    public class IIAsIndexResponseAdaptee :  IReadAdaptee
    {
        public IResponse GenerateSampleData(Dictionary<string, List<string>> parameters)
        {
            ResponseIIAsIndex response = new ResponseIIAsIndex();
            response.iiaIds = new List<string>() { "1", "2" };
            return response;
        }
    }
}
