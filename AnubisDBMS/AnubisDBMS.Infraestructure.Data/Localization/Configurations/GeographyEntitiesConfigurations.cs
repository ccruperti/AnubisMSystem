using System.Data.Entity.ModelConfiguration;
using AnubisDBMS.Data.Localization.Entities;
using AnubisDBMS.Infraestructure.Data.Helpers;
using AnubisDBMS.Infraestructure.Data.Extensions;

namespace AnubisDBMS.Infraestructure.Data.Localization.Configurations
{
    public class ContinenteConfiguration : EntityTypeConfiguration<Continente>
    {
        public ContinenteConfiguration()
        {
            ToTable("Continentes", Schemas.General);

            HasKey(c => c.IdContinente);

            Property(c => c.Codigo)
                .HasColumnType(ColumnTypes.Varchar)
                .HasMaxLength(2)
                .IsRequired()
                .IsUnique();

            Property(c => c.Nombre)
                .HasColumnType(ColumnTypes.Varchar)
                .HasMaxLength(200)
                .IsRequired();
        }
    }

    public class IdiomaConfiguration : EntityTypeConfiguration<Idioma>
    {
        public IdiomaConfiguration()
        {
            ToTable("Idiomas", Schemas.General);

            HasKey(c => c.IdIdioma);

            Property(c => c.Codigo)
                .HasColumnType(ColumnTypes.Varchar)
                .HasMaxLength(10)
                .IsRequired()
                .IsUnique();
        }
    }

    public class PaisConfiguration : EntityTypeConfiguration<Pais>
    {
        public PaisConfiguration()
        {
            ToTable("Paises", Schemas.General);

            HasKey(c => c.IdPais);

            Property(c => c.Codigo)
                .IsRequired()
                .IsUnique();

            Property(c => c.NombreES)
                .HasColumnType(ColumnTypes.Varchar)
                .HasMaxLength(300)
                .IsRequired();

            Property(c => c.NombreEN)
                .HasColumnType(ColumnTypes.Varchar)
                .HasMaxLength(300);

            Property(c => c.NombreDE)
                .HasColumnType(ColumnTypes.Varchar)
                .HasMaxLength(300);

            Property(c => c.NombreCS)
                .HasColumnType(ColumnTypes.Varchar)
                .HasMaxLength(300);

            Property(c => c.NombreFR)
                .HasColumnType(ColumnTypes.Varchar)
                .HasMaxLength(300);

            Property(c => c.NombreIT)
                .HasColumnType(ColumnTypes.Varchar)
                .HasMaxLength(300);

            Property(c => c.NombreNL)
                .HasColumnType(ColumnTypes.Varchar)
                .HasMaxLength(300);

            Property(c => c.CodigoTelefonico)
                .HasColumnType(ColumnTypes.Varchar)
                .HasMaxLength(10);

            Property(c => c.DenominacionEstado)
                .HasColumnType(ColumnTypes.Varchar)
                .HasMaxLength(150);

            HasOptional(c => c.Idioma)
                .WithMany(c => c.Paises)
                .HasForeignKey(c => c.IdIdioma);

            HasOptional(c => c.Continente)
                .WithMany(c => c.Paises)
                .HasForeignKey(c => c.IdContinente);
        }
    }

    public class RegionConfiguration : EntityTypeConfiguration<Region>
    {
        public RegionConfiguration()
        {
            ToTable("Regiones", Schemas.General);

            HasKey(c => c.IdRegion);

            Property(c => c.Codigo)
                .HasColumnType(ColumnTypes.Varchar)
                .HasMaxLength(10)
                .IsRequired()
                .IsUnique();

            Property(c => c.Nombre)
                .HasColumnType(ColumnTypes.Varchar)
                .HasMaxLength(200)
                .IsRequired();

            HasRequired(c => c.Pais)
                .WithMany(c => c.Regiones)
                .HasForeignKey(c => c.IdPais);
        }
    }

    public class EstadoConfiguration : EntityTypeConfiguration<Estado>
    {
        public EstadoConfiguration()
        {
            ToTable("Estados", Schemas.General);

            HasKey(c => c.IdEstado);

            Property(c => c.Codigo)
                .HasColumnType(ColumnTypes.Varchar)
                .HasMaxLength(10)
                .IsRequired()
                .IsUnique();

            Property(c => c.Nombre)
                .HasColumnType(ColumnTypes.Varchar)
                .HasMaxLength(200)
                .IsRequired();

            HasOptional(c => c.Region)
                .WithMany(c => c.Estados)
                .HasForeignKey(c => c.IdRegion);

            HasRequired(c => c.Pais)
                .WithMany(c => c.Estados)
                .HasForeignKey(c => c.IdPais);
        }
    }

    public class CiudadConfiguration : EntityTypeConfiguration<Ciudad>
    {
        public CiudadConfiguration()
        {
            ToTable("Ciudades", Schemas.General);

            HasKey(c => c.IdCiudad);

            HasRequired(c => c.Estado)
                .WithMany(c => c.Ciudades)
                .HasForeignKey(c => c.IdEstado);
        }
    }
}