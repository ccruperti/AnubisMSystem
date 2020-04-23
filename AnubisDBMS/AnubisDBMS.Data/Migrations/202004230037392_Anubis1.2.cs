namespace AnubisDBMS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Anubis12 : DbMigration
    {
        public override void Up()
        {
            AddColumn("MON.DataSensores", "AlertaRecibida", c => c.Boolean(nullable: false));
            AddColumn("SEG.Usuarios", "PrimeraNotificacion", c => c.Int(nullable: false));
            AddColumn("SEG.Usuarios", "SegundaNotificacion", c => c.Int(nullable: false));
            AddColumn("SEG.Usuarios", "TerceraNotificacion", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("SEG.Usuarios", "TerceraNotificacion");
            DropColumn("SEG.Usuarios", "SegundaNotificacion");
            DropColumn("SEG.Usuarios", "PrimeraNotificacion");
            DropColumn("MON.DataSensores", "AlertaRecibida");
        }
    }
}
