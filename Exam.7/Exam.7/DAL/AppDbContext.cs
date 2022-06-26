using Exam._7.Models;
using Microsoft.EntityFrameworkCore;

namespace Exam._7.DAL
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }
        public DbSet<Team> Teams { get; set; }
    }
}
