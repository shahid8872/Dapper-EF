using DapperApp.Entities;

namespace DapperApp.Contracts
{
    public interface IComRepository
    {
        Company Find(int id);
        List<Company> GetAll();
        Company AddCompany(Company company);
        Company Update(Company company);
        int Remove(int id);
    }
}
