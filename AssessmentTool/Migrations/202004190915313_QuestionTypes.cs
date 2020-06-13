namespace AssessmentTool.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuestionTypes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questions", "QuestionTypes", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Questions", "QuestionTypes");
        }
    }
}
