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

        //[HttpPost]
        //public async Task<IActionResult> CreateCompany(Employee employee)
        //{
           
        //    try
        //    {
        //        var createdEmployee = await _employeeRepo.CreateEmployee(employee);
        //        return CreatedAtRoute("EmployeeId", new { id = employee.Id }, createdEmployee);
        //    }
        //    catch (Exception ex)
        //    {
        //        //log error
        //        return StatusCode(500, ex.Message);
        //    }

        //}

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
