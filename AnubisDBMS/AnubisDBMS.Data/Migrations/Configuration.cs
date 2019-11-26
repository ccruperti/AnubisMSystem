namespace AnubisDBMS.Data.Migrations
{
    using AnubisDBMS.Data.Entities; 
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<AnubisDBMS.Data.AnubisDbContext>
    {
        private AnubisDbContext db = new AnubisDbContext();
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AnubisDBMS.Data.AnubisDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
            if (!db.Estados.Any(c => c.Activo))
            {
                List<Estados> EstadosList = new List<Estados>();
                EstadosList.Add(new Estados
                {
                    NombreEstado = "Inspeccionado",
                    EstiloCss = "positive",
                    Activo = true,
                    FechaRegistro = DateTime.Now,
                    TipoEstado = "Monitoreo",
                    UsuarioRegistro = "System"
                });
                EstadosList.Add(new Estados
                {
                    NombreEstado = "Pendiente",
                    EstiloCss = "negative",
                    Activo = true,
                    FechaRegistro = DateTime.Now,
                    TipoEstado = "Monitoreo",
                    UsuarioRegistro = "System"
                });
                EstadosList.Add(new Estados
                {
                    NombreEstado = "Sin Servicio",
                    EstiloCss = "negative",
                    Activo = true,
                    FechaRegistro = DateTime.Now,
                    TipoEstado = "Monitoreo",
                    UsuarioRegistro = "System"
                });
                db.Estados.AddRange(EstadosList);
                db.SaveChanges();

            }


            if (!db.Frecuencia.Any(c => c.Activo)) {
                List<Frecuencia> FrecuenciaList = new List<Frecuencia>();

                FrecuenciaList.Add(new Frecuencia
                {
                    NombreFrecuencia = "Anual",
                    DiasFrecuencia = 365,
                    Activo = true,
                    FechaRegistro = DateTime.Now,
                    UsuarioRegistro = "System"
                });
                FrecuenciaList.Add(new Frecuencia
                {
                    NombreFrecuencia = "Mensual",
                    DiasFrecuencia = 31,
                    Activo = true,
                    FechaRegistro = DateTime.Now,
                    UsuarioRegistro = "System"
                });
                FrecuenciaList.Add(new Frecuencia
                {
                    NombreFrecuencia = "Quincenal",
                    DiasFrecuencia = 15,
                    Activo = true,
                    FechaRegistro = DateTime.Now,
                    UsuarioRegistro = "System"
                });

                db.Frecuencia.AddRange(FrecuenciaList);
                db.SaveChanges();
            }
        
            if (!db.Servicio.Any())
            {
                db.Servicio.Add(new Servicio
                {
                    Activo = true,
                    FechaRegistro = DateTime.Now,
                    EstadoServicio = true,
                    UsuarioRegistro = "System"
                });
            }
       
            db.SaveChanges();

        }
    }
}
