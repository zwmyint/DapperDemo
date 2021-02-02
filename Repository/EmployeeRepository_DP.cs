using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DapperDemo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DapperDemo.Repository
{
    public class EmployeeRepository_DP : IEmployeeRepository
    {
        // Dapper
        private IDbConnection _db;

        public EmployeeRepository_DP(IConfiguration configuration)
        {
            this._db = new SqlConnection(configuration.GetConnectionString("DBDefaultConnection"));
        }
        
        public Employee Add(Employee employee)
        {
            var sql = "INSERT INTO tbl_Employees (Name, Title, Email, Phone, CompanyId) "
                        +"VALUES(@Name, @Title, @Email, @Phone, @CompanyId); "
                        +"SELECT CAST(SCOPE_IDENTITY() as int); ";
            var id = _db.Query<int>(sql, employee).Single();
            employee.EmployeeId = id;
            return employee;
        }

        public Employee Find(int id)
        {
            var sql = "SELECT * FROM tbl_Employees WHERE EmployeeId = @Id";
            return _db.Query<Employee>(sql, new { @Id = id }).Single();
        }

        public List<Employee> GetAll()
        {
            var sql = "SELECT * FROM tbl_Employees";
            return _db.Query<Employee>(sql).ToList();
        }

        public void Remove(int id)
        {
            var sql = "DELETE FROM tbl_Employees WHERE EmployeeId = @Id";
            _db.Execute(sql, new { id });
        }

        public Employee Update(Employee employee)
        {
            var sql = "UPDATE tbl_Employees SET Name = @Name, Title = @Title, "
                        +"Email = @Email, Phone = @Phone, CompanyId = @CompanyId "
                        +"WHERE EmployeeId = @EmployeeId";
            _db.Execute(sql, employee);
            return employee;
        }

        //
    }
}