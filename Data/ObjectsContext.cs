using Microsoft.EntityFrameworkCore;
using TestAuth2Mvc.Models;

namespace TestAuth2Mvc.Data
{
    public class TestAuth2MvcContext : DbContext
    {
        public TestAuth2MvcContext (DbContextOptions<TestAuth2MvcContext> options)
            : base(options)
        {
        }

        public DbSet<MvcObject> MvcObject { get; set; }
    }
}