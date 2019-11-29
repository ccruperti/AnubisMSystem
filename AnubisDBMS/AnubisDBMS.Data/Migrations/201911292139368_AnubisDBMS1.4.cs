namespace AnubisDBMS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AnubisDBMS14 : DbMigration
    {
        public override void Up()
        {
            AddColumn("CAT.Equipos", "AplicaMonitoreo", c => c.Boolean(nullable: false));
            DropColumn("CAT.Equipos", "AplicaMantenimiento");
        }
        
        public override void Down()
        {
            AddColumn("CAT.Equipos", "AplicaMantenimiento", c => c.Boolean(nullable: false));
            DropColumn("CAT.Equipos", "AplicaMonitoreo");
        }
    }
}
