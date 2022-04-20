using EwpApi.Dto;
using System.Collections.Generic;

namespace EwpApi.Service.Builders.Adaptees
{
    public interface IReadAdaptee
    {
        public IResponse GenerateSampleData(Dictionary<string, List<string>> parameters);
    }
}
