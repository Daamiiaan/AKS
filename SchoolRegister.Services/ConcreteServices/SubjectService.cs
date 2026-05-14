using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.ConcreteServices
{
    public class SubjectService : BaseService, ISubjectService
    {
        public SubjectService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger)
            : base(dbContext, mapper, logger) { }

        public SubjectVm? GetSubject(int id)
        {
            var subject = DbContext.Subjects.FirstOrDefault(s => s.Id == id);
            return subject == null ? null : Mapper.Map<SubjectVm>(subject);
        }

        public IEnumerable<SubjectVm> GetSubjects(Func<SubjectVm, bool>? filterPredicate = null)
        {
            var subjects = DbContext.Subjects.ToList();
            var subjectVms = Mapper.Map<IEnumerable<SubjectVm>>(subjects);
            return filterPredicate == null ? subjectVms : subjectVms.Where(filterPredicate);
        }

        public SubjectVm AddOrUpdateSubject(AddOrUpdateSubjectVm addOrUpdateSubjectVm)
        {
            if (addOrUpdateSubjectVm.Id.HasValue)
            {
                var existing = DbContext.Subjects.FirstOrDefault(s => s.Id == addOrUpdateSubjectVm.Id.Value);
                if (existing == null)
                    throw new ArgumentException($"Subject with id {addOrUpdateSubjectVm.Id} not found.");

                existing.Name = addOrUpdateSubjectVm.Name;
                existing.Description = addOrUpdateSubjectVm.Description;
                existing.TeacherId = addOrUpdateSubjectVm.TeacherId;
                DbContext.SaveChanges();
                return Mapper.Map<SubjectVm>(existing);
            }
            else
            {
                var newSubject = Mapper.Map<Subject>(addOrUpdateSubjectVm);
                DbContext.Subjects.Add(newSubject);
                DbContext.SaveChanges();
                return Mapper.Map<SubjectVm>(newSubject);
            }
        }
    }
}
