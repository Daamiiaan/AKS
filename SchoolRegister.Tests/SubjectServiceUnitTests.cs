using SchoolRegister.DAL.EF;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using Xunit;

namespace SchoolRegister.Tests
{
    public class SubjectServiceUnitTests : BaseUnitTests
    {
        private readonly ISubjectService _subjectService;

        public SubjectServiceUnitTests(ApplicationDbContext dbContext, ISubjectService subjectService)
            : base(dbContext)
        {
            _subjectService = subjectService;
        }

        [Fact]
        public void GetSubject_ExistingId_ReturnsSubjectVm()
        {
            var result = _subjectService.GetSubject(1);
            Assert.NotNull(result);
            Assert.Equal("Matematyka", result.Name);
        }

        [Fact]
        public void GetSubject_NonExistingId_ReturnsNull()
        {
            var result = _subjectService.GetSubject(999);
            Assert.Null(result);
        }

        [Fact]
        public void GetSubjects_ReturnsAll()
        {
            var result = _subjectService.GetSubjects();
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public void AddOrUpdateSubject_NewSubject_AddsSubject()
        {
            var vm = new AddOrUpdateSubjectVm { Name = "Chemia", TeacherId = 1 };
            var result = _subjectService.AddOrUpdateSubject(vm);
            Assert.NotNull(result);
            Assert.Equal("Chemia", result.Name);
        }

        [Fact]
        public void AddOrUpdateSubject_ExistingSubject_UpdatesName()
        {
            var vm = new AddOrUpdateSubjectVm { Id = 1, Name = "Matematyka zaawansowana", TeacherId = 1 };
            var result = _subjectService.AddOrUpdateSubject(vm);
            Assert.Equal("Matematyka zaawansowana", result.Name);
        }
    }
}
