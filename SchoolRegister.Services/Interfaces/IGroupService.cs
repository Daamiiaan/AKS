using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces
{
    public interface IGroupService
    {
        GroupVm? GetGroup(int id);
        IEnumerable<GroupVm> GetGroups(Func<GroupVm, bool>? filterPredicate = null);
        GroupVm AddOrUpdateGroup(AddOrUpdateGroupVm addOrUpdateGroupVm);
        GroupVm AttachStudentToGroup(AttachDetachStudentToGroupVm attachDetachStudentToGroupVm);
        GroupVm DetachStudentFromGroup(AttachDetachStudentToGroupVm attachDetachStudentToGroupVm);
        GroupVm AttachSubjectToGroup(AttachDetachSubjectGroupVm attachDetachSubjectGroupVm);
        GroupVm DetachSubjectFromGroup(AttachDetachSubjectGroupVm attachDetachSubjectGroupVm);
    }
}
