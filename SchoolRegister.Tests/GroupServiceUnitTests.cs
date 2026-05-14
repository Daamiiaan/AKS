using SchoolRegister.DAL.EF;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using Xunit;

namespace SchoolRegister.Tests
{
    public class GroupServiceUnitTests : BaseUnitTests
    {
        private readonly IGroupService _groupService;

        public GroupServiceUnitTests(ApplicationDbContext dbContext, IGroupService groupService)
            : base(dbContext)
        {
            _groupService = groupService;
        }

        [Fact]
        public void GetGroup_ExistingId_ReturnsGroupVm()
        {
            var result = _groupService.GetGroup(1);
            Assert.NotNull(result);
            Assert.Equal("3A", result.Name);
        }

        [Fact]
        public void GetGroups_ReturnsAll()
        {
            var result = _groupService.GetGroups();
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void AddOrUpdateGroup_NewGroup_AddsGroup()
        {
            var vm = new AddOrUpdateGroupVm { Name = "4C" };
            var result = _groupService.AddOrUpdateGroup(vm);
            Assert.NotNull(result);
            Assert.Equal("4C", result.Name);
        }

        [Fact]
        public void AttachStudentToGroup_ValidIds_StudentInGroup()
        {
            // Student 5 is in group 1 from seed; move her to group 2
            var vm = new AttachDetachStudentToGroupVm { StudentId = 5, GroupId = 2 };
            var result = _groupService.AttachStudentToGroup(vm);
            Assert.NotNull(result);
        }

        [Fact]
        public void AttachSubjectToGroup_ValidIds_SubjectInGroup()
        {
            var vm = new AttachDetachSubjectGroupVm { SubjectId = 3, GroupId = 1 };
            var result = _groupService.AttachSubjectToGroup(vm);
            Assert.NotNull(result);
        }
    }
}
