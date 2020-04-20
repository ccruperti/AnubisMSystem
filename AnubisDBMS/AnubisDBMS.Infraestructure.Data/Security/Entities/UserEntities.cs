using AnubisDBMS.Data.Localization.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace AnubisDBMS.Infraestructure.Data.Security.Entities
{
    /// <summary>
    ///     Entidad base para identificar un Usuario. Hereda de <see cref="IdentityUser" />.
    ///     <para>
    ///         Para extender propiedades de usuario heredar de esta clase.
    ///     </para>
    /// </summary>
    public class AnubisDBMSUser : IdentityUser<long, AnubisDBMSLogin, AnubisDBMSRole, AnubisDBMSClaim>
    {
        public AnubisDBMSUser()
        {
            FechaRegistro = DateTime.Now;
            Activo = true;
        }

        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string TipoUsuario { get; set; }
        public string Cedula { get; set; }
        public string Celular { get; set; }
        public int PrimeraNotificacion { get; set; }
        public int SegundaNotificacion { get; set; }
        public int TerceraNotificacion { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime? FechaActivacion { get; set; }
        public DateTime? FechaDesactivacion { get; set; }
        public DateTime? FechaExpiracion { get; set; }
        public DateTime? FechaModificacion { get; set; }
   
    }

    /// <summary>
    ///     Entidad relacional entre Roles y Usuario Usuario. Hereda de <see cref="IdentityUserRole" />.
    ///     <para>
    ///         Para extender propiedades de usuario heredar de esta clase.
    ///     </para>
    /// </summary>
    public class AnubisDBMSRole : IdentityUserRole<long>
    {
        public AnubisDBMSRole()
        {
            Activo = true;
            FechaRegistro = DateTime.Now;
        }
        public bool Activo { get; set; }
        public bool Primario { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public DateTime? FechaEliminacion { get; set; }
    }

    public class AnubisDBMSClaim : IdentityUserClaim<long>
    {
    }

    public class AnubisDBMSLogin : IdentityUserLogin<long>
    {
    }
}