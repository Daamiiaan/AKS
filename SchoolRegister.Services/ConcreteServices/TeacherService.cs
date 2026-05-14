using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.ConcreteServices
{
    public class TeacherService : BaseService, ITeacherService
    {
        public TeacherService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger)
            : base(dbContext, mapper, logger) { }

        public TeacherVm? GetTeacher(int id)
        {
            var teacher = DbContext.Users.OfType<Teacher>().FirstOrDefault(t => t.Id == id);
            return teacher == null ? null : Mapper.Map<TeacherVm>(teacher);
        }

        public IEnumerable<TeacherVm> GetTeachers(Func<TeacherVm, bool>? filterPredicate = null)
        {
            var teachers = DbContext.Users.OfType<Teacher>().ToList();
            var teacherVms = Mapper.Map<IEnumerable<TeacherVm>>(teachers);
            return filterPredicate == null ? teacherVms : teacherVms.Where(filterPredicate);
        }

        public TeacherVm AttachSubjectToTeacher(AttachDetachSubjectToTeacherVm vm)
        {
            var teacher = DbContext.Users.OfType<Teacher>().FirstOrDefault(t => t.Id == vm.TeacherId)
                ?? throw new ArgumentException($"Teacher with id {vm.TeacherId} not found.");
            var subject = DbContext.Subjects.FirstOrDefault(s => s.Id == vm.SubjectId)
                ?? throw new ArgumentException($"Subject with id {vm.SubjectId} not found.");

            subject.TeacherId = teacher.Id;
            DbContext.SaveChanges();
            return Mapper.Map<TeacherVm>(teacher);
        }

        public TeacherVm DetachSubjectFromTeacher(AttachDetachSubjectToTeacherVm vm)
        {
            var teacher = DbContext.Users.OfType<Teacher>().FirstOrDefault(t => t.Id == vm.TeacherId)
                ?? throw new ArgumentException($"Teacher with id {vm.TeacherId} not found.");
            var subject = DbContext.Subjects.FirstOrDefault(s => s.Id == vm.SubjectId && s.TeacherId == vm.TeacherId)
                ?? throw new ArgumentException($"Subject with id {vm.SubjectId} is not assigned to teacher {vm.TeacherId}.");

            subject.TeacherId = null;
            DbContext.SaveChanges();
            return Mapper.Map<TeacherVm>(teacher);
        }

        public TeachersGroupsVm? GetTeachersGroups(int teacherId)
        {
            var teacher = DbContext.Users.OfType<Teacher>().FirstOrDefault(t => t.Id == teacherId);
            if (teacher == null) return null;

            var subjects = DbContext.Subjects.Where(s => s.TeacherId == teacherId).ToList();
            var subjectIds = subjects.Select(s => s.Id).ToList();

            var groups = DbContext.Groups
                .Where(g => g.SubjectGroups.Any(sg => subjectIds.Contains(sg.SubjectId)))
                .ToList();

            return new TeachersGroupsVm
            {
                TeacherId = teacher.Id,
                TeacherName = $"{teacher.FirstName} {teacher.LastName}",
                Groups = Mapper.Map<IList<GroupVm>>(groups)
            };
        }
    }
}
