
namespace ZestTrainingFlow.Application.DTOs
{
    public class EmployeeDTO//This DTO made for showing data
    {
        public string Name { get; set; }
        public double Salary { get; set; }
        public int DepartmentId {  get; set; }
        public string DepartmentName {  get; set; }
        public string Email {  get; set; }
        public string Password { get; set; }
        public string Role {  get; set; }//for role based autho

    }
}
