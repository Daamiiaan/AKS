using SchoolRegister.DAL.EF;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using Xunit;

namespace SchoolRegister.Tests
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
        public void GetTeacher_ExistingId_ReturnsTeacherVm()
        {
            var result = _teacherService.GetTeacher(1);
            Assert.NotNull(result);
            Assert.Equal("Jan", result.FirstName);
            Assert.Equal("Kowalski", result.LastName);
        }

        [Fact]
        public void GetTeacher_NonExistingId_ReturnsNull()
        {
            var result = _teacherService.GetTeacher(999);
            Assert.Null(result);
        }

        [Fact]
        public void GetTeachers_ReturnsAll()
        {
            var result = _teacherService.GetTeachers();
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void AttachSubjectToTeacher_ValidIds_AssignsTeacher()
        {
            var vm = new AttachDetachSubjectToTeacherVm { TeacherId = 2, SubjectId = 1 };
            var result = _teacherService.AttachSubjectToTeacher(vm);
            Assert.NotNull(result);
        }

        [Fact]
        public void GetTeachersGroups_TeacherWithSubjects_ReturnsGroups()
        {
            var result = _teacherService.GetTeachersGroups(1);
            Assert.NotNull(result);
            Assert.True(result.Groups.Count > 0);
        }
    }
}
