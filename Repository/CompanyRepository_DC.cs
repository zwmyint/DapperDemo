using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Dapper.Contrib.Extensions;
using DapperDemo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DapperDemo.Repository
{
    public class CompanyRepository_DC : ICompanyRepository
    {
        // Dapper.Contrib
        private IDbConnection _db;

        public CompanyRepository_DC(IConfiguration configuration)
        {
            this._db = new SqlConnection(configuration.GetConnectionString("DBDefaultConnection"));
        }

        //
        public Company Add(Company company)
        {
            var id = _db.Insert(company); // Dapper.Contrib.Extensions
            company.CompanyId = (int)id;
            return company;
        }

        public Company Find(int id)
        {
            return _db.Get<Company>(id); // Dapper.Contrib.Extensions
        }

        public List<Company> GetAll()
        {
            return _db.GetAll<Company>().ToList(); // Dapper.Contrib.Extensions

        }

        public void Remove(int id)
        {
            _db.Delete(new Company { CompanyId = id }); // Dapper.Contrib.Extensions
        }

        public Company Update(Company company)
        {

            _db.Update(company); // Dapper.Contrib.Extensions
            return company;
        }
    }
}