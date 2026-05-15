using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.ConcreteServices
{
    public class TeacherService : BaseService, ITeacherService
    {
        private readonly UserManager<User> _userManager;

        public TeacherService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger,
            UserManager<User> userManager) : base(dbContext, mapper, logger)
        {
            _userManager = userManager;
        }

        public TeacherVm GetTeacher(Expression<Func<Teacher, bool>> filterPredicate)
        {
            try
            {
                if (filterPredicate == null)
                    throw new ArgumentNullException("FilterPredicate is null");

                var teacher = DbContext.Users.OfType<Teacher>().FirstOrDefault(filterPredicate);
                return Mapper.Map<TeacherVm>(teacher);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public IEnumerable<TeacherVm> GetTeachers(Expression<Func<Teacher, bool>> filterPredicate = null)
        {
            try
            {
                var teachers = DbContext.Users.OfType<Teacher>().AsQueryable();
                if (filterPredicate != null)
                    teachers = teachers.Where(filterPredicate);

                return Mapper.Map<IEnumerable<TeacherVm>>(teachers.ToList());
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public IEnumerable<GroupVm> GetTeachersGroups(TeachersGroupsVm getTeachersGroups)
        {
            try
            {
                if (getTeachersGroups == null)
                    throw new ArgumentNullException("ViewModel is null");

                // For each subject taught by this teacher, add all groups assigned to that subject
                var result = new List<GroupVm>();
                var subjects = DbContext.Subjects
                    .Where(s => s.TeacherId == getTeachersGroups.TeacherId)
                    .ToList();

                foreach (var subject in subjects)
                {
                    var groups = DbContext.SubjectGroups
                        .Where(sg => sg.SubjectId == subject.Id)
                        .Select(sg => sg.Group)
                        .ToList();

                    result.AddRange(Mapper.Map<IEnumerable<GroupVm>>(groups));
                }

                return result;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
