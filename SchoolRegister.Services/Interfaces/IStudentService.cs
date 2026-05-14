using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces
{
    public interface IStudentService
    {
        StudentVm? GetStudent(int id);
        IEnumerable<StudentVm> GetStudents(Func<StudentVm, bool>? filterPredicate = null);
    }
}
