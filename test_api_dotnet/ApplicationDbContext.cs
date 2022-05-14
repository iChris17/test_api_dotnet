using System;
using Microsoft.EntityFrameworkCore;

namespace test_api_dotnet
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Notes>().HasKey(prop => prop.NotesId);

            modelBuilder.Entity<Notes>().Property(prop => prop.Title)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Notes>().Property(prop => prop.Body)
               .HasMaxLength(300)
               .IsRequired();

            modelBuilder.Entity<Notes>().Property(prop => prop.NoteDate)
                .HasColumnType("timestamp")
               .HasDefaultValueSql("CURRENT_TIMESTAMP")
               .IsRequired();
        }

        public DbSet<Notes> Notes { get; set; }
    }
}
