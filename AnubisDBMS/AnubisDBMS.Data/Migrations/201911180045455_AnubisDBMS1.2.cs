namespace AnubisDBMS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AnubisDBMS12 : DbMigration
    {
        public override void Up()
        {
            DropIndex("MON.EquipoSensores", new[] { "IdEquipo" });
            DropIndex("MON.EquipoSensores", new[] { "IdSensor" });
            AlterColumn("MON.EquipoSensores", "IdEquipo", c => c.Long());
            AlterColumn("MON.EquipoSensores", "IdSensor", c => c.Long());
            CreateIndex("MON.EquipoSensores", "IdEquipo");
            CreateIndex("MON.EquipoSensores", "IdSensor");
        }
        
        public override void Down()
        {
            DropIndex("MON.EquipoSensores", new[] { "IdSensor" });
            DropIndex("MON.EquipoSensores", new[] { "IdEquipo" });
            AlterColumn("MON.EquipoSensores", "IdSensor", c => c.Long(nullable: false));
            AlterColumn("MON.EquipoSensores", "IdEquipo", c => c.Long(nullable: false));
            CreateIndex("MON.EquipoSensores", "IdSensor");
            CreateIndex("MON.EquipoSensores", "IdEquipo");
        }
    }
}
