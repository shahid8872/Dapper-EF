using Dapper;
using DapperApp.Context;
using DapperApp.Contracts;
using DapperApp.Dto;
using DapperApp.Entities;
using System.Data;

namespace DapperApp.Repository
{
    public class EmployeeSPRepository
    {
        private readonly DapperContext _context;

        public EmployeeSPRepository(DapperContext context)
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
        public int CreateEmployee1(Employee employee)
        {
            using (var connection = _context.CreateConnection())
            {
                try
                {
                    var sql = "INSERT INTO Employees(Name,Age,Position,CompanyId)VALUES(@Name,@Age,@Position,@CompanyId);" +
                              "SELECT CAST(SCOPE_IDENTITY() AS INT)";
                    var id = connection.Query<int>(sql, employee).Single();
                    employee.Id = id;
                    return id;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<Employee> CreateEmployee2(Employee employee)
        {
            using (var connection = _context.CreateConnection())
            {
                try
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    using var tran = connection.BeginTransaction();
                    var parameters = new DynamicParameters();
                    parameters.Add("@Id",0,DbType.Int32,direction:ParameterDirection.Output);
                    parameters.Add("@Message",0,DbType.String,direction:ParameterDirection.Output);
                    parameters.Add("Name", employee.Name, DbType.String);
                    parameters.Add("Age", employee.Age, DbType.String);
                    parameters.Add("Position", employee.Position, DbType.String);
                    parameters.Add("CompanyId", employee.CompanyId, DbType.Int32);
                    connection.Execute("usp_AddEmployee",parameters,commandType:CommandType.StoredProcedure);
                    tran.Commit();
                    employee.Id = parameters.Get<int>("Id");      
                    var message = parameters.Get<int>("Message");
                    return employee;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public async Task<Employee> UpdateEmployee(Employee employee)
        {
            using (var connection = _context.CreateConnection())
            {
                try
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    using var tran = connection.BeginTransaction();
                    var parameters = new DynamicParameters();
                    parameters.Add("@Id", DbType.Int32);
                    parameters.Add("Name", employee.Name, DbType.String);
                    parameters.Add("Age", employee.Age, DbType.String);
                    parameters.Add("Position", employee.Position, DbType.String);
                    parameters.Add("CompanyId", employee.CompanyId, DbType.Int32);
                    connection.Execute("usp_UpdateCompany", parameters, commandType: CommandType.StoredProcedure);
                    tran.Commit();
                    return employee;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public Employee Find(int id)
        {
            using (var connection = _context.CreateConnection())
            {
                try
                {
                    return connection.Query<Employee>("usp_GetEmployee", new { Id = id }, commandType: CommandType.StoredProcedure).SingleOrDefault();
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }
        public List<Employee> GetAll()
        {
            using (var connection = _context.CreateConnection())
            {
                try
                {
                    return connection.Query<Employee>("usp_GetAllEmployee", commandType: CommandType.StoredProcedure).ToList();
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }
        public int Remove(int id)
        {
            using (var connection = _context.CreateConnection())
            {
              return  connection.Execute("usp_RemoveEmployee", new {Id=id},commandType:CommandType.StoredProcedure);
            }

        }
    }
}
