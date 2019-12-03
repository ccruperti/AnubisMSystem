namespace AnubisDBMS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AnubisDBMS15 : DbMigration
    {
        public override void Up()
        {
            AddColumn("CAT.Sensores", "Max", c => c.Double());
            AddColumn("CAT.Sensores", "Min", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("CAT.Sensores", "Min");
            DropColumn("CAT.Sensores", "Max");
        }
    }
}
