using System.Data.Entity.ModelConfiguration;
using AnubisDBMS.Infraestructure.Data.Security.Entities;
using AnubisDBMS.Infraestructure.Data.Helpers;

namespace AnubisDBMS.Infraestructure.Data.Security.Configurations
{
    public class PermissionConfiguration : EntityTypeConfiguration<Permission>
    {
        public PermissionConfiguration()
        {
            ToTable("Permisos", Schemas.Seguridad);

            HasKey(c => c.IdPermiso);

            HasRequired(c => c.AreaSeguridad)
                .WithMany(c => c.Permisos)
                .HasForeignKey(c => c.IdArea);
        }
    }

    public class SecurityAreaConfiguration : EntityTypeConfiguration<SecurityArea>
    {
        public SecurityAreaConfiguration()
        {
            ToTable("SeguridadAreas", Schemas.Seguridad);

            HasKey(c => c.IdArea);
        }
    }

    public class SecurityLevelConfiguration : EntityTypeConfiguration<SecurityLevel>
    {
        public SecurityLevelConfiguration()
        {
            ToTable("SeguridadNiveles", Schemas.Seguridad);

            HasKey(c => c.IdNivel);
        }
    }
}