using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AnimeSite.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<FileModel> Files { get; set; }
    public ApplicationContext(DbContextOptions<ApplicationContext> option): base(option)
        {
            Database.EnsureCreated();
        }
    }
}
		
