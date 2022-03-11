using DapperApp.Contracts;
using DapperApp.Data;
using DapperApp.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DapperApp.Controllers
{
    [Route("api/emp")]
    [ApiController]
    public class EmpController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepo;
        ApplicationDbContext _dbContext;
        public EmpController(IEmployeeRepository employeeRep, ApplicationDbContext applicationDbContext)
        {
            _employeeRepo = employeeRep;
            _dbContext = applicationDbContext;

        }
     
        [HttpGet]

        public IEnumerable<Employee> GetAll()
        {
            try
            {
                return _dbContext.Employees.ToList();
            }
            catch (Exception ee)
            {
                throw;
            }
        }

    }
}
