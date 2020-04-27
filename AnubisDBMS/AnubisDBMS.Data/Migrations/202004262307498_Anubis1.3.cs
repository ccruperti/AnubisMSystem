namespace AnubisDBMS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Anubis13 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "GEN.Empresas",
                c => new
                    {
                        IdEmpresa = c.Long(nullable: false, identity: true),
                        Nombre = c.String(),
                        RUC = c.String(),
                        RazonSocial = c.String(),
                        ServicioActivo = c.Boolean(nullable: false),
                        Activo = c.Boolean(nullable: false),
                        FechaRegistro = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(),
                        FechaEliminacion = c.DateTime(),
                        UsuarioRegistro = c.String(),
                        UsuarioModificacion = c.String(),
                        UsuarioEliminacion = c.String(),
                    })
                .PrimaryKey(t => t.IdEmpresa);
            
            AddColumn("MON.DataSensores", "IdEmpresa", c => c.Long());
            AddColumn("CAT.Equipos", "IdEmpresa", c => c.Long());
            AddColumn("MON.EquipoSensores", "IdEmpresa", c => c.Long());
            AddColumn("CAT.Sensores", "IdEmpresa", c => c.Long());
            AddColumn("CAT.TipoSensores", "IdEmpresa", c => c.Long());
            AddColumn("CAT.Estado", "IdEmpresa", c => c.Long());
            AddColumn("CAT.Frecuencias", "IdEmpresa", c => c.Long());
            AddColumn("MON.Mantenimientos", "IdEmpresa", c => c.Long());
            AddColumn("CAT.Tecnicos", "IdEmpresa", c => c.Long());
            AddColumn("MON.Servicio", "IdEmpresa", c => c.Long());
            AddColumn("SEG.Usuarios", "IdEmpresa", c => c.Long());
            CreateIndex("MON.DataSensores", "IdEmpresa");
            CreateIndex("CAT.Equipos", "IdEmpresa");
            CreateIndex("MON.EquipoSensores", "IdEmpresa");
            CreateIndex("CAT.Sensores", "IdEmpresa");
            CreateIndex("CAT.TipoSensores", "IdEmpresa");
            CreateIndex("CAT.Estado", "IdEmpresa");
            CreateIndex("CAT.Frecuencias", "IdEmpresa");
            CreateIndex("MON.Mantenimientos", "IdEmpresa");
            CreateIndex("CAT.Tecnicos", "IdEmpresa");
            CreateIndex("MON.Servicio", "IdEmpresa");
            CreateIndex("SEG.Usuarios", "IdEmpresa");
            AddForeignKey("MON.DataSensores", "IdEmpresa", "GEN.Empresas", "IdEmpresa");
            AddForeignKey("CAT.Equipos", "IdEmpresa", "GEN.Empresas", "IdEmpresa");
            AddForeignKey("MON.EquipoSensores", "IdEmpresa", "GEN.Empresas", "IdEmpresa");
            AddForeignKey("CAT.Sensores", "IdEmpresa", "GEN.Empresas", "IdEmpresa");
            AddForeignKey("CAT.TipoSensores", "IdEmpresa", "GEN.Empresas", "IdEmpresa");
            AddForeignKey("CAT.Estado", "IdEmpresa", "GEN.Empresas", "IdEmpresa");
            AddForeignKey("CAT.Frecuencias", "IdEmpresa", "GEN.Empresas", "IdEmpresa");
            AddForeignKey("MON.Mantenimientos", "IdEmpresa", "GEN.Empresas", "IdEmpresa");
            AddForeignKey("CAT.Tecnicos", "IdEmpresa", "GEN.Empresas", "IdEmpresa");
            AddForeignKey("MON.Servicio", "IdEmpresa", "GEN.Empresas", "IdEmpresa");
            AddForeignKey("SEG.Usuarios", "IdEmpresa", "GEN.Empresas", "IdEmpresa");
        }
        
        public override void Down()
        {
            DropForeignKey("SEG.Usuarios", "IdEmpresa", "GEN.Empresas");
            DropForeignKey("MON.Servicio", "IdEmpresa", "GEN.Empresas");
            DropForeignKey("CAT.Tecnicos", "IdEmpresa", "GEN.Empresas");
            DropForeignKey("MON.Mantenimientos", "IdEmpresa", "GEN.Empresas");
            DropForeignKey("CAT.Frecuencias", "IdEmpresa", "GEN.Empresas");
            DropForeignKey("CAT.Estado", "IdEmpresa", "GEN.Empresas");
            DropForeignKey("CAT.TipoSensores", "IdEmpresa", "GEN.Empresas");
            DropForeignKey("CAT.Sensores", "IdEmpresa", "GEN.Empresas");
            DropForeignKey("MON.EquipoSensores", "IdEmpresa", "GEN.Empresas");
            DropForeignKey("CAT.Equipos", "IdEmpresa", "GEN.Empresas");
            DropForeignKey("MON.DataSensores", "IdEmpresa", "GEN.Empresas");
            DropIndex("SEG.Usuarios", new[] { "IdEmpresa" });
            DropIndex("MON.Servicio", new[] { "IdEmpresa" });
            DropIndex("CAT.Tecnicos", new[] { "IdEmpresa" });
            DropIndex("MON.Mantenimientos", new[] { "IdEmpresa" });
            DropIndex("CAT.Frecuencias", new[] { "IdEmpresa" });
            DropIndex("CAT.Estado", new[] { "IdEmpresa" });
            DropIndex("CAT.TipoSensores", new[] { "IdEmpresa" });
            DropIndex("CAT.Sensores", new[] { "IdEmpresa" });
            DropIndex("MON.EquipoSensores", new[] { "IdEmpresa" });
            DropIndex("CAT.Equipos", new[] { "IdEmpresa" });
            DropIndex("MON.DataSensores", new[] { "IdEmpresa" });
            DropColumn("SEG.Usuarios", "IdEmpresa");
            DropColumn("MON.Servicio", "IdEmpresa");
            DropColumn("CAT.Tecnicos", "IdEmpresa");
            DropColumn("MON.Mantenimientos", "IdEmpresa");
            DropColumn("CAT.Frecuencias", "IdEmpresa");
            DropColumn("CAT.Estado", "IdEmpresa");
            DropColumn("CAT.TipoSensores", "IdEmpresa");
            DropColumn("CAT.Sensores", "IdEmpresa");
            DropColumn("MON.EquipoSensores", "IdEmpresa");
            DropColumn("CAT.Equipos", "IdEmpresa");
            DropColumn("MON.DataSensores", "IdEmpresa");
            DropTable("GEN.Empresas");
        }
    }
}
