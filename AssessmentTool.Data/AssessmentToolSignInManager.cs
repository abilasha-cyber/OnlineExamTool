using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using AssessmentTool.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AssessmentTool.Data
{
    // Configure the application sign-in manager which is used in this application.
    public class AssessmentToolSignInManager : SignInManager<AssessmentToolUser, string>
    {
        public AssessmentToolSignInManager(AssessmentToolUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(AssessmentToolUser user)
        {
            return user.GenerateUserIdentityAsync((AssessmentToolUserManager)UserManager);
        }

        public static AssessmentToolSignInManager Create(IdentityFactoryOptions<AssessmentToolSignInManager> options, IOwinContext context)
        {
            return new AssessmentToolSignInManager(context.GetUserManager<AssessmentToolUserManager>(), context.Authentication);
        }
    }
}
