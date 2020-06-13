namespace AssessmentTool.Data.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using AssessmentTool.Entities;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<AssessmentTool.Data.AssessmentToolContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "AssessmentTool.Data.AssessmentToolContext";
        }

        protected override void Seed(AssessmentTool.Data.AssessmentToolContext context)
        {
            //  This method will be called after migrating to the latest version.
        }

    }
}
