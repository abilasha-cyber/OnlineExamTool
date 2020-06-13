using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using AssessmentTool.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;

namespace AssessmentTool.Data
{
    public class AssessmentToolDatabaseInitializer : CreateDatabaseIfNotExists<AssessmentToolContext>
    {
        protected override void Seed(AssessmentToolContext context)
        {
            //add these roles to database if does not exists
            SeedRoles(context);

            //add these users to database if not exists
            SeedUsers(context);
        }
        
        public void SeedRoles(AssessmentTool.Data.AssessmentToolContext context)
        {
            List<IdentityRole> rolesInAssessmentTool = new List<IdentityRole>();

            rolesInAssessmentTool.Add(new IdentityRole() { Name = "Administrator" });
            rolesInAssessmentTool.Add(new IdentityRole() { Name = "User" });

            var rolesStore = new RoleStore<IdentityRole>(context);
            var rolesManager = new RoleManager<IdentityRole>(rolesStore);
            
            foreach (IdentityRole role in rolesInAssessmentTool)
            {
                if (!rolesManager.RoleExists(role.Name))
                {
                    var result = rolesManager.Create(role);

                    if (result.Succeeded) continue;
                }
            }
        }

        public void SeedUsers(AssessmentTool.Data.AssessmentToolContext context)
        {
            var usersStore = new UserStore<AssessmentToolUser>(context);
            var usersManager = new UserManager<AssessmentToolUser>(usersStore);

            AssessmentToolUser admin = new AssessmentToolUser();
            admin.Email = "admin@email.com";
            admin.UserName = "admin";
            var password = "admin123";

            if (usersManager.FindByEmail(admin.Email) == null)
            {
                var result = usersManager.Create(admin, password);

                if(result.Succeeded)
                {
                    //add necessary roles to admin
                    usersManager.AddToRole(admin.Id, "Administrator");
                    usersManager.AddToRole(admin.Id, "User");
                }
            }
        }
    }
}





