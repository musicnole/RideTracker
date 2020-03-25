
namespace WMMCRCNational.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ChapterAddress")]
    public partial class ChapterAddress
    {
        [Key]
        public int ChapterAddressId { get; set; }

        public int ChapterId { get; set; }

        [StringLength(250)]
        public string StreetAddress1 { get; set; }

        [StringLength(250)]
        public string StreetAddress2 { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        public int? StateId { get; set; }

        public string Zip { get; set; }

        public bool? Active { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateModified { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateCreated { get; set; }
    }
}
