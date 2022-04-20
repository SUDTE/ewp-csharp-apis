using EwpApi.Dto;
using EwpApi.Dto.ewp_specs_api_institutions;
using EwpApi.Dto.ewp_specs_types_address;
using EwpApi.Dto.ewp_specs_types_contact;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace EwpApi.Service.Builders.Adaptees
{
    public class InstitutionsResponseAdaptee : IReadAdaptee
    {        

        public IResponse GenerateSampleData(Dictionary<string, List<string>> parameters)
        {
            List<string> hei_id = parameters.GetValueOrDefault(Constants.InstitutionsParameters.Hei.ToString());
            
            Log.Information("Insititutions API is called. Hei_id = " + String.Join(", ", hei_id.ToArray()));
            ResponseInstitutions subReq = new ResponseInstitutions();


            if (hei_id.Where<string>(x => x.Equals("iyte.edu.tr")).ToList().Count() != 0)
            {
                Contact newContact = new Contact();
                newContact.persongivennames = new string[] { "John" };
                newContact.personfamilyname = new string[] { "Doe" };
                newContact.contactname = new string[] { "John doe" };
                newContact.email = new string[] { "johndoe@iyte.edu.tr" };
                newContact.mailingaddress = new mailingaddress()
                {
                    AddressLines = new List<string>() { "Urla İzmir" },
                    Locality = "izmir",
                    PostalCode = "35430",
                    Country = "TR"
                };
                newContact.streetaddress = new streetaddress()
                {
                    RecipientNames = new List<string>() { "John Doe" },
                    Country = "TR",
                    Locality = "Izmir",
                    PostalCode = "35430"

                };

                newContact.phonenumber = new List<phonenumber>() {
                    new phonenumber()
                    {
                    e164 = "+902327507667",
                    }
                };

                Hei heiObj = new Hei();
                heiObj.heiid = "iyte.edu.tr";
                heiObj.abbreviation = "Iyte";
                heiObj.contact = new Contact[] { newContact };
                heiObj.ounitid = new string[] { "1", "3", "5", "7" };
                heiObj.name = new List<HeiName>(){
                    new HeiName() { lang = "tr", Value = "İzmir Yüksek Teknoloji Enstitüsü" },
                    new HeiName() { lang = "en", Value = "Izmir Institute of Technology" }
                };
                heiObj.websiteurl = new List<HeiWebsiteurl>() {
                    new HeiWebsiteurl() { lang = "tr", Value = "https://www.iyte.edu.tr" },
                    new HeiWebsiteurl() { lang = "en", Value = "https://www.iyte.edu.tr" }
                };
                heiObj.otherid = new List<HeiOtherid>() {
                    new HeiOtherid() { type = "pic", Value = "999869308" },
                    new HeiOtherid() { type = "erasmus", Value = "TR IZMIR03" }
                };


                subReq.hei = new Hei[] { heiObj };

                Log.Information("Response body :\n" + JsonSerializer.Serialize(subReq));
                Log.Information("================================== Request is replied =================================");

            }

            return subReq;
        }


    }
}
