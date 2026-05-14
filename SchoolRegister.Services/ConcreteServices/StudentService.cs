using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.ConcreteServices
{
    public class StudentService : BaseService, IStudentService
    {
        public StudentService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger)
            : base(dbContext, mapper, logger) { }

        public StudentVm? GetStudent(int id)
        {
            var student = DbContext.Users.OfType<Student>().FirstOrDefault(s => s.Id == id);
            return student == null ? null : Mapper.Map<StudentVm>(student);
        }

        public IEnumerable<StudentVm> GetStudents(Func<StudentVm, bool>? filterPredicate = null)
        {
            var students = DbContext.Users.OfType<Student>().ToList();
            var studentVms = Mapper.Map<IEnumerable<StudentVm>>(students);
            return filterPredicate == null ? studentVms : studentVms.Where(filterPredicate);
        }
    }
}
