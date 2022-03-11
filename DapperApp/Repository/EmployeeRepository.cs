using Dapper;
using DapperApp.Context;
using DapperApp.Contracts;
using DapperApp.Dto;
using DapperApp.Entities;
using System.Data;

namespace DapperApp.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DapperContext _context;

        public EmployeeRepository(DapperContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            var query = "SELECT * FROM Employees";

            using (var connection = _context.CreateConnection())
            {
                var companies = await connection.QueryAsync<Employee>(query);
                return companies.ToList();
            }
        }
        public string InsertEmployee(EmployeeForCreationDto model)
        {
            using (var connection = _context.CreateConnection())
            {
                try
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    using var tran = connection.BeginTransaction();
                    try
                    {
                        var employee = connection.Query<List<EmployeeForCreationDto>>("InsertEmployee", model, commandType: CommandType.StoredProcedure);
                        var topRow = employee.FirstOrDefault();
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        throw ex;
                    }
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }


            }
            return "";
        }

    }
}
