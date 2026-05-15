using System.Linq;
using SchoolRegister.DAL.EF;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using Xunit;

namespace SchoolRegister.Tests.UnitTests
{
    public class TeacherServiceUnitTests : BaseUnitTests
    {
        private readonly ITeacherService _teacherService;

        public TeacherServiceUnitTests(ApplicationDbContext dbContext, ITeacherService teacherService)
            : base(dbContext)
        {
            _teacherService = teacherService;
        }

        [Fact]
        public void GetTeacher()
        {
            var teacher = _teacherService.GetTeacher(x => x.UserName == "t1@eg.eg");
            Assert.NotNull(teacher);
        }

        [Fact]
        public void GetTeachers()
        {
            var teachers = _teacherService.GetTeachers(x => x.UserName.Contains("@eg.eg"));
            Assert.NotNull(teachers);
            Assert.NotEmpty(teachers);
            Assert.Equal(3, teachers.Count());
        }

        [Fact]
        public void GetAllTeachers()
        {
            var teachers = _teacherService.GetTeachers();
            Assert.NotNull(teachers);
            Assert.NotEmpty(teachers);
            Assert.Equal(3, teachers.Count());
        }

        [Fact]
        public void GetTeachersGroups()
        {
            var getTeachersGroup = new TeachersGroupsVm
            {
                TeacherId = 1
            };
            var teachersGroups = _teacherService.GetTeachersGroups(getTeachersGroup);
            Assert.NotNull(teachersGroups);
            Assert.NotEmpty(teachersGroups);
            // Teacher 1 teaches subjects 1 and 2.
            // Subject 1 -> groups IO (1), PAI (2) = 2 entries
            // Subject 2 -> groups IO (1), PAI (2), AIP Erasmus (3) = 3 entries
            // Total = 5 group entries (with repeats)
            Assert.Equal(5, teachersGroups.Count());
        }
    }
}
