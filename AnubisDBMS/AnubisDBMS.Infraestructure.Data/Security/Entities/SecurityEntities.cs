using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using AnubisDBMS.Infraestructure.Data.Security.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AnubisDBMS.Infraestructure.Data.Security.Entities
{
   
    public class AnubisDBMSUserRole : IdentityRole<long, AnubisDBMSRole>
    {
        public AnubisDBMSUserRole()
        {
        }

        public AnubisDBMSUserRole(string nombre, string nombrePlural, string descripcion = null, bool sistema = false, int PrioridadR = 0)
        {
            Name = nombre;
            Plural = nombrePlural;
            Descripcion = descripcion;
            Sistema = sistema;
            Activo = true;
            FechaRegistro = DateTime.Now;
            Prioridad = PrioridadR;
            Orden = PrioridadR;
        }

        public string Plural { get; set; }
        public string Descripcion { get; set; }
        public bool Sistema { get; set; }
        public bool Defecto { get; set; }
        public bool Activo { get; set; }
        public int Prioridad { get; set; }
        public int Orden { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public DateTime? FechaEliminacion { get; set; }
    }

    public class SecurityLevel
    {
        public int IdNivel { get; set; }
        public string Guid { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public virtual ICollection<SecurityArea> AreasSeguridad { get; set; }
    }

    public class SecurityArea
    {
        public int IdArea { get; set; }
        public string Guid { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public virtual ICollection<Permission> Permisos { get; set; }
    }

    public class Permission
    {
        public long IdPermiso { get; set; }
        public string Guid { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int? IdArea { get; set; }
        public virtual SecurityArea AreaSeguridad { get; set; }
    }

    public class RolePermission
    {
    }
}