namespace AssessmentTool.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AttemptedQuestions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        StudentQuizID = c.Int(),
                        QuestionID = c.Int(nullable: false),
                        AnsweredAt = c.DateTime(precision: 0),
                        IsCorrect = c.Boolean(nullable: false),
                        Score = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ModifiedOn = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Questions", t => t.QuestionID, cascadeDelete: true)
                .ForeignKey("dbo.StudentQuizs", t => t.StudentQuizID)
                .Index(t => t.StudentQuizID)
                .Index(t => t.QuestionID);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(unicode: false),
                        QuizID = c.Int(nullable: false),
                        ImageID = c.Int(),
                        QuestionType = c.String(unicode: false),
                        ModifiedOn = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Images", t => t.ImageID)
                .ForeignKey("dbo.Quizs", t => t.QuizID, cascadeDelete: true)
                .Index(t => t.QuizID)
                .Index(t => t.ImageID);
            
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Options",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Answer = c.String(unicode: false),
                        IsCorrect = c.Boolean(nullable: false),
                        ImageID = c.Int(),
                        ModifiedOn = c.DateTime(precision: 0),
                        Question_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Images", t => t.ImageID)
                .ForeignKey("dbo.Questions", t => t.Question_ID)
                .Index(t => t.ImageID)
                .Index(t => t.Question_ID);
            
            CreateTable(
                "dbo.Quizs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                        Description = c.String(unicode: false),
                        TimeDuration = c.Time(nullable: false, precision: 0),
                        EnableQuizTimer = c.Boolean(nullable: false),
                        EnableQuestionTimer = c.Boolean(nullable: false),
                        OwnerID = c.String(unicode: false),
                        QuizType = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        ModifiedOn = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.AttemptedOptions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AttemptedQuestionID = c.Int(nullable: false),
                        OptionID = c.Int(nullable: false),
                        ModifiedOn = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AttemptedQuestions", t => t.AttemptedQuestionID, cascadeDelete: true)
                .ForeignKey("dbo.Options", t => t.OptionID, cascadeDelete: true)
                .Index(t => t.AttemptedQuestionID)
                .Index(t => t.OptionID);
            
            CreateTable(
                "dbo.StudentQuizs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        StudentID = c.String(maxLength: 128, storeType: "nvarchar"),
                        QuizID = c.Int(nullable: false),
                        StartedAt = c.DateTime(precision: 0),
                        CompletedAt = c.DateTime(precision: 0),
                        Score = c.Int(nullable: false),
                        ModifiedOn = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Quizs", t => t.QuizID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.StudentID)
                .Index(t => t.StudentID)
                .Index(t => t.QuizID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        Photo = c.String(unicode: false),
                        RegisteredOn = c.DateTime(precision: 0),
                        Email = c.String(maxLength: 256, storeType: "nvarchar"),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(unicode: false),
                        SecurityStamp = c.String(unicode: false),
                        PhoneNumber = c.String(unicode: false),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(precision: 0),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.UserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        ClaimType = c.String(unicode: false),
                        ClaimValue = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        ProviderKey = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        UserId = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        RoleId = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        Name = c.String(nullable: false, maxLength: 256, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.StudentQuizs", "StudentID", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserLogins", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserClaims", "UserId", "dbo.Users");
            DropForeignKey("dbo.StudentQuizs", "QuizID", "dbo.Quizs");
            DropForeignKey("dbo.AttemptedQuestions", "StudentQuizID", "dbo.StudentQuizs");
            DropForeignKey("dbo.AttemptedOptions", "OptionID", "dbo.Options");
            DropForeignKey("dbo.AttemptedOptions", "AttemptedQuestionID", "dbo.AttemptedQuestions");
            DropForeignKey("dbo.AttemptedQuestions", "QuestionID", "dbo.Questions");
            DropForeignKey("dbo.Questions", "QuizID", "dbo.Quizs");
            DropForeignKey("dbo.Options", "Question_ID", "dbo.Questions");
            DropForeignKey("dbo.Options", "ImageID", "dbo.Images");
            DropForeignKey("dbo.Questions", "ImageID", "dbo.Images");
            DropIndex("dbo.Roles", "RoleNameIndex");
            DropIndex("dbo.UserRoles", new[] { "RoleId" });
            DropIndex("dbo.UserRoles", new[] { "UserId" });
            DropIndex("dbo.UserLogins", new[] { "UserId" });
            DropIndex("dbo.UserClaims", new[] { "UserId" });
            DropIndex("dbo.Users", "UserNameIndex");
            DropIndex("dbo.StudentQuizs", new[] { "QuizID" });
            DropIndex("dbo.StudentQuizs", new[] { "StudentID" });
            DropIndex("dbo.AttemptedOptions", new[] { "OptionID" });
            DropIndex("dbo.AttemptedOptions", new[] { "AttemptedQuestionID" });
            DropIndex("dbo.Options", new[] { "Question_ID" });
            DropIndex("dbo.Options", new[] { "ImageID" });
            DropIndex("dbo.Questions", new[] { "ImageID" });
            DropIndex("dbo.Questions", new[] { "QuizID" });
            DropIndex("dbo.AttemptedQuestions", new[] { "QuestionID" });
            DropIndex("dbo.AttemptedQuestions", new[] { "StudentQuizID" });
            DropTable("dbo.Roles");
            DropTable("dbo.UserRoles");
            DropTable("dbo.UserLogins");
            DropTable("dbo.UserClaims");
            DropTable("dbo.Users");
            DropTable("dbo.StudentQuizs");
            DropTable("dbo.AttemptedOptions");
            DropTable("dbo.Quizs");
            DropTable("dbo.Options");
            DropTable("dbo.Images");
            DropTable("dbo.Questions");
            DropTable("dbo.AttemptedQuestions");
        }
    }
}
