namespace AnubisDBMS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Anubis10 : DbMigration
    {
        public override void Up()
        {
            AddColumn("GEN.Empresas", "EmailNotificacion", c => c.String());
            AddColumn("GEN.Empresas", "PrimeraNotificacion", c => c.Int(nullable: false));
            AddColumn("GEN.Empresas", "SegundaNotificacion", c => c.Int(nullable: false));
            AddColumn("GEN.Empresas", "TerceraNotificacion", c => c.Int(nullable: false));
            DropColumn("SEG.Usuarios", "PrimeraNotificacion");
            DropColumn("SEG.Usuarios", "SegundaNotificacion");
            DropColumn("SEG.Usuarios", "TerceraNotificacion");
        }
        
        public override void Down()
        {
            AddColumn("SEG.Usuarios", "TerceraNotificacion", c => c.Int(nullable: false));
            AddColumn("SEG.Usuarios", "SegundaNotificacion", c => c.Int(nullable: false));
            AddColumn("SEG.Usuarios", "PrimeraNotificacion", c => c.Int(nullable: false));
            DropColumn("GEN.Empresas", "TerceraNotificacion");
            DropColumn("GEN.Empresas", "SegundaNotificacion");
            DropColumn("GEN.Empresas", "PrimeraNotificacion");
            DropColumn("GEN.Empresas", "EmailNotificacion");
        }
    }
}
