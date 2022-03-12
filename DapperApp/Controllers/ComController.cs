using Dapper.Contrib.Extensions;
using DapperApp.Contracts;
using DapperApp.Data;
using DapperApp.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace DapperApp.Controllers
{
    [Route("api/com")]
    [ApiController]
    public class ComController : ControllerBase
    {
        private readonly IComRepository _empRepo;

        public ComController(IComRepository employeeRep)
        {
            _empRepo = employeeRep;


        }

        [HttpPost(Name ="createcom")]
        public Company CreateCompany([FromBody]Company company)
        {
            try
            {

                return _empRepo.AddCompany(company);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("{id}", Name = "ComById")]
        public Company Find(int id)
        {
            try
            {
                return _empRepo.Find(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet]
        public List<Company> GetAll()
        {
            try
            {
                List<Company> companies = _empRepo.GetAll();
                return companies;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpDelete]
        public int Remove(int id)
        {
            try
            {
                int deletedid = _empRepo.Remove(id);
                return deletedid;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPut]
        public Company Update(Company company)
        {
            try
            {
                Company com = _empRepo.Update(company);
                return com;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
