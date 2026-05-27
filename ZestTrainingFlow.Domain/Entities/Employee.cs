
using ZestTrainingFlow.Domain.Entities;
//namespace ZestTask.Model
namespace ZestTrainingFlow.Domain.Entities
{
    public class Employee
    {
        public int Id {  get; set; }
        public string Name { get; set; }
        public double Salary {  get; set; }
        public int DepartmentId { get; set; } // foreign key 
         public Department Department { get; set; } // Navigation property 
        //navigation properties are used to access related table data 

        public string Email {  get; set; }
        public string Password { get; set; }
        public string Role {  get; set; } // i added his property for role based authorization

    }
}
