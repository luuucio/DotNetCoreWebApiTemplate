using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiTemplate.Entities;

namespace WebApiTemplate.DbContexts
{
    public class WebApiTemplateContext : DbContext
    {
        public DbSet<Student> Students { get; set; }

        // Constructor needed to receive a configuration        
        public WebApiTemplateContext(DbContextOptions<WebApiTemplateContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Student>().HasIndex(student => student.LastName);
        }
    }
}
