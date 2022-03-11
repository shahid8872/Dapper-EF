using DapperApp.Contracts;
using DapperApp.Data;
using DapperApp.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DapperApp.Controllers
{
    [Route("api/EmployeeEF")]
    [ApiController]
    public class EmployeeEFController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepo;
        ApplicationDbContext _dbContext;
        public EmployeeEFController(IEmployeeRepository employeeRep, ApplicationDbContext applicationDbContext)
        {
            _employeeRepo = employeeRep;
            _dbContext = applicationDbContext;

        }
        public IEnumerable<Employee> GetEmployees()
        {
            try
            {
                return _dbContext.Employees.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
