using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineEducationPlatform.Shared.Entities
{
    public class Policy
    {
        public const string SuperAdmin = "SuperAdminId";
        public const string TenantAdmin = "TenantId";
        public const string CourseCreator = "CourseCreatorId";
        public const string CourseEditor = "CourseEditorId";
        public const string CourseViewer = "CourseViewerId";
        public const string Instructor = "InstructorId";
        public const string Student = "StudentId";
        public const string LimitedRolePolicy = "LimitedRoleId";

    }
}
