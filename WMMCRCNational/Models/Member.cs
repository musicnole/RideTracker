//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class Member
    {
        public int MemberId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RoadName { get; set; }
        public Nullable<System.DateTime> StandupDate { get; set; }
        public Nullable<System.DateTime> PatchDate { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<System.DateTime> DateModified { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public string FullName { get; set; }
    }
}
