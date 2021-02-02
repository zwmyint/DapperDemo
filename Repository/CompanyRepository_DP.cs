using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using DapperDemo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DapperDemo.Repository
{
    public class CompanyRepository_DP : ICompanyRepository
    {
        // Dapper
        private IDbConnection _db;

        public CompanyRepository_DP(IConfiguration configuration)
        {
            this._db = new SqlConnection(configuration.GetConnectionString("DBDefaultConnection"));
        }

        //
        public Company Add(Company company)
        {
            var sql = "INSERT INTO tbl_Companies (Name, Address, City, State, PostalCode) "
                        + "VALUES (@Name, @Address, @City, @State, @PostalCode); "
                        + "SELECT CAST(SCOPE_IDENTITY() as int); ";
            var id = _db.Query<int>(sql,company).Single();
            company.CompanyId = id;
            return company;
        }

        public Company Find(int id)
        {
            var sql = "SELECT * FROM tbl_Companies WHERE CompanyId = @CompanyId";
            return _db.Query<Company>(sql, new { @CompanyId = id }).Single();
        }

        public List<Company> GetAll()
        {
            var sql = "SELECT * FROM tbl_Companies";
            return _db.Query<Company>(sql).ToList();
        }

        public void Remove(int id)
        {
            var sql = "DELETE FROM tbl_Companies WHERE CompanyId = @Id";
            _db.Execute(sql, new { id });
        }

        public Company Update(Company company)
        {
            var sql = "UPDATE tbl_Companies SET Name = @Name, Address = @Address, City = @City, " +
                "State = @State, PostalCode = @PostalCode WHERE CompanyId = @CompanyId";
            _db.Execute(sql, company);
            return company;
        }
    }
}