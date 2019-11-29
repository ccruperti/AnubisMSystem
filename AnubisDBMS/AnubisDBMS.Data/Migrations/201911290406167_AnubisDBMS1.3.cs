namespace AnubisDBMS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AnubisDBMS13 : DbMigration
    {
        public override void Up()
        {
            AddColumn("MON.Mantenimientos", "FechaFinMantenimiento", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("MON.Mantenimientos", "FechaFinMantenimiento");
        }
    }
}
