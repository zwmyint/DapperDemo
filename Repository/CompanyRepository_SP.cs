using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using DapperDemo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DapperDemo.Repository
{
    public class CompanyRepository_SP : ICompanyRepository
    {
        // SP
        private IDbConnection _db;

        public CompanyRepository_SP(IConfiguration configuration)
        {
            this._db = new SqlConnection(configuration.GetConnectionString("DBDefaultConnection"));
        }

        //
        public Company Add(Company company)
        {

            var parameters = new DynamicParameters();
                parameters.Add("@CompanyId", 0, DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("@Name", company.Name);
                parameters.Add("@Address", company.Address);
                parameters.Add("@City", company.City);
                parameters.Add("@State", company.State);
                parameters.Add("@PostalCode", company.PostalCode);
            _db.Execute("sp_AddCompany", parameters, commandType: CommandType.StoredProcedure);

            company.CompanyId = parameters.Get<int>("CompanyId");

            return company;
        }

        public Company Find(int id)
        {
            return _db.Query<Company>("sp_GetCompany", new { CompanyId = id }, commandType: CommandType.StoredProcedure).SingleOrDefault();
        }

        public List<Company> GetAll()
        {
            return _db.Query<Company>("sp_GetAllCompany", commandType: CommandType.StoredProcedure).ToList();
        }

        public void Remove(int id)
        {
            _db.Execute("sp_RemoveCompany", new { CompanyId = id }, commandType: CommandType.StoredProcedure);
        }

        public Company Update(Company company)
        {

             var parameters = new DynamicParameters();
                parameters.Add("@CompanyId", company.CompanyId, DbType.Int32);
                parameters.Add("@Name", company.Name);
                parameters.Add("@Address", company.Address);
                parameters.Add("@City", company.City);
                parameters.Add("@State", company.State);
                parameters.Add("@PostalCode", company.PostalCode);

            _db.Execute("sp_UpdateCompany", parameters, commandType: CommandType.StoredProcedure);

            return company;
        }
    }
}