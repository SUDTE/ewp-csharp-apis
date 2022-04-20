using EwpApi.Dto;
using EwpApi.Dto.ewp_specs_api_ounits;
using EwpApi.Dto.ewp_specs_architecture;
using EwpApi.Dto.ewp_specs_types_address;
using EwpApi.Dto.ewp_specs_types_contact;
using EwpApi.Helper;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EwpApi.Service.Builders.Adaptees
{
    public class OrganizationsResponseAdaptee : IReadAdaptee
    {
        public IResponse GenerateSampleData(Dictionary<string, List<string>> parameters)
        {
            List<string> heiIds = parameters.GetValueOrDefault(Constants.OrganizationsParameters.HeiId.ToString());
            List<string> ounitIds = parameters.GetValueOrDefault(Constants.OrganizationsParameters.OunitId.ToString());
            List<string> ounitCodes = parameters.GetValueOrDefault(Constants.OrganizationsParameters.OunitCode.ToString());


            ResponseOrganization response = new ResponseOrganization();
            response.ounit = new List<Ounit>();


            if ((ounitIds.Count > 0 && !(ounitIds[0].Equals("73"))) || (ounitCodes.Count > 0 && !(ounitCodes[0].Equals("454545"))))
            {

            }
            else
            {


                Ounit unit = new Ounit();
                unit.Ounitid = "73";
                unit.Ounitcode = "454545";

                unit.Name = new List<StringWithOptionalLang>(){
                    new StringWithOptionalLang
                    {
                        Value = "Bilgisayar Mühendisliği",
                        lang = "tr"
                    },
                     new StringWithOptionalLang
                     {
                         Value = "Computer Science",
                         lang = "en"
                     }
                 };
                unit.Parentounitid = "5";


                unit.Mailingaddress = new mailingaddress()
                {

                    Locality = "izmir",
                    PostalCode = "35430",
                    Country = "TR"
                };
                unit.Mailingaddress.AddressLines.Add("Urla İzmir");

                unit.Streetaddress = new streetaddress()
                {
                    Country = "TR",
                    Locality = "Izmir",
                    PostalCode = "35430"

                };
                unit.Streetaddress.RecipientNames.Add("bulentyigitalp@iyte.edu.tr");


                unit.Contact = new List<Contact>{
                    new EwpApi.Dto.ewp_specs_types_contact.Contact
                    {
                        contactname = new string[] { "John Doe (Faculty's Mobility Coordinator)" },
                        email = new string[] { "bulentyigitalp@iyte.edu.tr" },
                        roledescription = new MultilineStringWithOptionalLang
                        {
                            lang = "en",
                            Value="John Doe is advising students who are interested in studying programmes in Faculty of Mathematics, Informatics, and Mechanics..."
                        }
                    }
                };

                response.ounit.Add(unit);
            }


            return response;
        }
    }
}
