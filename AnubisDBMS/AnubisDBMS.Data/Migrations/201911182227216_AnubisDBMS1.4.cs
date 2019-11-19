namespace AnubisDBMS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AnubisDBMS14 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("MON.Servicio", "EstadoServicio", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("MON.Servicio", "EstadoServicio", c => c.Long(nullable: false));
        }
    }
}
