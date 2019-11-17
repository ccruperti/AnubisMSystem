using System.Data.Entity;
using System.Linq;
using AnubisDBMS.Infraestructure.Data.Security.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AnubisDBMS.Infraestructure.Security.Stores
{
    public class AnubisDBMSUserStore : UserStore<AnubisDBMSUser, Data.Security.Entities.AnubisDBMSUserRole, long, AnubisDBMSLogin,
        Data.Security.Entities.AnubisDBMSRole, AnubisDBMSClaim>
    {
        public AnubisDBMSUserStore(DbContext context) : base(context)
        {
        }

        public IQueryable<AnubisDBMSUser> GetUsersInRole(Data.Security.Entities.AnubisDBMSRole role)
        {
            return Users.Where(c => c.Roles.Any(r => r.RoleId == role.RoleId && r.Activo));
        }
    }
}