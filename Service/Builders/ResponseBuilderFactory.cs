using EwpApi.Service.Builders.Adaptees;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EwpApi.Service.Builders
{
    public class ResponseBuilderFactory
    {        
        public ResponseBuilderFactory()
        {
            
        }

        public enum ResponseBuilderType
        {
            Echo,
            Error,
            Institutions,
            Organizations,
            IIAget,
            IIAindex
        }
        public ResponseBuilder Create(ResponseBuilderType type)
        {
            switch (type)
            {
                case ResponseBuilderType.Echo:
                    return new EchoResponseBuilder();
                case ResponseBuilderType.Error:
                    return new EchoResponseBuilder();
                case ResponseBuilderType.Institutions:
                    return new ResponseBuilderAdapter(new InstitutionsResponseAdaptee()); 
                case ResponseBuilderType.Organizations:
                    return new ResponseBuilderAdapter(new OrganizationsResponseAdaptee());
                case ResponseBuilderType.IIAget:
                    return new ResponseBuilderAdapter(new IIAsGetResponseAdaptee());
                case ResponseBuilderType.IIAindex:
                    return new ResponseBuilderAdapter(new IIAsIndexResponseAdaptee());
            }
            return null;
        }
    }
}
