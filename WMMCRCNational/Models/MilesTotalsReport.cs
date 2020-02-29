using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WMMCRCNational.Models
{
    public class MilesTotalsReport
    {
        [Key]
        public int MilesTotalsReportId { get; set; }
        public int MemberId { get; set; }

        [StringLength(150)]
        [Display(Name = "Member")]
        public string FullName { get; set; }

        public int Miles { get; set; }


    }
}