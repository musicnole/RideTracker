namespace WMMCRCNational.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Mile
    {
        public Mile()
        { }

        public Mile(int milesId, int fromId, int toId, int miles, bool active, DateTime? dateModified, DateTime? dateCreated, string googleUri)
        {
            MilesId = milesId;
            FromId = fromId;
            ToId = toId;
            Miles = miles;
            Active = active;
            DateModified = dateModified;
            DateCreated = dateCreated;
            GoogleUri = googleUri;
        }

        [Key]
        public int MilesId { get; set; }

        public int FromId { get; set; }
        [NotMapped]
        public string FromName { get; set; }

        public int ToId { get; set; }
        [NotMapped]
        public string ToName { get; set; }

        public int Miles { get; set; }

        public bool Active { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateModified { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateCreated { get; set; }

        [NotMapped]
        public string FromChapterAddress { get; set; }

        [NotMapped]
        public string ToChapterAddress { get; set; }

        public string GoogleUri { get; set; }
    }
}
