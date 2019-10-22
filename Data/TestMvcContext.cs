using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestAuth2Mvc.Models;

    public class TestMvcContext : DbContext
    {
        public TestMvcContext (DbContextOptions<TestMvcContext> options)
            : base(options)
        {
        }

        public DbSet<MvcObject> Object { get; set; }
    }
