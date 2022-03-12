using Dapper;
using DapperApp.Context;
using DapperApp.Contracts;
using DapperApp.Dto;
using DapperApp.Entities;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DapperApp.Repository
{
    public class EmployeeRepository : IEmpRepository
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
                    var query = "INSERT INTO Employees(Name,Age,Position,CompanyId)VALUES(@Name,@Age,@Position,@CompanyId);" +
                              "SELECT CAST(SCOPE_IDENTITY() AS INT)";

                    var parameters = new DynamicParameters();
                    parameters.Add("Name", employee.Name, DbType.String);
                    parameters.Add("Age", employee.Age, DbType.String);
                    parameters.Add("Position", employee.Position, DbType.String);
                    parameters.Add("CompanyId", employee.CompanyId, DbType.Int32);
                    var id = await connection.QuerySingleAsync<int>(query, parameters);
                    var createdEmployee = new Employee
                    {
                        Id = id,
                        Name = employee.Name,
                        Age = employee.Age,
                        Position = employee.Position,
                        CompanyId = employee.CompanyId,
                    };
                    return createdEmployee;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<Employee> CreateEmployee(Employee employee)
        {
            using (var connection = _context.CreateConnection())
            {
                try
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    using (var tran = connection.BeginTransaction())
                    {
                        try
                        {
                            var parameters = new DynamicParameters();
                            parameters.Add("@Id", 0, DbType.Int32, direction: ParameterDirection.Output);
                            parameters.Add("@message", "", DbType.String, direction: ParameterDirection.Output);
                            parameters.Add("Name", employee.Name, DbType.String);
                            parameters.Add("Age", employee.Age, DbType.String);
                            parameters.Add("Position", employee.Position, DbType.String);
                            parameters.Add("CompanyId", employee.CompanyId, DbType.Int32);
                            connection.Execute("usp_AddEmployee", parameters, commandType: CommandType.StoredProcedure, transaction: tran);
                            tran.Commit();
                            employee.Id = parameters.Get<int>("Id");
                            var message = parameters.Get<string>("message");
                            return employee;
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            throw ex;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public string CreateEmployeeList1(List<Employee> employee)
        {
            using (var connection = _context.CreateConnection())
            {
                try
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    using (var tran = connection.BeginTransaction())
                    {
                        try
                        {
                            var message = "";
                            foreach (var item in employee)
                            {
                                var parameters = new DynamicParameters();
                                parameters.Add("@Id", 0, DbType.Int32, direction: ParameterDirection.Output);
                                parameters.Add("@message", "", DbType.String, direction: ParameterDirection.Output);
                                parameters.Add("Name", item.Name, DbType.String);
                                parameters.Add("Age", item.Age, DbType.String);
                                parameters.Add("Position", item.Position, DbType.String);
                                parameters.Add("CompanyId", item.CompanyId, DbType.Int32);
                                connection.Execute("usp_AddEmployee", parameters, commandType: CommandType.StoredProcedure, transaction: tran);
                                message = parameters.Get<string>("message");
                            }
                            tran.Commit();

                            return message;

                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            throw ex;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public string CreateEmployeeList(List<Employee> employee)
        {
            using (var connection = _context.CreateConnection())
            {
                try
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    using (var tran = connection.BeginTransaction())
                    {
                        try
                        {
                            string processQuery = "INSERT INTO Employees(Name,Age,Position,CompanyId)VALUES(@Name,@Age,@Position,@CompanyId)";
                            connection.Execute(processQuery, employee,transaction:tran);
                            tran.Commit();
                            return "";
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            throw ex;
                        }
                    }
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
                    var sql = "select * from Employees where Id=@Id";
                    return connection.Query<Employee>(sql, new { @Id = id }).Single();
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
                    var sql = "select * from EmplEmployeesoyee";
                    return connection.Query<Employee>(sql).ToList();
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
                try
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    using var tran = connection.BeginTransaction();
                    try
                    {
                        var sql = "delete from employees where id=@id";
                        connection.Execute(sql, new { id });
                        tran.Commit();

                        return id;

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

        }
    }
}
