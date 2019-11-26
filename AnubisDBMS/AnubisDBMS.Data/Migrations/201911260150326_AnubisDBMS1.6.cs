namespace AnubisDBMS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AnubisDBMS16 : DbMigration
    {
        public override void Up()
        {
            AddColumn("MON.Mantenimientos", "FechaMantenimiento", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("MON.Mantenimientos", "FechaMantenimiento");
        }
    }
}
