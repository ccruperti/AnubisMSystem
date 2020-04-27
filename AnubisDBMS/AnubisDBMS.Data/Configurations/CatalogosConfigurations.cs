using AnubisDBMS.Data.Entities;
using AnubisDBMS.Infraestructure.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnubisDBMS.Data.Configurations
{
    public class EquiposConfiguration : EntityTypeConfiguration<Equipo>
    {
        public EquiposConfiguration()
        {
            ToTable("Equipos", AnubisDBMSSchemas.Catalogo);
            HasKey(c => c.IdEquipo);

            //Property(c => c.Alias)
            //   .HasColumnType(ColumnTypes.Varchar)
            //   .IsOptional();

            //Property(c => c.SerieEquipo)
            //   .HasColumnType(ColumnTypes.Varchar)
            //   .IsOptional();

            //Property(c => c.CodigoQR)
            //   .HasColumnType(ColumnTypes.Varchar)
            //   .IsOptional();

            //HasRequired(x => x.Usuarios)
            //  .WithMany()
            //  .HasForeignKey(x => x.IdUsuario).WillCascadeOnDelete(false);


        }
    }

    public class SensorConfiguration : EntityTypeConfiguration<Sensor>
    {
        public SensorConfiguration()
        {
            ToTable("Sensores", AnubisDBMSSchemas.Catalogo);
            HasKey(c => c.IdSensor);

            //Property(c => c.FechaConsulta)
            //   .HasColumnType(ColumnTypes.Date)
            //   .IsOptional();

            //Property(c => c.SerieSensor)
            //   .HasColumnType(ColumnTypes.Varchar)
            //   .IsOptional();


            //HasRequired(x => x.IdTipoSensor)
            //  .WithMany()
            //  .HasForeignKey(x => x.TipoSensor).WillCascadeOnDelete(false);


            //HasRequired(x => x.)
            //  .WithMany()
            //  .HasForeignKey(x => x.EquiposSensor).WillCascadeOnDelete(false);


        }
    }

    public class EquipoSensorConfiguration : EntityTypeConfiguration<EquipoSensor>
    {
        public EquipoSensorConfiguration()
        {
            ToTable("EquipoSensores", AnubisDBMSSchemas.Monitoreo);
            HasKey(c => c.IdEquipoSensor);

            //Property(c => c.NumeroPuerto)
            //   .HasColumnType(ColumnTypes.Int)
            //   .IsOptional();


            HasOptional(x => x.Equipos)
              .WithMany()
              .HasForeignKey(x => x.IdEquipo).WillCascadeOnDelete(false);

            HasOptional(x => x.Sensores)
              .WithMany()
              .HasForeignKey(x => x.IdSensor).WillCascadeOnDelete(false);

        }
    }


    public class TipoSensorConfiguration : EntityTypeConfiguration<TipoSensor>
    {
        public TipoSensorConfiguration()
        {
            ToTable("TipoSensores", AnubisDBMSSchemas.Catalogo);
            HasKey(c => c.IdTipoSensor);

            //Property(c => c.NombreTipoSensor)
            //   .HasColumnType(ColumnTypes.Varchar)
            //   .IsOptional();

            //Property(c => c.UnidadSensor)
            //  .HasColumnType(ColumnTypes.Varchar)
            //  .IsOptional();


        }
    }

    public class FrecuenciaConfiguration : EntityTypeConfiguration<Frecuencia>
    {
        public FrecuenciaConfiguration()
        {
            ToTable("Frecuencias", AnubisDBMSSchemas.Catalogo);
            HasKey(c => c.IdFrecuencia);

            //Property(c => c.NombreFrecuencia)
            //   .HasColumnType(ColumnTypes.Varchar)
            //   .IsOptional();

            //Property(c => c.DiasFrecuencia)
            //  .HasColumnType(ColumnTypes.Int)
            //  .IsOptional();

        }


 

    }
    public class EstadosConfiguration : EntityTypeConfiguration<Estados>
    {
        public EstadosConfiguration()
        {
            ToTable("Estado", AnubisDBMSSchemas.Catalogo);
            HasKey(c => c.IdEstado);

            //Property(c => c.NombreEstado)
            //   .HasColumnType(ColumnTypes.Varchar)
            //   .IsOptional();

            //Property(c => c.EstiloCss)
            //  .HasColumnType(ColumnTypes.Varchar)
            //  .IsOptional();

            //Property(c => c.TipoEstado)
            //  .HasColumnType(ColumnTypes.Varchar)
            //  .IsOptional();

        }
    }

    public class TecnicosConfiguration : EntityTypeConfiguration<Tecnicos>
    {
        public TecnicosConfiguration()
        {
            ToTable("Tecnicos", AnubisDBMSSchemas.Catalogo);
            HasKey(c => c.IdTecnico);

            //Property(c => c.NombreTecnico)
            //   .HasColumnType(ColumnTypes.Varchar)
            //   .IsOptional();

            //Property(c => c.CelularTecnico)
            //  .HasColumnType(ColumnTypes.Varchar)
            //  .IsOptional();

            //Property(c => c.CorreoTecnico)
            //  .HasColumnType(ColumnTypes.Varchar)
            //  .IsOptional();


            //Property(c => c.Cedula)
            //  .HasColumnType(ColumnTypes.Varchar)
            //  .IsOptional();

        }
    }

    public class ServicioConfiguration : EntityTypeConfiguration<Servicio>
    {
        public ServicioConfiguration()
        {
            ToTable("Servicio", AnubisDBMSSchemas.Monitoreo);
            HasKey(c => c.IdServicio); 
        }
    }


    public class DataSensoresConfiguration : EntityTypeConfiguration<DataSensores>
    {
        public DataSensoresConfiguration()
        {
            ToTable("DataSensores", AnubisDBMSSchemas.Monitoreo);
            HasKey(c => c.IdDataSensor);
           
        }
    }


    public class MantenimientoConfiguration : EntityTypeConfiguration<Mantenimiento>
    {
        public MantenimientoConfiguration()
        {
            ToTable("Mantenimientos", AnubisDBMSSchemas.Monitoreo);
            HasKey(c => c.IdManteniemiento);

            //Property(c => c.Notas)
            //   .HasColumnType(ColumnTypes.Varchar)
            //   .IsOptional();

            //Property(c => c.Descripcion)
            //  .HasColumnType(ColumnTypes.Varchar)
            //  .IsOptional();

            //Property(c => c.ServicioActivo)
            //  .HasColumnType(ColumnTypes.Bool)
            //  .IsRequired();

            //Property(c => c.Notificiaciones)
            //  .HasColumnType(ColumnTypes.Bool)
            //  .IsRequired();

            HasRequired(x => x.Equipo)
              .WithMany()
              .HasForeignKey(x => x.IdEquipo).WillCascadeOnDelete(false);


            HasRequired(x => x.Estados)
              .WithMany()
              .HasForeignKey(x => x.IdEstado).WillCascadeOnDelete(false);



            HasRequired(x => x.Tecnicos)
              .WithMany()
              .HasForeignKey(x => x.IdTecnico).WillCascadeOnDelete(false);

            //HasRequired(x => x.Usuarios)
            //  .WithMany()
            //  .HasForeignKey(x => x.IdUsuario).WillCascadeOnDelete(false);


            HasRequired(x => x.Frecuencias)
              .WithMany()
              .HasForeignKey(x => x.IdFrecuencia).WillCascadeOnDelete(false);


        }
    }


}
