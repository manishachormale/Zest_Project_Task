using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureDemo.Application.DTOs////This DTO made for showing data
{
    public class EmployeeDTO
    {
        public string Name { get; set; }
        public double Salary { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }//for role based autho
    }
}
