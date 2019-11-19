namespace AnubisDBMS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AnubisDBMS13 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "MON.Servicio",
                c => new
                    {
                        IdServicio = c.Long(nullable: false, identity: true),
                        EstadoServicio = c.Long(nullable: false),
                        Activo = c.Boolean(nullable: false),
                        FechaRegistro = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(),
                        FechaEliminacion = c.DateTime(),
                        UsuarioRegistro = c.String(),
                        UsuarioModificacion = c.String(),
                        UsuarioEliminacion = c.String(),
                    })
                .PrimaryKey(t => t.IdServicio);
            
        }
        
        public override void Down()
        {
            DropTable("MON.Servicio");
        }
    }
}
