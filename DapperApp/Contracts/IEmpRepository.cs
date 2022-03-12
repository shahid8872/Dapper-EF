using DapperApp.Entities;

namespace DapperApp.Contracts
{
    public interface IEmpRepository
    {
        public Task<Employee> CreateEmployee(Employee employee);
        public string CreateEmployeeList(List<Employee> employee);
    }
}
