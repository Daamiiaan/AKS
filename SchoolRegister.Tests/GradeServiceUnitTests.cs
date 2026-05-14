using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using Xunit;

namespace SchoolRegister.Tests
{
    public class GradeServiceUnitTests : BaseUnitTests
    {
        private readonly IGradeService _gradeService;

        public GradeServiceUnitTests(ApplicationDbContext dbContext, IGradeService gradeService)
            : base(dbContext)
        {
            _gradeService = gradeService;
        }

        [Fact]
        public void AddGradeToStudent_ValidData_ReturnsGradeVm()
        {
            var vm = new AddGradeToStudentVm
            {
                StudentId = 4,
                SubjectId = 1,
                TeacherId = 1,
                GradeValue = GradeScale.BDB
            };

            var result = _gradeService.AddGradeToStudent(vm);
            Assert.NotNull(result);
            Assert.Equal(GradeScale.BDB, result.GradeValue);
        }

        [Fact]
        public void AddGradeToStudent_WrongTeacher_ThrowsException()
        {
            var vm = new AddGradeToStudentVm
            {
                StudentId = 4,
                SubjectId = 3, // Subject 3 belongs to Teacher 2, not Teacher 1
                TeacherId = 1,
                GradeValue = GradeScale.DB
            };

            Assert.Throws<ArgumentException>(() => _gradeService.AddGradeToStudent(vm));
        }

        [Fact]
        public void GetGradesReportForStudent_ByStudentId_ReturnsReport()
        {
            var vm = new GetGradesReportVm { StudentId = 4 };
            var result = _gradeService.GetGradesReportForStudent(vm);
            Assert.NotNull(result);
            Assert.Equal(4, result.StudentId);
            Assert.True(result.Grades.Count >= 2);
        }

        [Fact]
        public void GetGradesReportForStudent_ByParentId_ReturnsReport()
        {
            var vm = new GetGradesReportVm { ParentId = 3 };
            var result = _gradeService.GetGradesReportForStudent(vm);
            Assert.NotNull(result);
        }

        [Fact]
        public void GetGradesReportForStudent_NonExistingStudent_ReturnsNull()
        {
            var vm = new GetGradesReportVm { StudentId = 999 };
            var result = _gradeService.GetGradesReportForStudent(vm);
            Assert.Null(result);
        }
    }
}
