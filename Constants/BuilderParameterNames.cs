using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EwpApi.Constants
{
    public class BuilderParameterNames
    {
        
    }

    public enum ErrorParameters
    {
        [Display(Name = "developerMessage")]
        DeveloperMessage,
        [Display(Name = "userMessage")]
        UserMessage
    }
    public enum EchoParameters
    {
        [Display(Name = "echo")]
        Echo,
        [Display(Name = "hei")]
        Hei
    }

    public enum InstitutionsParameters
    {        
        [Display(Name = "hei")]
        Hei
    }

    public enum OrganizationsParameters
    {
        [Display(Name = "hei_id")]
        HeiId,
        [Display(Name = "ounit_id")]
        OunitId,
        [Display(Name = "ounit_code")]
        OunitCode
    }
    
    public enum IIAsIndexParameters
    {
        [Display(Name = "hei_id")]
        HeiId,
        [Display(Name = "partner_hei_id")]
        PartnerHeiId,
        [Display(Name = "receiving_academic_year_id")]
        RecievingAcademicYearId,
        [Display(Name = "modified_since")]
        ModifiedSince
    }
   
    public enum IIAsGetParameters
    {
        [Display(Name = "hei_id")]
        HeiId,
        [Display(Name = "iia_id")]
        IiaId,
        [Display(Name = "iia_code")]
        IiaCode,
        [Display(Name = "send_pdf")]
        SendPdf
    }
}
