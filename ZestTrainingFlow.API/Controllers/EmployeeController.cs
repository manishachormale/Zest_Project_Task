using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using ZestTrainingFlow.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using ZestTrainingFlow.Application.DTOs;
using ZestTrainingFlow.Domain.Entities;

namespace ZestTrainingFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly MyDbContext _context;
        //private readonly Employee _employee;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public EmployeeController(MyDbContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _configuration = configuration;//this is for JWT
            _mapper = mapper;
        }
        //Ef converts c# objects into database tables

        //get all empoyees 
        [HttpGet("GetAllEmployees")]
        public IActionResult GetAllEmployees()
        {
            var employees = _context.Employees
                .Include(x => x.Department) //include used to load related data withut include data will be null
                .Select(x => new EmployeeDTO
                {
                    Name = x.Name,
                    Salary = x.Salary,
                    DepartmentName = x.Department.DepartmentName,
                    Email = x.Email,
                    Password = x.Password,
                    Role = x.Role

                }).ToList();

            return Ok(employees);
        }

        //get employees by ID
        [HttpGet("{Id}")]
        public IActionResult GetEmployeesById(int Id)
        {
            var employee = _context.Employees
                .Include(x => x.Department)
                .Where(x => x.Id == Id)
                .Select(x => new EmployeeDTO
                {
                    Name = x.Name,
                    Salary = x.Salary,
                    DepartmentName = x.Department.DepartmentName,
                    Email = x.Email,
                    Password = x.Password,
                    Role = x.Role,
                    DepartmentId = x.DepartmentId
                }).FirstOrDefault();
            if (employee == null)
            {
                return NotFound("Employee not found");
            }
            return Ok(employee);
        }


        [HttpPost("AddDepartment")]//add only department Id And DepartmentName
        public IActionResult AddDepartment(DepartmentDTO createEmployeeDTO)
        {
            Department department = new Department()
            {
                //  Id = createEmployeeDTO.DepartmentId,
                DepartmentName = createEmployeeDTO.DepartmentName
            };
            _context.Departments.Add(department);
            _context.SaveChanges();
            return Ok("Department added successfully");
        }

        //create employees


        [HttpPost("AddEmployee")]
        public IActionResult AddEmployee(EmployeeDTO dto)
        {
            var department = _context.Departments.FirstOrDefault(d => d.DepartmentName == dto.DepartmentName);
            if (department == null)
            {
                return BadRequest();
            }
            Employee employee = new Employee()
            {
                Name = dto.Name,
                Salary = dto.Salary,
                DepartmentId = department.Id,
                Email = dto.Email,
                Password = dto.Password,
                Role = dto.Role
            };

            _context.Employees.Add(employee);
            _context.SaveChanges();
            return Ok("Employee added succesfully");
        }

        [HttpGet("GetDepartmets")]
        public IActionResult GetDepartmets(int Id)
        {
            var departments = _context.Departments
                .Where(d => d.Id == Id)// this condition gives only specific record along with Id
                .Select(d => new
                {
                    d.Id,
                    d.DepartmentName

                }).ToList();
            return Ok(departments);
        }


        [HttpPost("InsertDept")]
        public IActionResult InsertDept(Department department)
        {
            _context.Departments.Add(department);
            _context.SaveChanges();
            return Ok("Deparment added successfully");
        }


        [HttpGet("GetDepartments12")]
        public IActionResult GetDepartments12(string Name)
        {
            return Ok(_context.Departments.ToList());
        }



        [HttpGet]
        [Authorize]
        [Route("JWTtokenUsers")]
        public IActionResult JWTtokenUsers()
        {
            return Ok(_context.Employees.ToList());
        }

        [HttpPost]
        [Route("Login1")]
        public IActionResult Login1(LoginDTO loginDTO)
        {
            var user = _context.Employees.FirstOrDefault(x =>
                x.Email == loginDTO.Email &&
                x.Password == loginDTO.Password);

            if (user == null)
            {
                return Unauthorized("Invalid Email or Password");
            }

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, //this is payload
            _configuration["Jwt:Subject"]!),

        new Claim(JwtRegisteredClaimNames.Jti,
            Guid.NewGuid().ToString()),

        new Claim("UserID", user.Id.ToString()),

        new Claim("Email", user.Email!),
        new Claim("Role", user.Role!) //add this role for roled based authorization
    };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _configuration["Jwt:Key"]!)); //this is signature


            var signIn = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: signIn);

            string tokenValue = new JwtSecurityTokenHandler()
                    .WriteToken(token);

            return Ok(new
            {
                Token = tokenValue,
                UserName = user.Name,
                Email = user.Email,
                Role = user.Role
            });

        }
        //role based authorization
        [HttpPost("AddEmployeeRoleBased")]
        public IActionResult AddEmployeeRoleBased(EmployeeDTO employeeDTO)
        {
            var department = _context.Departments.FirstOrDefault(d => d.DepartmentName == employeeDTO.DepartmentName);
            if (department == null)
            {
                return BadRequest("Department not found");
            }

            Employee employee = new Employee()
            {
                Name = employeeDTO.Name,
                Salary = employeeDTO.Salary,
                DepartmentId = employeeDTO.DepartmentId,
                Email = employeeDTO.Email,
                Password = employeeDTO.Password,
                Role = employeeDTO.Role
            };

            _context.Employees.Add(employee);
            _context.SaveChanges();
            return Ok("emp added successfully ");
        }


        [Authorize(Roles = "Admin,User")]//role added here
        [HttpGet("AdminAPI")]
        public IActionResult AdminAPI()
        {
            return Ok("only admin can access");
        }

        [HttpGet("TestMidleware")]//this is for custome middleware
        public IActionResult TestMiddleware()
        {
            return Ok("Middleware executed successfully");
        }

        [HttpGet]//automapper i.e we doesnt need to write DTO.Name,DTO.Email,DTO.Phone,DTO.Address
                 //autommapper directly map  entities to DTO And DTO to entity `
        public IActionResult GetEmployeesAutoMapper()
        {
            var employees = _context.Employees.ToList();
            var employeeDTO = _mapper.Map<List<EmployeeDTO>>(employees);
            return Ok(employeeDTO);
        }

    }
}

