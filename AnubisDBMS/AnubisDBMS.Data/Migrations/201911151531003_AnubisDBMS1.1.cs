namespace AnubisDBMS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AnubisDBMS11 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("CAT.Sensores", "FechaConsulta", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("CAT.Sensores", "FechaConsulta", c => c.DateTime(nullable: false));
        }
    }
}
