using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.ConcreteServices
{
    public class GradeService : BaseService, IGradeService
    {
        public GradeService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger)
            : base(dbContext, mapper, logger) { }

        public GradeVm AddGradeToStudent(AddGradeToStudentVm vm)
        {
            var teacher = DbContext.Users.OfType<Teacher>().FirstOrDefault(t => t.Id == vm.TeacherId)
                ?? throw new ArgumentException($"Teacher with id {vm.TeacherId} not found.");

            var subject = DbContext.Subjects.FirstOrDefault(s => s.Id == vm.SubjectId && s.TeacherId == vm.TeacherId)
                ?? throw new ArgumentException($"Teacher {vm.TeacherId} does not teach subject {vm.SubjectId}.");

            var student = DbContext.Users.OfType<Student>().FirstOrDefault(s => s.Id == vm.StudentId)
                ?? throw new ArgumentException($"Student with id {vm.StudentId} not found.");

            var grade = new Grade
            {
                DateOfIssue = DateTime.Now,
                GradeValue = vm.GradeValue,
                SubjectId = vm.SubjectId,
                StudentId = vm.StudentId
            };

            DbContext.Grades.Add(grade);
            DbContext.SaveChanges();
            return Mapper.Map<GradeVm>(grade);
        }

        public GradesReportVm? GetGradesReportForStudent(GetGradesReportVm vm)
        {
            Student? student = null;

            if (vm.StudentId.HasValue)
            {
                student = DbContext.Users.OfType<Student>().FirstOrDefault(s => s.Id == vm.StudentId.Value);
            }
            else if (vm.ParentId.HasValue)
            {
                student = DbContext.Users.OfType<Student>().FirstOrDefault(s => s.ParentId == vm.ParentId.Value);
            }

            if (student == null) return null;

            var grades = DbContext.Grades.Where(g => g.StudentId == student.Id).ToList();

            return new GradesReportVm
            {
                StudentId = student.Id,
                StudentFirstName = student.FirstName,
                StudentLastName = student.LastName,
                Grades = Mapper.Map<IList<GradeVm>>(grades)
            };
        }
    }
}
