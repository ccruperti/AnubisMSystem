namespace AnubisDBMS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Anubis11 : DbMigration
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
            DropForeignKey("SEG.ClaimsUsuarios", "IdUsuario", "SEG.Usuarios");
            DropForeignKey("SEG.RolesUsuarios", "IdRol", "SEG.Roles");
            DropForeignKey("SEG.SeguridadAreas", "SecurityLevel_IdNivel", "SEG.SeguridadNiveles");
            DropForeignKey("SEG.Permisos", "IdArea", "SEG.SeguridadAreas");
            DropIndex("SEG.LoginsUsuarios", new[] { "IdUsuario" });
            DropIndex("SEG.ClaimsUsuarios", new[] { "IdUsuario" });
            DropIndex("SEG.Usuarios", "UserNameIndex");
            DropIndex("SEG.RolesUsuarios", new[] { "IdRol" });
            DropIndex("SEG.RolesUsuarios", new[] { "IdUsuario" });
            DropIndex("SEG.Roles", "RoleNameIndex");
            DropIndex("SEG.Permisos", new[] { "IdArea" });
            DropIndex("SEG.SeguridadAreas", new[] { "SecurityLevel_IdNivel" });
            DropTable("SEG.LoginsUsuarios");
            DropTable("SEG.ClaimsUsuarios");
            DropTable("SEG.Usuarios");
            DropTable("SEG.RolesUsuarios");
            DropTable("SEG.Roles");
            DropTable("SEG.SeguridadNiveles");
            DropTable("SEG.Permisos");
            DropTable("SEG.SeguridadAreas");
        }
    }
}
