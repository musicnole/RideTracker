namespace WMMCRCNational.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ride
    {
        [Key]
        public int RideId { get; set; }
        [Required]
        public int MemberId { get; set; }

        [Column(TypeName = "date")]
        [Required]
        [DisplayFormat(DataFormatString = "{0:M/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime RideDate { get; set; }
        [Required]
        public int RideFrom { get; set; }
        [Required]
        public int RideTo { get; set; }

        [Required]
        [StringLength(250)]
        public string Miles { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateModified { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateCreated { get; set; }
        [NotMapped]
        public string MemberName { get; set; }
        [NotMapped]
        public string RideFromName { get; set; }
        [NotMapped]
        public string RideToName { get; set; }

        public bool Partial { get; set; }

        public bool VerifiableEvent { get; set; }

        public bool Cage { get; set; }

        public string RideNotes { get; set; }

        [NotMapped]
        public int ChapterId { get; set; }

    }
}
