using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces
{
    public interface ISubjectService
    {
        SubjectVm? GetSubject(int id);
        IEnumerable<SubjectVm> GetSubjects(Func<SubjectVm, bool>? filterPredicate = null);
        SubjectVm AddOrUpdateSubject(AddOrUpdateSubjectVm addOrUpdateSubjectVm);
    }
}
