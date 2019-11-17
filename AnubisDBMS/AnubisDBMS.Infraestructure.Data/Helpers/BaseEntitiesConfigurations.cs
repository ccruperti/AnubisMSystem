using System.Data.Entity.ModelConfiguration;

namespace AnubisDBMS.Infraestructure.Data.Helpers
{
    public class CrudEntitiesConfigurations<TEntityType> : EntityTypeConfiguration<TEntityType> where TEntityType : CrudEntities
    {
        public CrudEntitiesConfigurations()
        {
            Property(c => c.FechaModificacion)
                .IsOptional();

            Property(c => c.FechaRegistro)
                .IsRequired();

            Property(c => c.UsuarioRegistro)
                .HasColumnType(ColumnTypes.Varchar)
                .HasMaxLength(350)
                .IsRequired();

            Property(c => c.UsuarioModificacion)
                .HasColumnType(ColumnTypes.Varchar)
                .HasMaxLength(350)
                .IsOptional();

            Property(c => c.UsuarioEliminacion)
                .HasColumnType(ColumnTypes.Varchar)
                .HasMaxLength(350)
                .IsOptional();

            //Property(c => c.IpRegistro)
            //    .HasColumnType(ColumnTypes.Varchar)
            //    .HasMaxLength(25)
            //    .IsOptional();

            //Property(c => c.IpModificacion)
            //    .HasColumnType(ColumnTypes.Varchar)
            //    .HasMaxLength(25)
            //    .IsOptional();

            //Property(c => c.IpEliminacion)
            //    .HasColumnType(ColumnTypes.Varchar)
            //    .HasMaxLength(25)
            //    .IsOptional();

            //Property(c => c.SesionRegistro)
            //    .HasColumnType(ColumnTypes.Varchar)
            //    .HasMaxLength(25)
            //    .IsOptional();

            //Property(c => c.SesionModificacion)
            //    .HasColumnType(ColumnTypes.Varchar)
            //    .HasMaxLength(25)
            //    .IsOptional();

            //Property(c => c.SesionEliminacion)
            //    .HasColumnType(ColumnTypes.Varchar)
            //    .HasMaxLength(25)
            //    .IsOptional();
        }
    }
}