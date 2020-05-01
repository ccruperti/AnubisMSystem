namespace AnubisDBMS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Anubis10 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "SEG.SeguridadAreas",
                c => new
                    {
                        IdArea = c.Int(nullable: false, identity: true),
                        Guid = c.String(),
                        Nombre = c.String(),
                        Descripcion = c.String(),
                        SecurityLevel_IdNivel = c.Int(),
                    })
                .PrimaryKey(t => t.IdArea)
                .ForeignKey("SEG.SeguridadNiveles", t => t.SecurityLevel_IdNivel)
                .Index(t => t.SecurityLevel_IdNivel);
            
            CreateTable(
                "SEG.Permisos",
                c => new
                    {
                        IdPermiso = c.Long(nullable: false, identity: true),
                        Guid = c.String(),
                        Nombre = c.String(),
                        Descripcion = c.String(),
                        IdArea = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdPermiso)
                .ForeignKey("SEG.SeguridadAreas", t => t.IdArea, cascadeDelete: true)
                .Index(t => t.IdArea);
            
            CreateTable(
                "MON.DataSensores",
                c => new
                    {
                        IdDataSensor = c.Long(nullable: false, identity: true),
                        SerieSensor = c.String(),
                        TipoSensor = c.String(),
                        Medida = c.Double(nullable: false),
                        UnidadMedida = c.String(),
                        Chequeado = c.Boolean(nullable: false),
                        Error = c.Boolean(nullable: false),
                        EncimaNormal = c.Boolean(nullable: false),
                        DebajoNormal = c.Boolean(nullable: false),
                        AlertaRecibida = c.Boolean(nullable: false),
                        Notificado = c.Boolean(nullable: false),
                        IdEmpresa = c.Long(),
                        Activo = c.Boolean(nullable: false),
                        FechaRegistro = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(),
                        FechaEliminacion = c.DateTime(),
                        UsuarioRegistro = c.String(),
                        UsuarioModificacion = c.String(),
                        UsuarioEliminacion = c.String(),
                    })
                .PrimaryKey(t => t.IdDataSensor)
                .ForeignKey("GEN.Empresas", t => t.IdEmpresa)
                .Index(t => t.IdEmpresa);
            
            CreateTable(
                "GEN.Empresas",
                c => new
                    {
                        IdEmpresa = c.Long(nullable: false, identity: true),
                        Nombre = c.String(),
                        RUC = c.String(),
                        RazonSocial = c.String(),
                        ServicioActivo = c.Boolean(nullable: false),
                        EmailNotificacion = c.String(),
                        PrimeraNotificacion = c.Int(nullable: false),
                        SegundaNotificacion = c.Int(nullable: false),
                        TerceraNotificacion = c.Int(nullable: false),
                        Activo = c.Boolean(nullable: false),
                        FechaRegistro = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(),
                        FechaEliminacion = c.DateTime(),
                        UsuarioRegistro = c.String(),
                        UsuarioModificacion = c.String(),
                        UsuarioEliminacion = c.String(),
                    })
                .PrimaryKey(t => t.IdEmpresa);
            
            CreateTable(
                "CAT.Equipos",
                c => new
                    {
                        IdEquipo = c.Long(nullable: false, identity: true),
                        Alias = c.String(),
                        SerieEquipo = c.String(),
                        CodigoQR = c.String(),
                        IdUsuario = c.String(),
                        AplicaMonitoreo = c.Boolean(nullable: false),
                        IdEmpresa = c.Long(),
                        Activo = c.Boolean(nullable: false),
                        FechaRegistro = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(),
                        FechaEliminacion = c.DateTime(),
                        UsuarioRegistro = c.String(),
                        UsuarioModificacion = c.String(),
                        UsuarioEliminacion = c.String(),
                    })
                .PrimaryKey(t => t.IdEquipo)
                .ForeignKey("GEN.Empresas", t => t.IdEmpresa)
                .Index(t => t.IdEmpresa);
            
            CreateTable(
                "MON.EquipoSensores",
                c => new
                    {
                        IdEquipoSensor = c.Long(nullable: false, identity: true),
                        IdEquipo = c.Long(),
                        IdSensor = c.Long(),
                        NumeroPuerto = c.Int(nullable: false),
                        IdEmpresa = c.Long(),
                        Activo = c.Boolean(nullable: false),
                        FechaRegistro = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(),
                        FechaEliminacion = c.DateTime(),
                        UsuarioRegistro = c.String(),
                        UsuarioModificacion = c.String(),
                        UsuarioEliminacion = c.String(),
                    })
                .PrimaryKey(t => t.IdEquipoSensor)
                .ForeignKey("GEN.Empresas", t => t.IdEmpresa)
                .ForeignKey("CAT.Equipos", t => t.IdEquipo)
                .ForeignKey("CAT.Sensores", t => t.IdSensor)
                .Index(t => t.IdEquipo)
                .Index(t => t.IdSensor)
                .Index(t => t.IdEmpresa);
            
            CreateTable(
                "CAT.Sensores",
                c => new
                    {
                        IdSensor = c.Long(nullable: false, identity: true),
                        IdTipoSensor = c.Long(nullable: false),
                        FechaConsulta = c.DateTime(),
                        SerieSensor = c.String(),
                        Max = c.Double(),
                        Min = c.Double(),
                        IdEmpresa = c.Long(),
                        Activo = c.Boolean(nullable: false),
                        FechaRegistro = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(),
                        FechaEliminacion = c.DateTime(),
                        UsuarioRegistro = c.String(),
                        UsuarioModificacion = c.String(),
                        UsuarioEliminacion = c.String(),
                    })
                .PrimaryKey(t => t.IdSensor)
                .ForeignKey("GEN.Empresas", t => t.IdEmpresa)
                .ForeignKey("CAT.TipoSensores", t => t.IdTipoSensor, cascadeDelete: true)
                .Index(t => t.IdTipoSensor)
                .Index(t => t.IdEmpresa);
            
            CreateTable(
                "CAT.TipoSensores",
                c => new
                    {
                        IdTipoSensor = c.Long(nullable: false, identity: true),
                        NombreTipoSensor = c.String(),
                        Min_TipoSensor = c.Double(nullable: false),
                        Max_TipoSensor = c.Double(nullable: false),
                        UnidadSensor = c.String(),
                        IdEmpresa = c.Long(),
                        Activo = c.Boolean(nullable: false),
                        FechaRegistro = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(),
                        FechaEliminacion = c.DateTime(),
                        UsuarioRegistro = c.String(),
                        UsuarioModificacion = c.String(),
                        UsuarioEliminacion = c.String(),
                    })
                .PrimaryKey(t => t.IdTipoSensor)
                .ForeignKey("GEN.Empresas", t => t.IdEmpresa)
                .Index(t => t.IdEmpresa);
            
            CreateTable(
                "CAT.Estado",
                c => new
                    {
                        IdEstado = c.Long(nullable: false, identity: true),
                        NombreEstado = c.String(),
                        EstiloCss = c.String(),
                        TipoEstado = c.String(),
                        IdEmpresa = c.Long(),
                        Activo = c.Boolean(nullable: false),
                        FechaRegistro = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(),
                        FechaEliminacion = c.DateTime(),
                        UsuarioRegistro = c.String(),
                        UsuarioModificacion = c.String(),
                        UsuarioEliminacion = c.String(),
                    })
                .PrimaryKey(t => t.IdEstado)
                .ForeignKey("GEN.Empresas", t => t.IdEmpresa)
                .Index(t => t.IdEmpresa);
            
            CreateTable(
                "CAT.Frecuencias",
                c => new
                    {
                        IdFrecuencia = c.Long(nullable: false, identity: true),
                        NombreFrecuencia = c.String(),
                        DiasFrecuencia = c.Int(nullable: false),
                        IdEmpresa = c.Long(),
                        Activo = c.Boolean(nullable: false),
                        FechaRegistro = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(),
                        FechaEliminacion = c.DateTime(),
                        UsuarioRegistro = c.String(),
                        UsuarioModificacion = c.String(),
                        UsuarioEliminacion = c.String(),
                    })
                .PrimaryKey(t => t.IdFrecuencia)
                .ForeignKey("GEN.Empresas", t => t.IdEmpresa)
                .Index(t => t.IdEmpresa);
            
            CreateTable(
                "MON.Mantenimientos",
                c => new
                    {
                        IdManteniemiento = c.Long(nullable: false, identity: true),
                        IdEquipo = c.Long(nullable: false),
                        IdEstado = c.Long(nullable: false),
                        IdTecnico = c.Long(nullable: false),
                        IdUsuario = c.String(),
                        IdFrecuencia = c.Long(nullable: false),
                        Notas = c.String(),
                        ServicioActivo = c.Boolean(nullable: false),
                        Notificiaciones = c.Boolean(nullable: false),
                        Descripcion = c.String(),
                        FechaMantenimiento = c.DateTime(nullable: false),
                        FechaFinMantenimiento = c.DateTime(),
                        IdEmpresa = c.Long(),
                        Activo = c.Boolean(nullable: false),
                        FechaRegistro = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(),
                        FechaEliminacion = c.DateTime(),
                        UsuarioRegistro = c.String(),
                        UsuarioModificacion = c.String(),
                        UsuarioEliminacion = c.String(),
                    })
                .PrimaryKey(t => t.IdManteniemiento)
                .ForeignKey("GEN.Empresas", t => t.IdEmpresa)
                .ForeignKey("CAT.Equipos", t => t.IdEquipo)
                .ForeignKey("CAT.Estado", t => t.IdEstado)
                .ForeignKey("CAT.Frecuencias", t => t.IdFrecuencia)
                .ForeignKey("CAT.Tecnicos", t => t.IdTecnico)
                .Index(t => t.IdEquipo)
                .Index(t => t.IdEstado)
                .Index(t => t.IdTecnico)
                .Index(t => t.IdFrecuencia)
                .Index(t => t.IdEmpresa);
            
            CreateTable(
                "CAT.Tecnicos",
                c => new
                    {
                        IdTecnico = c.Long(nullable: false, identity: true),
                        NombreTecnico = c.String(),
                        CelularTecnico = c.String(),
                        CorreoTecnico = c.String(),
                        Cedula = c.String(),
                        IdEmpresa = c.Long(),
                        Activo = c.Boolean(nullable: false),
                        FechaRegistro = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(),
                        FechaEliminacion = c.DateTime(),
                        UsuarioRegistro = c.String(),
                        UsuarioModificacion = c.String(),
                        UsuarioEliminacion = c.String(),
                    })
                .PrimaryKey(t => t.IdTecnico)
                .ForeignKey("GEN.Empresas", t => t.IdEmpresa)
                .Index(t => t.IdEmpresa);
            
            CreateTable(
                "SEG.SeguridadNiveles",
                c => new
                    {
                        IdNivel = c.Int(nullable: false, identity: true),
                        Guid = c.String(),
                        Nombre = c.String(),
                        Descripcion = c.String(),
                    })
                .PrimaryKey(t => t.IdNivel);
            
            CreateTable(
                "SEG.Roles",
                c => new
                    {
                        IdRol = c.Long(nullable: false, identity: true),
                        Plural = c.String(),
                        Descripcion = c.String(),
                        Sistema = c.Boolean(nullable: false),
                        Defecto = c.Boolean(nullable: false),
                        Activo = c.Boolean(nullable: false),
                        Prioridad = c.Int(nullable: false),
                        Orden = c.Int(nullable: false),
                        FechaRegistro = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(),
                        FechaEliminacion = c.DateTime(),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.IdRol)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "SEG.RolesUsuarios",
                c => new
                    {
                        IdUsuario = c.Long(nullable: false),
                        IdRol = c.Long(nullable: false),
                        Activo = c.Boolean(nullable: false),
                        Primario = c.Boolean(nullable: false),
                        FechaRegistro = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(),
                        FechaEliminacion = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.IdUsuario, t.IdRol })
                .ForeignKey("SEG.Roles", t => t.IdRol, cascadeDelete: true)
                .ForeignKey("SEG.Usuarios", t => t.IdUsuario, cascadeDelete: true)
                .Index(t => t.IdUsuario)
                .Index(t => t.IdRol);
            
            CreateTable(
                "MON.Servicio",
                c => new
                    {
                        IdServicio = c.Long(nullable: false, identity: true),
                        EstadoServicio = c.Boolean(nullable: false),
                        IdEmpresa = c.Long(),
                        Activo = c.Boolean(nullable: false),
                        FechaRegistro = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(),
                        FechaEliminacion = c.DateTime(),
                        UsuarioRegistro = c.String(),
                        UsuarioModificacion = c.String(),
                        UsuarioEliminacion = c.String(),
                    })
                .PrimaryKey(t => t.IdServicio)
                .ForeignKey("GEN.Empresas", t => t.IdEmpresa)
                .Index(t => t.IdEmpresa);
            
            CreateTable(
                "SEG.Usuarios",
                c => new
                    {
                        IdUsuario = c.Long(nullable: false, identity: true),
                        Nombres = c.String(),
                        Apellidos = c.String(),
                        TipoUsuario = c.String(),
                        Cedula = c.String(),
                        Celular = c.String(),
                        Activo = c.Boolean(nullable: false),
                        IdEmpresa = c.Long(),
                        FechaRegistro = c.DateTime(nullable: false),
                        FechaActivacion = c.DateTime(),
                        FechaDesactivacion = c.DateTime(),
                        FechaExpiracion = c.DateTime(),
                        FechaModificacion = c.DateTime(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.IdUsuario)
                .ForeignKey("GEN.Empresas", t => t.IdEmpresa)
                .Index(t => t.IdEmpresa)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "SEG.ClaimsUsuarios",
                c => new
                    {
                        IdClaim = c.Int(nullable: false, identity: true),
                        IdUsuario = c.Long(nullable: false),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.IdClaim)
                .ForeignKey("SEG.Usuarios", t => t.IdUsuario, cascadeDelete: true)
                .Index(t => t.IdUsuario);
            
            CreateTable(
                "SEG.LoginsUsuarios",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        IdUsuario = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.IdUsuario })
                .ForeignKey("SEG.Usuarios", t => t.IdUsuario, cascadeDelete: true)
                .Index(t => t.IdUsuario);
            
        }
        
        public override void Down()
        {
            DropForeignKey("SEG.RolesUsuarios", "IdUsuario", "SEG.Usuarios");
            DropForeignKey("SEG.LoginsUsuarios", "IdUsuario", "SEG.Usuarios");
            DropForeignKey("SEG.Usuarios", "IdEmpresa", "GEN.Empresas");
            DropForeignKey("SEG.ClaimsUsuarios", "IdUsuario", "SEG.Usuarios");
            DropForeignKey("MON.Servicio", "IdEmpresa", "GEN.Empresas");
            DropForeignKey("SEG.RolesUsuarios", "IdRol", "SEG.Roles");
            DropForeignKey("SEG.SeguridadAreas", "SecurityLevel_IdNivel", "SEG.SeguridadNiveles");
            DropForeignKey("MON.Mantenimientos", "IdTecnico", "CAT.Tecnicos");
            DropForeignKey("CAT.Tecnicos", "IdEmpresa", "GEN.Empresas");
            DropForeignKey("MON.Mantenimientos", "IdFrecuencia", "CAT.Frecuencias");
            DropForeignKey("MON.Mantenimientos", "IdEstado", "CAT.Estado");
            DropForeignKey("MON.Mantenimientos", "IdEquipo", "CAT.Equipos");
            DropForeignKey("MON.Mantenimientos", "IdEmpresa", "GEN.Empresas");
            DropForeignKey("CAT.Frecuencias", "IdEmpresa", "GEN.Empresas");
            DropForeignKey("CAT.Estado", "IdEmpresa", "GEN.Empresas");
            DropForeignKey("MON.EquipoSensores", "IdSensor", "CAT.Sensores");
            DropForeignKey("CAT.Sensores", "IdTipoSensor", "CAT.TipoSensores");
            DropForeignKey("CAT.TipoSensores", "IdEmpresa", "GEN.Empresas");
            DropForeignKey("CAT.Sensores", "IdEmpresa", "GEN.Empresas");
            DropForeignKey("MON.EquipoSensores", "IdEquipo", "CAT.Equipos");
            DropForeignKey("MON.EquipoSensores", "IdEmpresa", "GEN.Empresas");
            DropForeignKey("CAT.Equipos", "IdEmpresa", "GEN.Empresas");
            DropForeignKey("MON.DataSensores", "IdEmpresa", "GEN.Empresas");
            DropForeignKey("SEG.Permisos", "IdArea", "SEG.SeguridadAreas");
            DropIndex("SEG.LoginsUsuarios", new[] { "IdUsuario" });
            DropIndex("SEG.ClaimsUsuarios", new[] { "IdUsuario" });
            DropIndex("SEG.Usuarios", "UserNameIndex");
            DropIndex("SEG.Usuarios", new[] { "IdEmpresa" });
            DropIndex("MON.Servicio", new[] { "IdEmpresa" });
            DropIndex("SEG.RolesUsuarios", new[] { "IdRol" });
            DropIndex("SEG.RolesUsuarios", new[] { "IdUsuario" });
            DropIndex("SEG.Roles", "RoleNameIndex");
            DropIndex("CAT.Tecnicos", new[] { "IdEmpresa" });
            DropIndex("MON.Mantenimientos", new[] { "IdEmpresa" });
            DropIndex("MON.Mantenimientos", new[] { "IdFrecuencia" });
            DropIndex("MON.Mantenimientos", new[] { "IdTecnico" });
            DropIndex("MON.Mantenimientos", new[] { "IdEstado" });
            DropIndex("MON.Mantenimientos", new[] { "IdEquipo" });
            DropIndex("CAT.Frecuencias", new[] { "IdEmpresa" });
            DropIndex("CAT.Estado", new[] { "IdEmpresa" });
            DropIndex("CAT.TipoSensores", new[] { "IdEmpresa" });
            DropIndex("CAT.Sensores", new[] { "IdEmpresa" });
            DropIndex("CAT.Sensores", new[] { "IdTipoSensor" });
            DropIndex("MON.EquipoSensores", new[] { "IdEmpresa" });
            DropIndex("MON.EquipoSensores", new[] { "IdSensor" });
            DropIndex("MON.EquipoSensores", new[] { "IdEquipo" });
            DropIndex("CAT.Equipos", new[] { "IdEmpresa" });
            DropIndex("MON.DataSensores", new[] { "IdEmpresa" });
            DropIndex("SEG.Permisos", new[] { "IdArea" });
            DropIndex("SEG.SeguridadAreas", new[] { "SecurityLevel_IdNivel" });
            DropTable("SEG.LoginsUsuarios");
            DropTable("SEG.ClaimsUsuarios");
            DropTable("SEG.Usuarios");
            DropTable("MON.Servicio");
            DropTable("SEG.RolesUsuarios");
            DropTable("SEG.Roles");
            DropTable("SEG.SeguridadNiveles");
            DropTable("CAT.Tecnicos");
            DropTable("MON.Mantenimientos");
            DropTable("CAT.Frecuencias");
            DropTable("CAT.Estado");
            DropTable("CAT.TipoSensores");
            DropTable("CAT.Sensores");
            DropTable("MON.EquipoSensores");
            DropTable("CAT.Equipos");
            DropTable("GEN.Empresas");
            DropTable("MON.DataSensores");
            DropTable("SEG.Permisos");
            DropTable("SEG.SeguridadAreas");
        }
    }
}
