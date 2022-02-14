using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WMMCRCNational.Models;

namespace WMMCRCNational.Helpers
{
    public class GlobalHelper
    {
        
        public static SecurityObjectWM SetSecurity(WMMCRC db)
        {
            SecurityObjectWM secObj = new SecurityObjectWM();

            //Get the current logged in user
            var User = System.Web.HttpContext.Current.User;
            // Not going to use session get the chapter id from the user

            secObj.adminRole = User.IsInRole("Admin");
            secObj.roadCaptainRole = User.IsInRole("Road Captain");
            secObj.memberRole = User.IsInRole("Member");
            secObj.chapterId = (from a in db.AspNetUsers
                         where a.UserName == User.Identity.Name
                         select a.ChapterId).FirstOrDefault();

            return secObj;
        }
    }

    public class SecurityObjectWM
    {
        public bool adminRole { get; set; }
        public bool roadCaptainRole { get; set; }
        public bool memberRole { get; set; }
        public int chapterId { get; set; }

    }
}