using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;

namespace SchoolRegister.Tests
{
    public static class Extensions
    {
        /// <summary>
        /// Seeds the in-memory database with test data from the lab PDF.
        /// Has a guard: if users already exist, seeding is skipped (prevents duplicate-key errors
        /// when multiple test classes call this in the same process).
        /// </summary>
        public static void SeedData(this ApplicationDbContext dbContext)
        {
            if (dbContext.Users.Any()) return; // Already seeded

            // ── Roles ────────────────────────────────────────────────────────────
            var teacherRole = new Role { Id = 3, Name = "Teacher", NormalizedName = "TEACHER", RoleValue = RoleValue.Teacher };
            var studentRole = new Role { Id = 1, Name = "Student", NormalizedName = "STUDENT", RoleValue = RoleValue.Student };
            var parentRole  = new Role { Id = 2, Name = "Parent",  NormalizedName = "PARENT",  RoleValue = RoleValue.Parent };
            var adminRole   = new Role { Id = 4, Name = "Admin",   NormalizedName = "ADMIN",   RoleValue = RoleValue.Admin };
            dbContext.Roles.AddRange(teacherRole, studentRole, parentRole, adminRole);

            // ── Teachers ─────────────────────────────────────────────────────────
            var t1 = new Teacher
            {
                Id = 1, FirstName = "Adam",      LastName = "Bednarski",   Title = "mgr inż.",
                UserName = "t1@eg.eg", NormalizedUserName = "T1@EG.EG",
                Email = "real_email@eg.eg", NormalizedEmail = "REAL_EMAIL@EG.EG",
                RegistrationDate = new DateTime(2010, 1, 1),
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var t2 = new Teacher
            {
                Id = 2, FirstName = "Jan",       LastName = "Nowak",       Title = "mgr",
                UserName = "t2@eg.eg", NormalizedUserName = "T2@EG.EG",
                Email = "t2@eg.eg", NormalizedEmail = "T2@EG.EG",
                RegistrationDate = new DateTime(2010, 11, 12),
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var t3 = new Teacher
            {
                Id = 12, FirstName = "Stanisław", LastName = "Nowakowski", Title = "mgr inż.",
                UserName = "t11@eg.eg", NormalizedUserName = "T11@EG.EG",
                Email = "t11@eg.eg", NormalizedEmail = "T11@EG.EG",
                RegistrationDate = new DateTime(2010, 11, 12),
                SecurityStamp = Guid.NewGuid().ToString()
            };

            // ── Parents ──────────────────────────────────────────────────────────
            var p1 = new Parent
            {
                Id = 3, FirstName = "Zbigniew", LastName = "Kowalski",
                UserName = "p1@eg.eg", NormalizedUserName = "P1@EG.EG",
                Email = "real_email@eg.eg", NormalizedEmail = "REAL_EMAIL@EG.EG",
                RegistrationDate = new DateTime(2014, 3, 20),
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var p2 = new Parent
            {
                Id = 4, FirstName = "Anna",     LastName = "Nowakowska",
                UserName = "p2@eg.eg", NormalizedUserName = "P2@EG.EG",
                Email = "p2@eg.eg", NormalizedEmail = "P2@EG.EG",
                RegistrationDate = new DateTime(2014, 6, 21),
                SecurityStamp = Guid.NewGuid().ToString()
            };

            // ── Students ─────────────────────────────────────────────────────────
            var s1 = new Student
            {
                Id = 5, FirstName = "Tomasz",   LastName = "Kowalski",
                UserName = "s1@eg.eg", NormalizedUserName = "S1@EG.EG",
                Email = "s1@eg.eg", NormalizedEmail = "S1@EG.EG",
                RegistrationDate = new DateTime(2016, 5, 11),
                GroupId = 1, ParentId = 3,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var s2 = new Student
            {
                Id = 6, FirstName = "Krzysztof", LastName = "Kowalski",
                UserName = "s2@eg.eg", NormalizedUserName = "S2@EG.EG",
                Email = "s2@eg.eg", NormalizedEmail = "S2@EG.EG",
                RegistrationDate = new DateTime(2015, 9, 18),
                GroupId = 1, ParentId = 3,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var s3 = new Student
            {
                Id = 7, FirstName = "Natalia",   LastName = "Kowalska",
                UserName = "s3@eg.eg", NormalizedUserName = "S3@EG.EG",
                Email = "s3@eg.eg", NormalizedEmail = "S3@EG.EG",
                RegistrationDate = new DateTime(2017, 7, 16),
                GroupId = 2, ParentId = 3,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var s4 = new Student
            {
                Id = 8, FirstName = "Magdalena", LastName = "Wiśniewska",
                UserName = "s4@eg.eg", NormalizedUserName = "S4@EG.EG",
                Email = "s4@eg.eg", NormalizedEmail = "S4@EG.EG",
                RegistrationDate = new DateTime(2018, 5, 14),
                GroupId = 2, ParentId = 4,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var s5 = new Student
            {
                Id = 9, FirstName = "Jan",       LastName = "Wiśniewski",
                UserName = "s5@eg.eg", NormalizedUserName = "S5@EG.EG",
                Email = "s5@eg.eg", NormalizedEmail = "S5@EG.EG",
                RegistrationDate = new DateTime(2019, 2, 19),
                GroupId = 3, ParentId = 4,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var s6 = new Student
            {
                Id = 10, FirstName = "Krystian", LastName = "Wiśniewski",
                UserName = "s6@eg.eg", NormalizedUserName = "S6@EG.EG",
                Email = "s6@eg.eg", NormalizedEmail = "S6@EG.EG",
                RegistrationDate = new DateTime(2019, 5, 1),
                GroupId = 3, ParentId = 4,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            // ── Admin ────────────────────────────────────────────────────────────
            var a1 = new User
            {
                Id = 11, FirstName = "Jacek", LastName = "Kowalczyk",
                UserName = "a1@eg.eg", NormalizedUserName = "A1@EG.EG",
                Email = "a1@eg.eg", NormalizedEmail = "A1@EG.EG",
                RegistrationDate = new DateTime(2009, 1, 1),
                SecurityStamp = Guid.NewGuid().ToString()
            };

            dbContext.Users.AddRange(t1, t2, t3, p1, p2, s1, s2, s3, s4, s5, s6, a1);

            // ── Groups ───────────────────────────────────────────────────────────
            dbContext.Groups.AddRange(
                new Group { Id = 1, Name = "IO" },
                new Group { Id = 2, Name = "PAI" },
                new Group { Id = 3, Name = "AIP Erasmus" }
            );

            // ── Subjects ─────────────────────────────────────────────────────────
            dbContext.Subjects.AddRange(
                new Subject { Id = 1, Name = "Aplikacje WWW",                 Description = "Aplikacje webowe",                                                                        TeacherId = 1 },
                new Subject { Id = 2, Name = "Programowanie obiektowe",        Description = "Programowanie obiektowe jest przedmiotem realizującym przykłady programowania obiektowego", TeacherId = 1 },
                new Subject { Id = 3, Name = "Advanced Internet Programming",  Description = "Advanced Internet Programming is a course for ERASMUS+ students",                          TeacherId = 2 },
                new Subject { Id = 4, Name = "Administracja Intenetowymi Systemami Baz Danych", Description = "Administracja Intenetowymi Systemami Baz Danych jest kontynuacją przedmiotu Bazy danych", TeacherId = 2 },
                new Subject { Id = 5, Name = "Programowanie interaktywnej grafiki dla stron WWW", TeacherId = 12 }
            );

            // ── SubjectGroups ────────────────────────────────────────────────────
            dbContext.SubjectGroups.AddRange(
                new SubjectGroup { SubjectId = 1, GroupId = 1 },
                new SubjectGroup { SubjectId = 1, GroupId = 2 },
                new SubjectGroup { SubjectId = 2, GroupId = 1 },
                new SubjectGroup { SubjectId = 2, GroupId = 2 },
                new SubjectGroup { SubjectId = 2, GroupId = 3 },
                new SubjectGroup { SubjectId = 3, GroupId = 3 },
                new SubjectGroup { SubjectId = 4, GroupId = 2 },
                new SubjectGroup { SubjectId = 4, GroupId = 3 }
            );

            // ── Grades ───────────────────────────────────────────────────────────
            dbContext.Grades.Add(new Grade
            {
                DateOfIssue = new DateTime(2019, 3, 21, 17, 46, 38),
                StudentId = 5,
                SubjectId = 1,
                GradeValue = GradeScale.DB
            });

            // ── User-Role assignments ────────────────────────────────────────────
            dbContext.Set<IdentityUserRole<int>>().AddRange(
                new IdentityUserRole<int> { UserId = 1,  RoleId = 3 }, // t1  → Teacher
                new IdentityUserRole<int> { UserId = 2,  RoleId = 3 }, // t2  → Teacher
                new IdentityUserRole<int> { UserId = 12, RoleId = 3 }, // t3  → Teacher
                new IdentityUserRole<int> { UserId = 3,  RoleId = 2 }, // p1  → Parent
                new IdentityUserRole<int> { UserId = 4,  RoleId = 2 }, // p2  → Parent
                new IdentityUserRole<int> { UserId = 5,  RoleId = 1 }, // s1  → Student
                new IdentityUserRole<int> { UserId = 6,  RoleId = 1 }, // s2  → Student
                new IdentityUserRole<int> { UserId = 7,  RoleId = 1 }, // s3  → Student
                new IdentityUserRole<int> { UserId = 8,  RoleId = 1 }, // s4  → Student
                new IdentityUserRole<int> { UserId = 9,  RoleId = 1 }, // s5  → Student
                new IdentityUserRole<int> { UserId = 10, RoleId = 1 }, // s6  → Student
                new IdentityUserRole<int> { UserId = 11, RoleId = 4 }  // a1  → Admin
            );

            dbContext.SaveChanges();
        }
    }
}
