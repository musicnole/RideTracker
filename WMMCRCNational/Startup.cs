using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using WMMCRCNational.Models;

[assembly: OwinStartupAttribute(typeof(WMMCRCNational.Startup))]
namespace WMMCRCNational
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
        }
        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!roleManager.RoleExists("Admin"))
            {

                // first we create Admin rool   
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                //Here we create a Admin super user who will maintain the website                  

                var user = new ApplicationUser();
                user.UserName = "Joe DeForte";
                user.Email = "music_nole@yahoo.com";

                string userPWD = "$Boston01";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");

                }

            }
            // creating Creating Member role    
            if (!roleManager.RoleExists("Member"))
            {
                var roleMember = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                roleMember.Name = "Member";
                roleManager.Create(roleMember);

            }

            // creating Creating RideCaptain role    
            if (!roleManager.RoleExists("RideCaptain"))
            {
                var roleRc = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                roleRc.Name = "RideCaptain";
                roleManager.Create(roleRc);

            }
        }
    }
}
