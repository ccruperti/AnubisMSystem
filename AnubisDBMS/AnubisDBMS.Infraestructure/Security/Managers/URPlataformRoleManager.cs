using System.Linq;
using AnubisDBMS.Data;
using AnubisDBMS.Infraestructura.Data;
using AnubisDBMS.Infraestructure.Data.Security.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using AnubisDBMS.Infraestructure.Security.Stores;

namespace AnubisDBMS.Infraestructure.Security.Managers
{
    public class AnubisDBMSRoleManager : RoleManager<AnubisDBMSUserRole, long>
    {
        public AnubisDBMSRoleManager(IRoleStore<AnubisDBMSUserRole, long> store) : base(store)
        {
        }

        

        public override IQueryable<AnubisDBMSUserRole> Roles
        {
            get { return base.Roles.Where(c => c.Activo).OrderBy(c => c.Prioridad).ThenBy(c => c.Orden); }
        }
        
        public IQueryable<AnubisDBMSUserRole> SystemRoles
        {
            get { return base.Roles.Where(c => c.Activo && c.Sistema).OrderBy(c => c.Prioridad).ThenBy(c => c.Orden); }
        }

        public IQueryable<AnubisDBMSUserRole> CustomRoles
        {
            get
            {
                return base.Roles.Where(c => c.Activo && c.Sistema == false).OrderBy(c => c.Prioridad)
                    .ThenBy(c => c.Orden);
            }
        }

        public IQueryable<AnubisDBMSUserRole> AvailableEditRoles(int prioridad)
        {
            return base.Roles.Where(c => c.Activo && c.Prioridad >= prioridad).OrderBy(c => c.Prioridad).ThenBy(c => c.Orden);
        }

        public static AnubisDBMSRoleManager Create(IdentityFactoryOptions<AnubisDBMSRoleManager> options,
            IOwinContext context)
        {
            return new AnubisDBMSRoleManager(new AnubisDBMSRoleStore(context.Get<AnubisDBMSDbContext>()));
        }
    }
}