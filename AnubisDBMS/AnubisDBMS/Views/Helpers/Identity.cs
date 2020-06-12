using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using AnubisDBMS.Controllers;
using AnubisDBMS.Data;

namespace AnubisDBMS.Views.Helpers
{
    public static class RazorIdentityHelper 
    {
        private static AnubisDbContext _context = new AnubisDbContext();        
        public static string GetUserName(this IIdentity identity)
        {
            return identity.Name;
        }

        public static long GetUserId(this IIdentity identity)
        { 
            var user = _context.Users.FirstOrDefault(c => c.UserName == identity.Name);
            return user != null ?user.Id : 0;
        }
        //public static string GetPaisId(this IIdentity identity)
        //{
       
        //    var claim = ((ClaimsIdentity)identity).FindFirst("IdPais");
        //    if (claim == null)
        //    {
        //        var user = _context.Users.FirstOrDefault(c => c.UserName == identity.Name);
        //        if(user != null)
        //        {
        //            var paisesusuario = _context.DataSensores.FirstOrDefault(c => c.Activo && c.Error && c.AlertaRecibida == false);


        //            if (paisesusuario != null)
        //            {

        //                ((ClaimsIdentity)identity).AddClaim(new Claim("IdPais", paisesusuario.IdPais.ToString(), ClaimValueTypes.Integer32));
        //                claim = ((ClaimsIdentity)identity).FindFirst("IdPais");
        //            }
        //        }

              
        //    }
        //    // Test for null to avoid issues during local testing

        //    return (claim != null) ? claim.Value : "0";
        //}
        public static int ObtenerNumeroAlertasSensores(this IIdentity identity)
        { 
            int pendientes = 0; 
                var current = HttpContext.Current;
                var user = _context.Users.FirstOrDefault(c => c.UserName == identity.Name); 
            pendientes = _context.DataSensores.Count(c => c.Activo && c.Error && c.AlertaRecibida == false && c.IdEmpresa == (user.IdEmpresa??0));
             

            return pendientes;


        }
        public static int ObtenerNumeroAlertasSensoresPerfiles(this IIdentity identity, long IdEmpresa)
        {
            //var claim = ((ClaimsIdentity)identity).FindFirst("NumPendientes");
            int pendientes = 0;


            var current = HttpContext.Current;
            var user = _context.Users.FirstOrDefault(c => c.UserName == identity.Name);

            pendientes = _context.DataSensores.Count(c => c.Activo && c.Error && c.AlertaRecibida == false && c.IdEmpresa ==IdEmpresa);


            //var paisesusuario = _context.UsuarioPais.FirstOrDefault(c => c.Activo && c.IdUsuario == user.Id && c.PaisSeleccionado);



            //((ClaimsIdentity)identity).AddClaim(new Claim("NumPendientes", pendientes.ToString(), ClaimValueTypes.Integer));
            //claim = ((ClaimsIdentity)identity).FindFirst("NumPendientes");


            return pendientes;


        }
        public static string GetNombreEmpresa(this IIdentity identity)
        {



            var current = HttpContext.Current;
            var id = GetUserId(identity);
            var user = _context.Users.FirstOrDefault(c => c.Id == id);

            //pendientes = _context.DataSensores.Count(c => c.Activo && c.Error && c.AlertaRecibida == false);


            //var paisesusuario = _context.UsuarioPais.FirstOrDefault(c => c.Activo && c.IdUsuario == user.Id && c.PaisSeleccionado);



            //((ClaimsIdentity)identity).AddClaim(new Claim("NumPendientes", pendientes.ToString(), ClaimValueTypes.Integer));
            //claim = ((ClaimsIdentity)identity).FindFirst("NumPendientes");


            return user.Empresa?.Nombre ?? "---";
        }

        public static long GetEmpresaId(this IIdentity identity, long? Id = null)
        {
            

            
            var current = HttpContext.Current;
            var id = GetUserId(identity);
            var user = _context.Users.FirstOrDefault(c => c.Id == id);
            if(Id != null)
            {
                user = _context.Users.FirstOrDefault(c => c.Id == Id);
            }
            //pendientes = _context.DataSensores.Count(c => c.Activo && c.Error && c.AlertaRecibida == false);


            //var paisesusuario = _context.UsuarioPais.FirstOrDefault(c => c.Activo && c.IdUsuario == user.Id && c.PaisSeleccionado);



            //((ClaimsIdentity)identity).AddClaim(new Claim("NumPendientes", pendientes.ToString(), ClaimValueTypes.Integer));
            //claim = ((ClaimsIdentity)identity).FindFirst("NumPendientes");


            return user?.IdEmpresa ?? 0;
        }
        //public static int GetNumeroPendientesOrdenComprasGerencia(this IIdentity identity)
        //{
        //    //var claim = ((ClaimsIdentity)identity).FindFirst("NumPendientes");
        //    int pendientes = 0;
         

