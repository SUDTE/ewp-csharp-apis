using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EwpApi.Dto
{
    public interface IResponse
    {
        public string ToXml();

        public Task WriteXmlBody(HttpResponse response);
    }
}
