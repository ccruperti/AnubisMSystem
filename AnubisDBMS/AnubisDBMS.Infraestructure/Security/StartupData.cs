using System;
using System.Collections.Generic;
using AnubisDBMS.Infraestructure.Data.Security.Entities;
using AnubisDBMS.Infraestructure.Security.Managers;
using Microsoft.AspNet.Identity;

namespace AnubisDBMS.Infraestructure.Security
{
    public static class StartupData
    {
        public static void DefaultRoles(AnubisDBMSRoleManager roleManager, List<Data.Security.Entities.AnubisDBMSUserRole> roleDataList = null)
        {
            roleDataList = roleDataList ?? new List<AnubisDBMSUserRole>();
            //roleDataList.Add(new Data.Security.Entities.AnubisDBMSUserRole("Administrador Sistema", "Administradores de Sistema",
            //    "Usuario maestro con mayor nivel de privilegios. Generados por el sistema.", true,1));
            roleDataList.Add(new Data.Security.Entities.AnubisDBMSUserRole("Administrador", "Administradores",
                "Usuario con privilegios de administrador, puede crear otros usuarios.",true, 2));
            roleDataList.Add(new Data.Security.Entities.AnubisDBMSUserRole("Usuario", "Usuarios",
               "Usuario regular.", true, 2));


            foreach (var role in roleDataList)
                if (!roleManager.RoleExists(role.Name))
                    roleManager.Create(role);
        }

        public static void DefaultUsers(AnubisDBMSUserManager userManager, List<AnubisDBMSUser> userDataList = null)
        {
           


            
            ///////
            var systemAdmin = new AnubisDBMSUser
            {
                UserName = "system",
                Email = "system@test.com"
            };

            if (userManager.FindByName(systemAdmin.UserName) == null)
            {
                var creation = userManager.Create(systemAdmin, "test2019");
                if (creation.Succeeded)
                    userManager.AddToRole(systemAdmin.Id, "Developers");
            }

            var systemDev = new AnubisDBMSUser
            {
                UserName = "dev@test.com",
                Email = "dev@test.com"
            };

            if (userManager.FindByName(systemDev.UserName) == null)
            {
                var creation = userManager.Create(systemDev, "cr2019");
                if (creation.Succeeded)
                    userManager.AddToRole(systemDev.Id, "Developers");
            }

             
        }
    }
}