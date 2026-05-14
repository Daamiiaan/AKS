using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces
{
    public interface ITeacherService
    {
        TeacherVm? GetTeacher(int id);
        IEnumerable<TeacherVm> GetTeachers(Func<TeacherVm, bool>? filterPredicate = null);
        TeacherVm AttachSubjectToTeacher(AttachDetachSubjectToTeacherVm attachDetachSubjectToTeacherVm);
        TeacherVm DetachSubjectFromTeacher(AttachDetachSubjectToTeacherVm attachDetachSubjectToTeacherVm);
        TeachersGroupsVm? GetTeachersGroups(int teacherId);
    }
}
