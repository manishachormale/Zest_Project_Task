using ZestTrainingFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using ZestTrainingFlow.Infrastructure.Data;


namespace ZestTrainingFlow.Infrastructure.Data 
{
    public class MyDbContext:DbContext//this is main class in EF used for connect application with DB 
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
            
        }
        public DbSet<Employee> Employees{ get; set; }
        public DbSet<Department>Departments{ get; set; }
        //public DbSet <Employee>Employees   { get; set; }
        public DbSet<UserCQRS> User{ get; set; }

    }
    // i have stored this file in Model DB
}
