namespace AnubisDBMS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AnubisDBMS11 : DbMigration
    {
        public override void Up()
        {
            AddColumn("CAT.Equipos", "AplicaConfiguracion", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("CAT.Equipos", "AplicaConfiguracion");
        }
    }
}
