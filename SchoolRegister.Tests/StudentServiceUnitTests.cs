using SchoolRegister.DAL.EF;
using SchoolRegister.Services.Interfaces;
using Xunit;

namespace SchoolRegister.Tests
{
    public class StudentServiceUnitTests : BaseUnitTests
    {
        private readonly IStudentService _studentService;

        public StudentServiceUnitTests(ApplicationDbContext dbContext, IStudentService studentService)
            : base(dbContext)
        {
            _studentService = studentService;
        }

        [Fact]
        public void GetStudent_ExistingId_ReturnsStudentVm()
        {
            var result = _studentService.GetStudent(4);
            Assert.NotNull(result);
            Assert.Equal("Tomasz", result.FirstName);
        }

        [Fact]
        public void GetStudent_NonExistingId_ReturnsNull()
        {
            var result = _studentService.GetStudent(999);
            Assert.Null(result);
        }

        [Fact]
        public void GetStudents_ReturnsAll()
        {
            var result = _studentService.GetStudents();
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void GetStudents_WithFilter_ReturnsFiltered()
        {
            var result = _studentService.GetStudents(s => s.GroupId == 1);
            Assert.All(result, s => Assert.Equal(1, s.GroupId));
        }

        [Fact]
        public void GetStudents_FilterByName_ReturnsCorrect()
        {
            var result = _studentService.GetStudents(s => s.FirstName == "Kasia");
            Assert.Single(result);
        }
    }
}
