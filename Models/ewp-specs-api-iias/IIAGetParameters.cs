using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EwpApi.Dto.ewp_specs_api_iias
{
    public class IIAGetParameters
    {
        public List<string> hei_id { get; set; }
        public List<string> iia_id { get; set; }
        public List<string> iia_code { get; set; }
        public string send_pdf { get; set; }
    }
}
