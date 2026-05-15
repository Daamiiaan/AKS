using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public SubjectVm AddOrUpdateSubject(AddOrUpdateSubjectVm addOrUpdateVm)
        {
            try
            {
                if (addOrUpdateVm == null)
                    throw new ArgumentNullException("View model parameter is null");

                Subject subjectEntity;

                if (!addOrUpdateVm.Id.HasValue || addOrUpdateVm.Id == 0)
                {
                    // Create new subject
                    subjectEntity = Mapper.Map<Subject>(addOrUpdateVm);
                    DbContext.Subjects.Add(subjectEntity);
                }
                else
                {
                    // Update existing subject (find-and-modify to preserve navigation props)
                    subjectEntity = DbContext.Subjects
                        .FirstOrDefault(s => s.Id == addOrUpdateVm.Id.Value)
                        ?? throw new ArgumentException($"Subject with id {addOrUpdateVm.Id} not found.");

                    subjectEntity.Name = addOrUpdateVm.Name;
                    subjectEntity.Description = addOrUpdateVm.Description;
                    subjectEntity.TeacherId = addOrUpdateVm.TeacherId;
                }

                DbContext.SaveChanges();
                var subjectVm = Mapper.Map<SubjectVm>(subjectEntity);
                return subjectVm;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public SubjectVm GetSubject(Expression<Func<Subject, bool>> filterExpression)
        {
            try
            {
                if (filterExpression == null)
                    throw new ArgumentNullException("FilterExpression is null");

                var subjectEntity = DbContext.Subjects.FirstOrDefault(filterExpression);
                return Mapper.Map<SubjectVm>(subjectEntity);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public IEnumerable<SubjectVm> GetSubjects(Expression<Func<Subject, bool>> filterExpression = null)
        {
            try
            {
                var subjectEntities = DbContext.Subjects.AsQueryable();
                if (filterExpression != null)
                    subjectEntities = subjectEntities.Where(filterExpression);

                return Mapper.Map<IEnumerable<SubjectVm>>(subjectEntities.ToList());
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
