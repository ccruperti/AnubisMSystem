using System.Data.Entity;
using AnubisDBMS.Infraestructure.Data.Security.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Runtime.Remoting.Contexts;

namespace AnubisDBMS.Infraestructure.Security.Stores
{
    public class AnubisDBMSRoleStore : RoleStore<Infraestructure.Data.Security.Entities.AnubisDBMSUserRole, long, Data.Security.Entities.AnubisDBMSRole>
    {
        public AnubisDBMSRoleStore(DbContext context) : base(context)
        {
        }
    }
}