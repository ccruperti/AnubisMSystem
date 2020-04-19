using System.Data.Entity;
using AnubisDBMS.Data.FileManagement.Configurations;
using AnubisDBMS.Data.Localization.Entities;
using AnubisDBMS.Infraestructure.Data.FileManagement.Entities;
using AnubisDBMS.Infraestructure.Data.Helpers;
using AnubisDBMS.Infraestructure.Data.Localization.Configurations;
using AnubisDBMS.Infraestructure.Data.Security.Configurations;
using AnubisDBMS.Infraestructure.Data.Security.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AnubisDBMS.Infraestructura.Data
{
    /// <summary>
    ///     Contexto Base para Aplicaciones 
    /// </summary>
    [DbConfigurationType(typeof(AnubisDBMSDbConfiguration))]
    public class AnubisDBMSDbContext : IdentityDbContext<AnubisDBMSUser, AnubisDBMSUserRole, long, AnubisDBMSLogin,
        AnubisDBMSRole, AnubisDBMSClaim>
    {
        public AnubisDBMSDbContext(string contextName = "DefaultConnection") : base(contextName)
        {
            
        }

        // MODULE: Security
        public virtual DbSet<Permission> Permisos { get; set; }
        public virtual DbSet<SecurityArea> AreasSeguridad { get; set; }
        public virtual DbSet<SecurityLevel> NivelesSeguridad { get; set; }

        //// MODULE: Localization
        //// Geography
        //public virtual DbSet<Continente> Continentes { get; set; }
        //public virtual DbSet<Pais> Paises { get; set; }
        //public virtual DbSet<Region> Regiones { get; set; }
        //public virtual DbSet<Estado> Estados { get; set; }
        //public virtual DbSet<Ciudad> Ciudades { get; set; }

        // MODULE: FileManagement
        //public virtual DbSet<Archivo> Archivos { get; set; }

        //// Localization
        //public virtual DbSet<Idioma> Idiomas { get; set; }

        public static AnubisDBMSDbContext Create()
        {
            return new AnubisDBMSDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AnubisDBMSUserRole>().ToTable("Roles", Schemas.Seguridad).Property(p => p.Id).HasColumnName("IdRol");
            modelBuilder.Entity<AnubisDBMSUser>().ToTable("Usuarios", Schemas.Seguridad).Property(p => p.Id).HasColumnName("IdUsuario");
            modelBuilder.Entity<AnubisDBMSRole>().ToTable("RolesUsuarios", Schemas.Seguridad).Property(p => p.RoleId).HasColumnName("IdRol");
            modelBuilder.Entity<AnubisDBMSRole>().Property(p => p.UserId).HasColumnName("IdUsuario");
            modelBuilder.Entity<AnubisDBMSLogin>().ToTable("LoginsUsuarios", Schemas.Seguridad).Property(p => p.UserId).HasColumnName("IdUsuario");
            modelBuilder.Entity<AnubisDBMSClaim>().ToTable("ClaimsUsuarios", Schemas.Seguridad).Property(p => p.Id).HasColumnName("IdClaim");
            modelBuilder.Entity<AnubisDBMSClaim>().Property(p => p.UserId).HasColumnName("IdUsuario");

            modelBuilder.Configurations.Add(new PermissionConfiguration());
            modelBuilder.Configurations.Add(new SecurityAreaConfiguration());
            modelBuilder.Configurations.Add(new SecurityLevelConfiguration());

            ////Geography
            //modelBuilder.Configurations.Add(new ContinenteConfiguration());
            //modelBuilder.Configurations.Add(new IdiomaConfiguration());
            //modelBuilder.Configurations.Add(new PaisConfiguration());
            //modelBuilder.Configurations.Add(new RegionConfiguration());
            //modelBuilder.Configurations.Add(new EstadoConfiguration());
            //modelBuilder.Configurations.Add(new CiudadConfiguration());

            // FileManagement
        }
    }
}