using System.Security.Claims;
using System.Threading.Tasks;
using AnubisDBMS.Infraestructure.Security.Managers;
using AnubisDBMS.Infraestructure.Data.Security.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System.Net;

namespace AnubisDBMS.Infraestructure.Security.Managers
{
    public class AnubisDBMSSignInManager : SignInManager<AnubisDBMSUser, long>
    {
        public AnubisDBMSSignInManager(AnubisDBMSUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(AnubisDBMSUser user)
        {
            return UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
        }

        public static AnubisDBMSSignInManager Create(IdentityFactoryOptions<AnubisDBMSSignInManager> options,
            IOwinContext context)
        {
            return new AnubisDBMSSignInManager(context.GetUserManager<AnubisDBMSUserManager>(), context.Authentication);
        }
    }
}