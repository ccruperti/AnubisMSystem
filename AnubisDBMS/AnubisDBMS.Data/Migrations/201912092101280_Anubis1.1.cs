namespace AnubisDBMS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Anubis11 : DbMigration
    {
        public override void Up()
        {
            AddColumn("CAT.TipoSensores", "Min_TipoSensor", c => c.Double(nullable: false));
            AddColumn("CAT.TipoSensores", "Max_TipoSensor", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("CAT.TipoSensores", "Max_TipoSensor");
            DropColumn("CAT.TipoSensores", "Min_TipoSensor");
        }
    }
}
