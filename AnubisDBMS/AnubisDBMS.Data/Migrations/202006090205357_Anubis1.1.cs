namespace AnubisDBMS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Anubis11 : DbMigration
    {
        public override void Up()
        {
            DropColumn("CAT.Sensores", "SerieSensor");
        }
        
        public override void Down()
        {
            AddColumn("CAT.Sensores", "SerieSensor", c => c.String());
        }
    }
}
