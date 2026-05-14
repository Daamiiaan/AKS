using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Services.ConcreteServices;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.Services.Mapper;

namespace SchoolRegister.Tests
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // InMemory database for tests (each test run gets a fresh DB)
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()));

            // AutoMapper
            services.AddAutoMapper(typeof(MainProfile));

            // Logger
            services.AddLogging(cfg => cfg.AddConsole());
            services.AddTransient(typeof(ILogger), typeof(Logger<Startup>));

            // Services under test
            services.AddScoped<ISubjectService, SubjectService>();
            services.AddScoped<ITeacherService, TeacherService>();
            services.AddScoped<IGradeService, GradeService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IStudentService, StudentService>();
        }
    }
}
