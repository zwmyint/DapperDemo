using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using DapperDemo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DapperDemo.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private IDbConnection db;

        public EmployeeRepository(IConfiguration configuration)
        {
            this.db = new SqlConnection(configuration.GetConnectionString("DBDefaultConnection"));
        }
        
        public Employee Add(Employee employee)
        {
            throw new System.NotImplementedException();
        }

        public Task<Employee> AddAsync(Employee employee)
        {
            throw new System.NotImplementedException();
        }

        public Employee Find(int id)
        {
            throw new System.NotImplementedException();
        }

        public List<Employee> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new System.NotImplementedException();
        }

        public Employee Update(Employee employee)
        {
            throw new System.NotImplementedException();
        }
    }
}