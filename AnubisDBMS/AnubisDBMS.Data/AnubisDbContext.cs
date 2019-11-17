using AnubisDBMS.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Web;
using AnubisDBMS.Data.Configurations;


namespace AnubisDBMS.Data
{
   public class AnubisDbContext: DbContext
    {

        public DbSet<Equipo> Equipos { get; set; }
        public DbSet<EquipoSensor> EquipoSensor { get; set; }
        public DbSet<Tecnicos> Tecnicos { get; set; }
        public DbSet<Mantenimiento> Mantenimiento { get; set; }
        public DbSet<TipoSensor> TipoSensor { get; set; }
        public DbSet<Frecuencia> Frecuencia { get; set; }
        public DbSet<Estados> Estados { get; set; }
        public DbSet<Sensor> Sensores { get; set; }

        public AnubisDbContext() : base("DefaultConnection")
        {
        }

        //public DbSet<LaunchEntry> Launches { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new EquiposConfiguration());
            modelBuilder.Configurations.Add(new EquipoSensorConfiguration());
            modelBuilder.Configurations.Add(new TecnicosConfiguration());
            modelBuilder.Configurations.Add(new FrecuenciaConfiguration());
            modelBuilder.Configurations.Add(new EstadosConfiguration());
            modelBuilder.Configurations.Add(new MantenimientoConfiguration());
            modelBuilder.Configurations.Add(new TipoSensorConfiguration());

            modelBuilder.Configurations.Add(new SensorConfiguration());

            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}