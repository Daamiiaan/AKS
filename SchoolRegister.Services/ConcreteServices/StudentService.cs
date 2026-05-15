using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
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

        public StudentVm GetStudent(Expression<Func<Student, bool>> filterPredicate)
        {
            try
            {
                if (filterPredicate == null)
                    throw new ArgumentNullException("FilterPredicate is null");

                var student = DbContext.Users.OfType<Student>()
                    .Include(s => s.Group)
                    .Include(s => s.Parent)
                    .FirstOrDefault(filterPredicate);
                return Mapper.Map<StudentVm>(student);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public IEnumerable<StudentVm> GetStudents(Expression<Func<Student, bool>> filterPredicate = null)
        {
            try
            {
                var students = DbContext.Users.OfType<Student>()
                    .Include(s => s.Group)
                    .Include(s => s.Parent)
                    .AsQueryable();
                if (filterPredicate != null)
                    students = students.Where(filterPredicate);

                return Mapper.Map<IEnumerable<StudentVm>>(students.ToList());
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
