using Dapper.Contrib.Extensions;
using DapperApp.Contracts;
using DapperApp.Data;
using DapperApp.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

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

        

    }
}
