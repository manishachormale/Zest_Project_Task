using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CleanArchitectureDemo.Domain.Entities;


namespace CleanArchitectureDemo.Infrastructure.Data
{
    public class MyDbContext :DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {

        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        //public DbSet <Employee>Employees   { get; set; }
        public DbSet<UserCQRS> User { get; set; }
    }
}
