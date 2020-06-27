namespace AnubisDBMS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Anubis12 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "MON.Dispositivos",
                c => new
                    {
                        IdDispositivos = c.Long(nullable: false, identity: true),
                        ip = c.String(),
                        ult_lectura = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IdDispositivos);
            
        }
        
        public override void Down()
        {
            DropTable("MON.Dispositivos");
        }
    }
}
