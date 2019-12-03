namespace AnubisDBMS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AnubisDBMS15 : DbMigration
    {
        public override void Up()
        {
            AddColumn("MON.DataSensores", "IdEquipoSensor", c => c.Long());
            CreateIndex("MON.DataSensores", "IdEquipoSensor");
            AddForeignKey("MON.DataSensores", "IdEquipoSensor", "MON.EquipoSensores", "IdEquipoSensor");
        }
        
        public override void Down()
        {
            DropForeignKey("MON.DataSensores", "IdEquipoSensor", "MON.EquipoSensores");
            DropIndex("MON.DataSensores", new[] { "IdEquipoSensor" });
            DropColumn("MON.DataSensores", "IdEquipoSensor");
        }
    }
}
