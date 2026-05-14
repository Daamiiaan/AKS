using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;

namespace SchoolRegister.Tests
{
    public static class Extensions
    {
        public static void SeedData(this ApplicationDbContext dbContext)
        {
            // Roles
            var studentRole = new Role { Id = 1, Name = "Student", NormalizedName = "STUDENT" };
            var teacherRole = new Role { Id = 2, Name = "Teacher", NormalizedName = "TEACHER" };
            var parentRole  = new Role { Id = 3, Name = "Parent",  NormalizedName = "PARENT" };

            dbContext.Roles.AddRange(studentRole, teacherRole, parentRole);

            // Teachers
            var teacher1 = new Teacher
            {
                Id = 1,
                FirstName = "Jan",
                LastName = "Kowalski",
                UserName = "jan.kowalski@school.pl",
                NormalizedUserName = "JAN.KOWALSKI@SCHOOL.PL",
                Email = "jan.kowalski@school.pl",
                NormalizedEmail = "JAN.KOWALSKI@SCHOOL.PL",
                Title = "dr"
            };
            var teacher2 = new Teacher
            {
                Id = 2,
                FirstName = "Anna",
                LastName = "Nowak",
                UserName = "anna.nowak@school.pl",
                NormalizedUserName = "ANNA.NOWAK@SCHOOL.PL",
                Email = "anna.nowak@school.pl",
                NormalizedEmail = "ANNA.NOWAK@SCHOOL.PL",
                Title = "mgr"
            };

            // Parents
            var parent1 = new Parent
            {
                Id = 3,
                FirstName = "Marek",
                LastName = "Wiśniewski",
                UserName = "marek.wisniewski@mail.pl",
                NormalizedUserName = "MAREK.WISNIEWSKI@MAIL.PL",
                Email = "marek.wisniewski@mail.pl",
                NormalizedEmail = "MAREK.WISNIEWSKI@MAIL.PL"
            };

            // Students
            var student1 = new Student
            {
                Id = 4,
                FirstName = "Tomasz",
                LastName = "Kowalczyk",
                UserName = "tomasz.kowalczyk@school.pl",
                NormalizedUserName = "TOMASZ.KOWALCZYK@SCHOOL.PL",
                Email = "tomasz.kowalczyk@school.pl",
                NormalizedEmail = "TOMASZ.KOWALCZYK@SCHOOL.PL",
                ParentId = 3
            };
            var student2 = new Student
            {
                Id = 5,
                FirstName = "Kasia",
                LastName = "Zielińska",
                UserName = "kasia.zielinska@school.pl",
                NormalizedUserName = "KASIA.ZIELINSKA@SCHOOL.PL",
                Email = "kasia.zielinska@school.pl",
                NormalizedEmail = "KASIA.ZIELINSKA@SCHOOL.PL",
                ParentId = 3
            };

            dbContext.Users.AddRange(teacher1, teacher2, parent1, student1, student2);

            // Groups
            var group1 = new Group { Id = 1, Name = "3A" };
            var group2 = new Group { Id = 2, Name = "3B" };
            dbContext.Groups.AddRange(group1, group2);

            // Subjects
            var subject1 = new Subject { Id = 1, Name = "Matematyka", TeacherId = 1 };
            var subject2 = new Subject { Id = 2, Name = "Fizyka", TeacherId = 1 };
            var subject3 = new Subject { Id = 3, Name = "Język angielski", TeacherId = 2 };
            dbContext.Subjects.AddRange(subject1, subject2, subject3);

            // SubjectGroups
            dbContext.SubjectGroups.AddRange(
                new SubjectGroup { GroupId = 1, SubjectId = 1 },
                new SubjectGroup { GroupId = 1, SubjectId = 2 },
                new SubjectGroup { GroupId = 2, SubjectId = 3 }
            );

            // Assign students to groups
            student1.GroupId = 1;
            student2.GroupId = 1;

            // Grades
            dbContext.Grades.AddRange(
                new Grade { DateOfIssue = new DateTime(2024, 10, 1), GradeValue = GradeScale.BDB, SubjectId = 1, StudentId = 4 },
                new Grade { DateOfIssue = new DateTime(2024, 10, 5), GradeValue = GradeScale.DB,  SubjectId = 2, StudentId = 4 },
                new Grade { DateOfIssue = new DateTime(2024, 10, 8), GradeValue = GradeScale.DST, SubjectId = 1, StudentId = 5 }
            );

            dbContext.SaveChanges();
        }
    }
}
