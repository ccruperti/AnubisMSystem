namespace AnubisDBMS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AnubisDBMS15 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "MON.DataSensores",
                c => new
                    {
                        IdDataSensor = c.Long(nullable: false, identity: true),
                        ModeloSensor = c.String(),
                        TipoSensor = c.String(),
                        FechaLectura = c.DateTime(nullable: false),
                        lectura = c.Double(nullable: false),
                        UnidadMedida = c.String(),
                        Activo = c.Boolean(nullable: false),
                        FechaRegistro = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(),
                        FechaEliminacion = c.DateTime(),
                        UsuarioRegistro = c.String(),
                        UsuarioModificacion = c.String(),
                        UsuarioEliminacion = c.String(),
                    })
                .PrimaryKey(t => t.IdDataSensor);
            
        }
        
        public override void Down()
        {
            DropTable("MON.DataSensores");
        }
    }
}
