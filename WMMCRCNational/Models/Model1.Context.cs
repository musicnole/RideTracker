﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WMMCRCNational.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class WMMCRCEntities1 : DbContext
    {
        public WMMCRCEntities1()
            : base("name=WMMCRCEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<ChapterAddress> ChapterAddresses { get; set; }
        public virtual DbSet<Chapter> Chapters { get; set; }
        public virtual DbSet<ChaptersMile> ChaptersMiles { get; set; }
        public virtual DbSet<ChaptersMilesName> ChaptersMilesNames { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<Mile> Miles { get; set; }
        public virtual DbSet<Ride> Rides { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<SystemUser> SystemUsers { get; set; }
        public virtual DbSet<aspnet_ApplicationsDROP20161230> aspnet_ApplicationsDROP20161230 { get; set; }
        public virtual DbSet<aspnet_MembershipDROP20161230> aspnet_MembershipDROP20161230 { get; set; }
        public virtual DbSet<aspnet_PathsDROP20161230> aspnet_PathsDROP20161230 { get; set; }
        public virtual DbSet<aspnet_UsersDROP20161230> aspnet_UsersDROP20161230 { get; set; }
    }
}
