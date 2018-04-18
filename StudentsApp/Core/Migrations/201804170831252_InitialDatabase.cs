namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PersonalNumber = c.String(nullable: false, maxLength: 11),
                        Firstname = c.String(nullable: false, maxLength: 500),
                        Lastname = c.String(nullable: false, maxLength: 500),
                        Gender = c.Int(nullable: false),
                        Birthdate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.PersonalNumber, unique: true, name: "IX_PersonalNumberUnique");
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Students", "IX_PersonalNumberUnique");
            DropTable("dbo.Students");
        }
    }
}
