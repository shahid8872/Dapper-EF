using DapperApp.Contracts;
using DapperApp.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DapperApp.Controllers
{
    [Route("api/emp")]
    [ApiController]
    public class EmpController : ControllerBase
    {
        private readonly IEmpRepository _employeeRepo;
        public EmpController(IEmpRepository employeeRep)
        {
            _employeeRepo = employeeRep;
        }

        [HttpPost]
        public  string CreateCompanyList(List<Employee> employee)
        {

            try
            {
                return _employeeRepo.CreateEmployeeList(employee);               
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}
