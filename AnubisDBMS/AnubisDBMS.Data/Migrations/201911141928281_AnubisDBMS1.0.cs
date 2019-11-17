﻿namespace AnubisDBMS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AnubisDBMS10 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "CAT.Equipos",
                c => new
                    {
                        IdEquipo = c.Long(nullable: false, identity: true),
                        Alias = c.String(),
                        SerieEquipo = c.String(),
                        CodigoQR = c.String(),
                        IdUsuario = c.String(),
                        Activo = c.Boolean(nullable: false),
                        FechaRegistro = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(),
                        FechaEliminacion = c.DateTime(),
                        UsuarioRegistro = c.String(),
                        UsuarioModificacion = c.String(),
                        UsuarioEliminacion = c.String(),
                    })
                .PrimaryKey(t => t.IdEquipo);
            
            CreateTable(
                "MON.EquipoSensores",
                c => new
                    {
                        IdEquipoSensor = c.Long(nullable: false, identity: true),
                        IdEquipo = c.Long(nullable: false),
                        IdSensor = c.Long(nullable: false),
                        NumeroPuerto = c.Int(nullable: false),
                        Activo = c.Boolean(nullable: false),
                        FechaRegistro = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(),
                        FechaEliminacion = c.DateTime(),
                        UsuarioRegistro = c.String(),
                        UsuarioModificacion = c.String(),
                        UsuarioEliminacion = c.String(),
                    })
                .PrimaryKey(t => t.IdEquipoSensor)
                .ForeignKey("CAT.Equipos", t => t.IdEquipo)
                .ForeignKey("CAT.Sensores", t => t.IdSensor)
                .Index(t => t.IdEquipo)
                .Index(t => t.IdSensor);
            
            CreateTable(
                "CAT.Sensores",
                c => new
                    {
                        IdSensor = c.Long(nullable: false, identity: true),
                        IdTipoSensor = c.Long(nullable: false),
                        FechaConsulta = c.DateTime(nullable: false),
                        SerieSensor = c.String(),
                        Activo = c.Boolean(nullable: false),
                        FechaRegistro = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(),
                        FechaEliminacion = c.DateTime(),
                        UsuarioRegistro = c.String(),
                        UsuarioModificacion = c.String(),
                        UsuarioEliminacion = c.String(),
                    })
                .PrimaryKey(t => t.IdSensor)
                .ForeignKey("CAT.TipoSensores", t => t.IdTipoSensor, cascadeDelete: true)
                .Index(t => t.IdTipoSensor);
            
            CreateTable(
                "CAT.TipoSensores",
                c => new
                    {
                        IdTipoSensor = c.Long(nullable: false, identity: true),
                        NombreTipoSensor = c.String(),
                        UnidadSensor = c.String(),
                        Activo = c.Boolean(nullable: false),
                        FechaRegistro = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(),
                        FechaEliminacion = c.DateTime(),
                        UsuarioRegistro = c.String(),
                        UsuarioModificacion = c.String(),
                        UsuarioEliminacion = c.String(),
                    })
                .PrimaryKey(t => t.IdTipoSensor);
            
            CreateTable(
                "CAT.Estados",
                c => new
                    {
                        IdEstado = c.Long(nullable: false, identity: true),
                        NombreEstado = c.String(),
                        EstiloCss = c.String(),
                        TipoEstado = c.String(),
                        Activo = c.Boolean(nullable: false),
                        FechaRegistro = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(),
                        FechaEliminacion = c.DateTime(),
                        UsuarioRegistro = c.String(),
                        UsuarioModificacion = c.String(),
                        UsuarioEliminacion = c.String(),
                    })
                .PrimaryKey(t => t.IdEstado);
            
            CreateTable(
                "CAT.Frecuencias",
                c => new
                    {
                        IdFrecuencia = c.Long(nullable: false, identity: true),
                        NombreFrecuencia = c.String(),
                        DiasFrecuencia = c.Int(nullable: false),
                        Activo = c.Boolean(nullable: false),
                        FechaRegistro = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(),
                        FechaEliminacion = c.DateTime(),
                        UsuarioRegistro = c.String(),
                        UsuarioModificacion = c.String(),
                        UsuarioEliminacion = c.String(),
                    })
                .PrimaryKey(t => t.IdFrecuencia);
            
            CreateTable(
                "MON.Mantenimientos",
                c => new
                    {
                        IdManteniemiento = c.Long(nullable: false, identity: true),
                        IdEquipoSensor = c.Long(nullable: false),
                        IdEstado = c.Long(nullable: false),
                        IdTecnico = c.Long(nullable: false),
                        IdUsuario = c.String(),
                        IdFrecuencia = c.Long(nullable: false),
                        Notas = c.String(),
                        ServicioActivo = c.Boolean(nullable: false),
                        Notificiaciones = c.Boolean(nullable: false),
                        Descripcion = c.String(),
                        Activo = c.Boolean(nullable: false),
                        FechaRegistro = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(),
                        FechaEliminacion = c.DateTime(),
                        UsuarioRegistro = c.String(),
                        UsuarioModificacion = c.String(),
                        UsuarioEliminacion = c.String(),
                    })
                .PrimaryKey(t => t.IdManteniemiento)
                .ForeignKey("MON.EquipoSensores", t => t.IdEquipoSensor)
                .ForeignKey("CAT.Estados", t => t.IdEstado)
                .ForeignKey("CAT.Frecuencias", t => t.IdFrecuencia)
                .ForeignKey("CAT.Tecnicos", t => t.IdTecnico)
                .Index(t => t.IdEquipoSensor)
                .Index(t => t.IdEstado)
                .Index(t => t.IdTecnico)
                .Index(t => t.IdFrecuencia);
            
            CreateTable(
                "CAT.Tecnicos",
                c => new
                    {
                        IdTecnico = c.Long(nullable: false, identity: true),
                        NombreTecnico = c.String(),
                        CelularTecnico = c.String(),
                        CorreoTecnico = c.String(),
                        Cedula = c.String(),
                        Activo = c.Boolean(nullable: false),
                        FechaRegistro = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(),
                        FechaEliminacion = c.DateTime(),
                        UsuarioRegistro = c.String(),
                        UsuarioModificacion = c.String(),
                        UsuarioEliminacion = c.String(),
                    })
                .PrimaryKey(t => t.IdTecnico);
            
        }
        
        public override void Down()
        {
            DropForeignKey("MON.Mantenimientos", "IdTecnico", "CAT.Tecnicos");
            DropForeignKey("MON.Mantenimientos", "IdFrecuencia", "CAT.Frecuencias");
            DropForeignKey("MON.Mantenimientos", "IdEstado", "CAT.Estados");
            DropForeignKey("MON.Mantenimientos", "IdEquipoSensor", "MON.EquipoSensores");
            DropForeignKey("MON.EquipoSensores", "IdSensor", "CAT.Sensores");
            DropForeignKey("CAT.Sensores", "IdTipoSensor", "CAT.TipoSensores");
            DropForeignKey("MON.EquipoSensores", "IdEquipo", "CAT.Equipos");
            DropIndex("MON.Mantenimientos", new[] { "IdFrecuencia" });
            DropIndex("MON.Mantenimientos", new[] { "IdTecnico" });
            DropIndex("MON.Mantenimientos", new[] { "IdEstado" });
            DropIndex("MON.Mantenimientos", new[] { "IdEquipoSensor" });
            DropIndex("CAT.Sensores", new[] { "IdTipoSensor" });
            DropIndex("MON.EquipoSensores", new[] { "IdSensor" });
            DropIndex("MON.EquipoSensores", new[] { "IdEquipo" });
            DropTable("CAT.Tecnicos");
            DropTable("MON.Mantenimientos");
            DropTable("CAT.Frecuencias");
            DropTable("CAT.Estados");
            DropTable("CAT.TipoSensores");
            DropTable("CAT.Sensores");
            DropTable("MON.EquipoSensores");
            DropTable("CAT.Equipos");
        }
    }
}
