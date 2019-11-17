using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnubisDBMS.Data;
using AnubisDBMS.Infraestructura.Data;
using AnubisDBMS.Infraestructure.Data.Security.Entities;
using AnubisDBMS.Infraestructure.Security.Stores;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AnubisDBMS.Infraestructure.Security.Managers
{
    public class AnubisDBMSUserManager : UserManager<AnubisDBMSUser, long>
    {

        public AnubisDBMSUserManager(AnubisDBMSUserStore store)
            : base(store)
        {
            Store = store;
        }

      

        public static AnubisDBMSUserManager Create(IdentityFactoryOptions<AnubisDBMSUserManager> options,
            IOwinContext context)
        {
            var manager = new AnubisDBMSUserManager(new AnubisDBMSUserStore(context.Get<AnubisDBMSDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<AnubisDBMSUser, long>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = false
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 3,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false
            };

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<AnubisDBMSUser, long>(
                        dataProtectionProvider.Create("Authorization Server"));
            return manager;
        }

        public async Task<IdentityResult> ManualResetPasswordAsync(long userId, string password)
        {
            var user = await FindByIdAsync(userId);
            if (user == null)
                return IdentityResult.Failed("No se ha encontrado ese usuario.");

            if (!(Store is IUserPasswordStore<AnubisDBMSUser, long> store))
            {
                return IdentityResult.Failed("El UserStore usado no implementa PasswordStore");
            }

            var newPasswordHash = this.PasswordHasher.HashPassword(password);
            try
            {
                await store.SetPasswordHashAsync(user, newPasswordHash);
                await store.UpdateAsync(user);
            }
            catch (Exception e)
            {
                return IdentityResult.Failed("Error interno: " + e.Message);
            }
            return IdentityResult.Success;
        }

        public List<AnubisDBMSUser> GetUsersInRole(long roleId)
        {
            return this.Users.Where(c => c.Roles.Any(r => r.RoleId == roleId)).ToList();
        }

        public List<AnubisDBMSUser> GetUsersInRole(long roleId, long userId)
        {
            return this.Users.Where(c => c.Roles.Any(r => r.RoleId == roleId) && c.Id != userId).ToList();
        }
    }
}