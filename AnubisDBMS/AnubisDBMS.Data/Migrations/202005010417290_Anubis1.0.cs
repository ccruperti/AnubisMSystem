namespace AnubisDBMS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Anubis10 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("MON.Servicio", "IdEmpresa", "GEN.Empresas");
            DropIndex("MON.Servicio", new[] { "IdEmpresa" });
            DropTable("MON.Servicio");
        }
        
        public override void Down()
        {
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
                .PrimaryKey(t => t.IdServicio);
            
            CreateIndex("MON.Servicio", "IdEmpresa");
            AddForeignKey("MON.Servicio", "IdEmpresa", "GEN.Empresas", "IdEmpresa");
        }
    }
}
