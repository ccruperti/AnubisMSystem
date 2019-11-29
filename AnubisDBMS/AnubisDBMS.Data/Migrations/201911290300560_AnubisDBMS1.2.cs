namespace AnubisDBMS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AnubisDBMS12 : DbMigration
    {
        public override void Up()
        {
            AddColumn("CAT.Equipos", "AplicaMantenimiento", c => c.Boolean(nullable: false));
            DropColumn("CAT.Equipos", "AplicaConfiguracion");
        }
        
        public override void Down()
        {
            AddColumn("CAT.Equipos", "AplicaConfiguracion", c => c.Boolean(nullable: false));
            DropColumn("CAT.Equipos", "AplicaMantenimiento");
        }
    }
}
