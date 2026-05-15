using SchoolRegister.DAL.EF;
using SchoolRegister.Tests; // SeedData extension method

namespace SchoolRegister.Tests.UnitTests
{
    public abstract class BaseUnitTests
    {
        protected readonly ApplicationDbContext DbContext;

        protected BaseUnitTests(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
            // SeedData() has a guard inside: if data already exists it returns immediately.
            // This prevents duplicate-key errors when multiple test classes share one DB.
            DbContext.SeedData();
        }
    }
}
