using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.ConcreteServices;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.Services.Mapper;

namespace SchoolRegister.Tests
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Each test gets its own isolated InMemory database so that
            // mutating tests (AddGroup, AttachStudent, …) never bleed into
            // read-only tests (GetAllGroups, …). Xunit.DependencyInjection v9
            // creates a new DI scope (and therefore a new DbContext instance)
            // for every test method, so the Guid lambda runs fresh each time.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase(Guid.NewGuid().ToString())
                       .UseLazyLoadingProxies());

            // AutoMapper
            services.AddAutoMapper(typeof(MainProfile));

            // ASP.NET Identity – needed for UserManager<User> injection in services
            services.AddIdentity<User, Role>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddRoleManager<RoleManager<Role>>()
            .AddUserManager<UserManager<User>>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            // Logger
            services.AddLogging(cfg => cfg.AddConsole());
            services.AddTransient(typeof(ILogger), typeof(Logger<Startup>));

            // Services under test
            services.AddTransient<ISubjectService, SubjectService>();
            services.AddTransient<IGradeService, GradeService>();
            services.AddTransient<IGroupService, GroupService>();
            services.AddTransient<IStudentService, StudentService>();
            services.AddTransient<ITeacherService, TeacherService>();
        }
    }
}
