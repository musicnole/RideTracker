namespace WMMCRCNational.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using WMMCRCNational.Models;

    public partial class WMMCRC : DbContext
    {
        public WMMCRC()
            : base("name=WMMCRC")
        {
        }

        public virtual DbSet<ChapterAddress> ChapterAddresses { get; set; }
        public virtual DbSet<Chapter> Chapters { get; set; }
        public virtual DbSet<ChaptersMile> ChaptersMiles { get; set; }
        public virtual DbSet<ChaptersMilesName> ChaptersMilesNames { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<Mile> Miles { get; set; }
        public virtual DbSet<Ride> Rides { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<SystemUser> SystemUsers { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRoles>()
               .Property(e => e.Id)
               .IsUnicode(false);

            modelBuilder.Entity<AspNetUsers>()
               .Property(e => e.Id)
               .IsUnicode(false);

            modelBuilder.Entity<ASPNetUserRoles>()
               .Property(e => e.UserId)
               .IsUnicode(false);

            modelBuilder.Entity<ChapterAddress>()
                .Property(e => e.StreetAddress1)
                .IsUnicode(false);

            modelBuilder.Entity<ChapterAddress>()
                .Property(e => e.StreetAddress2)
                .IsUnicode(false);

            modelBuilder.Entity<ChapterAddress>()
                .Property(e => e.City)
                .IsUnicode(false);

            modelBuilder.Entity<Chapter>()
                .Property(e => e.ChapterName)
                .IsUnicode(false);

            modelBuilder.Entity<Chapter>()
                .Property(e => e.ChapterNickName)
                .IsUnicode(false);

            modelBuilder.Entity<ChaptersMilesName>()
                .Property(e => e.FromChapter)
                .IsUnicode(false);

            modelBuilder.Entity<ChaptersMilesName>()
                .Property(e => e.ToChapter)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.RoadName)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.FullName)
                .IsUnicode(false);

            modelBuilder.Entity<Ride>()
                .Property(e => e.Miles)
                .IsFixedLength();

            modelBuilder.Entity<State>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<State>()
                .Property(e => e.Name)
                .IsUnicode(false);

        }

        public System.Data.Entity.DbSet<WMMCRCNational.Models.ChapterVisitReport> ChapterVisitReports { get; set; }

        public System.Data.Entity.DbSet<WMMCRCNational.Models.MilesTotalsReport> MilesTotalsReports { get; set; }

        public System.Data.Entity.DbSet<WMMCRCNational.Models.ASPNetUserRoles> ASPNetUserRoles { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }

        public System.Data.Entity.DbSet<WMMCRCNational.Models.Event> Events { get; set; }

    }
}