        //        var current = HttpContext.Current;
        //        var user = _context.Users.FirstOrDefault(c => c.UserName == identity.Name);
   
        //        if (current.User.IsInRole("Gerente Compras") || user.Jefatura)
        //        {
        //            pendientes = _context.OrdenesCompra.Count(c => c.Activo && c.SubtotalOfertas > 3000
        //                                                  && c.SubtotalOfertas <= 10000 && c.EstadoOrdenCompra.Nombre == "APROBADO COMPRAS");
        //        }
   
        //            //var paisesusuario = _context.UsuarioPais.FirstOrDefault(c => c.Activo && c.IdUsuario == user.Id && c.PaisSeleccionado);



        //        //    ((ClaimsIdentity)identity).AddClaim(new Claim("NumPendientes", pendientes.ToString(), ClaimValueTypes.Integer));
        //        //claim = ((ClaimsIdentity)identity).FindFirst("NumPendientes");

            
        //    return pendientes;


        //}
        //public static int GetNumeroPendientesOrdenComprasJefatura(this IIdentity identity)
        //{
        //    //var claim = ((ClaimsIdentity)identity).FindFirst("NumPendientes");
        //    int pendientes = 0;
        

        //        var current = HttpContext.Current;
        //        var user = _context.Users.FirstOrDefault(c => c.UserName == identity.Name);
         
        //        if (current.User.IsInRole("Jefe Compras") || user.Jefatura)
        //        {
        //            pendientes = _context.OrdenesCompra.Count(c => c.EstadoOrdenCompra.Nombre == "RECIBIDA");
        //        }
        //            //var paisesusuario = _context.UsuarioPais.FirstOrDefault(c => c.Activo && c.IdUsuario == user.Id && c.PaisSeleccionado);



        //        //    ((ClaimsIdentity)identity).AddClaim(new Claim("NumPendientes", pendientes.ToString(), ClaimValueTypes.Integer));
        //        //claim = ((ClaimsIdentity)identity).FindFirst("NumPendientes");

            
        //    return pendientes;


        //}

        //public static string GetPaisName(this IIdentity identity)
        //{

        //    var claim = ((ClaimsIdentity)identity).FindFirst("NombreES");
        //    if (claim == null)
        //    {
        //        var user = _context.Users.FirstOrDefault(c => c.UserName == identity.Name);
        //        if (user != null) { var paisesusuario = _context.UsuarioPais.FirstOrDefault(c => c.Activo && c.IdUsuario == user.Id && c.PaisSeleccionado); 


        //        if (paisesusuario != null)
        //        {

        //            ((ClaimsIdentity)identity).AddClaim(new Claim("NombreES", paisesusuario.Pais.NombreES.ToString(), ClaimValueTypes.Integer32));
        //            claim = ((ClaimsIdentity)identity).FindFirst("NombreES");
        //        }
        //        }
        //    }
        //    // Test for null to avoid issues during local testing

        //    return (claim != null) ? claim.Value : string.Empty;
        //}
        public static bool GetIsAdmin(this IIdentity identity)
        {
            return false;
        }

        //public static bool HasPermission(this IIdentity identity, string grupo, string permiso)
        //{

        //    var userID = identity.GetUserId();

        //    var tienePermisos = _context.PermisosUsuarios.Any(i => i.Activo && i.IdUsuario == userID && i.Permiso.Nombre == permiso);

        //    if (userID == 1)
        //    {
        //        return true;
        //    }
        //    else if (tienePermisos)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return _context.PermisosUsuarios.Any(i => i.IdUsuario == userID && i.Permiso.Guid == grupo && i.Permiso.Nombre == permiso);
        //    }
        //}

        //public static bool HasAnyPermission(this IIdentity identity, params string[] permisos)
        //{

        //    var userID = identity.GetUserId();
        //    var tienePermisos = _context.PermisosUsuarios.Any(i => i.Activo && i.IdUsuario == userID && permisos.Contains(i.Permiso.Guid));

        //    if (tienePermisos)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        var grupo = permisos[0];
        //        permisos = permisos.Where((source, index) => index != 0).ToArray();
        //        return _context.PermisosUsuarios.Any(c => c.IdUsuario == userID && c.Permiso.Guid == grupo && permisos.Contains(c.Permiso.Nombre));
        //    }


        //}
        //public static bool HasPermission(this IIdentity identity, string grupo)
        //{

        //    var userID = identity.GetUserId();

        //    var tienePermisos = _context.PermisosUsuarios.Any(i => i.Activo && i.IdUsuario == userID && i.Permiso.Guid == grupo);

        //    if (userID == 1)
        //    {
        //        return true;
        //    }
        //    else if (tienePermisos)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return _context.PermisosUsuarios.Any(i => i.IdUsuario == userID && i.Permiso.Guid == grupo);
        //    }
        //}
    }
}
