namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCreateTimeColumnToStudentsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students", "CreateTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Students", "CreateTime");
        }
    }
}
