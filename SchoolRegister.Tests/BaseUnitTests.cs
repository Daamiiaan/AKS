using SchoolRegister.DAL.EF;

namespace SchoolRegister.Tests
{
    public abstract class BaseUnitTests
    {
        protected readonly ApplicationDbContext DbContext;

        protected BaseUnitTests(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
            DbContext.SeedData();
        }
    }
}
