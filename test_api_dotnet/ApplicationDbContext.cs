using System;
using Microsoft.EntityFrameworkCore;

namespace test_api_dotnet
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Notes> Notes { get; set; }
    }
}
