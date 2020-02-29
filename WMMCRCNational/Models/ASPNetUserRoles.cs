using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WMMCRCNational.Models
{
    public class ASPNetUserRoles
    {
        [Key]
        [Column(Order = 0)]
        public string UserId { get; set; }

        [NotMapped]
        public string MemberName { get; set; }

        [Key]
        [Column(Order = 1)]
        public string RoleId { get; set; }

        [NotMapped]
        public string RoleName { get; set; }
    }
}