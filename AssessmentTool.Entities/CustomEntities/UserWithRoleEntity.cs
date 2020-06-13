using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssessmentTool.Entities.CustomEntities
{
    public class UserWithRoleEntity
    {
        public AssessmentToolUser User { get; set; }

        public List<IdentityRole> Roles { get; set; }
    }
}
