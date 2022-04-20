using EwpApi.Dto;
using EwpApi.Dto.ewp_specs_api_iias;
using EwpApi.Helper;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EwpApi.Service.Builders.Adaptees
{
    public class IIAsGetResponseAdaptee : IReadAdaptee
    {
        public IResponse GenerateSampleData(Dictionary<string, List<string>> parameters)
        {
            List<string> heiIds = parameters.GetValueOrDefault(Constants.IIAsGetParameters.HeiId.ToString());
            List<string> iiaCodes = parameters.GetValueOrDefault(Constants.IIAsGetParameters.IiaCode.ToString());
            List<string> iiaIds = parameters.GetValueOrDefault(Constants.IIAsGetParameters.IiaId.ToString());
            List<string> sendPdf = parameters.GetValueOrDefault(Constants.IIAsGetParameters.SendPdf.ToString());

            IIAPartner partner1 = new IIAPartner()
            {
                HeiId = "uma.es",
                OUnitId = "8965F285-E763-IKEA-8163-C52C8B654037",
                IIAId = (iiaIds != null && iiaIds.Count > 0) ? iiaIds[0] : "8C182E5C-01C3-4124-BF3B-614657DBB0D4",
                IIACode = (iiaCodes != null && iiaCodes.Count > 0) ? iiaCodes[0] : "IK-UMA-01"
            };
            IIAPartner partner2 = new IIAPartner()
            {
                HeiId = "validator-hei01.developers.erasmuswithoutpaper.eu",
                OUnitId = "8965F285-E763-IKEA-8163-C52C8B654035",
                IIAId = "8C182E5C-01C3-4124-BF3B-614657DBB0D4",
                IIACode = "IK-UMA-01"
            };
            StudentStudiesMobility studiesMobility = new StudentStudiesMobility()
            {
                SendingHeiId = "validator-hei01.developers.erasmuswithoutpaper.eu",
                SendingHeiOunitId = "8965F285-E763-IKEA-8163-C52C8B654035",
                ReceivingHeiId = "uma.es",
                ReceivingHeiOunitId = "8965F285-E763-IKEA-8163-C52C8B654037",
                ReceivingAcademicYearId = new List<string>() { "2016/2017", "2017/2018" },
                MobilitiesPerYear = 3,
                totalMonthsPerYear = 2,
                Blended = false
            };

            ResponseIIAsGet response = new ResponseIIAsGet();
            response.iia = new List<IIA> {
                new IIA(){
                    Partner = new List<IIAPartner>{ partner1, partner2 },
                    ineffect = false,
                    cooperationconditions = new IIACooperationConditions()
                    {
                        studentstudiesmobilityspec = new List<StudentStudiesMobility>(){ studiesMobility }
                    }
                }
            };
            return response;
        }
    }
}
