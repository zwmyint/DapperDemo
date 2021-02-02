using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using Dapper;
using DapperDemo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DapperDemo.Repository
{
    public class BonusRepository : IBonusRepository
    {
        private IDbConnection _db;

        public BonusRepository(IConfiguration configuration)
        {
            this._db = new SqlConnection(configuration.GetConnectionString("DBDefaultConnection"));
        }

        public void AddTestCompanyWithEmployees(Company objComp)
        {
            var sql = "INSERT INTO tbl_Companies (Name, Address, City, State, PostalCode) "
                    + "VALUES(@Name, @Address, @City, @State, @PostalCode);"
                    + "SELECT CAST(SCOPE_IDENTITY() as int); ";
            var id = _db.Query<int>(sql, objComp).Single();
            objComp.CompanyId = id;

            foreach(var employee in objComp.Employees)
            {
               employee.CompanyId = objComp.CompanyId;
               var sql1 = "INSERT INTO tbl_Employees (Name, Title, Email, Phone, CompanyId) "
                        + "VALUES(@Name, @Title, @Email, @Phone, @CompanyId);"
                        + "SELECT CAST(SCOPE_IDENTITY() as int); ";
               _db.Query<int>(sql1, employee).Single();
            }

            //
            /* // bulk insert
            objComp.Employees.Select(c => { c.CompanyId = id; return c; }).ToList();
              var sqlEmp = "INSERT INTO tbl_Employees (Name, Title, Email, Phone, CompanyId) "
                        + "VALUES(@Name, @Title, @Email, @Phone, @CompanyId); "
                        + "SELECT CAST(SCOPE_IDENTITY() as int); ";

            _db.Execute(sqlEmp, objComp.Employees); */

            //
        }

        public void AddTestCompanyWithEmployeesWithTransaction(Company objComp)
        {
            using (var transaction = new TransactionScope()) //System.Transactions.TransactionScope;
            {
                try
                {
                    var sql = "INSERT INTO tbl_Companies (Name, Address, City, State, PostalCode) "
                                + "VALUES(@Name, @Address, @City, @State, @PostalCode);"
                                + "SELECT CAST(SCOPE_IDENTITY() as int); ";
                    var id = _db.Query<int>(sql, objComp).Single();
                    objComp.CompanyId = id;

                    objComp.Employees.Select(c => { c.CompanyId = id; return c; }).ToList();
                    var sqlEmp = "INSERT INTO tbl_Employees (Name, Title, Email, Phone, CompanyId) "
                                + "VALUES(@Name, @Title, @Email, @Phone, @CompanyId);"
                                + "SELECT CAST(SCOPE_IDENTITY() as int); ";
                    _db.Execute(sqlEmp, objComp.Employees);

                    transaction.Complete();
                }
                catch(Exception ex)
                {
                    string a = ex.Message;
                }
            }
        }

        //
        public List<Company> FilterCompanyByName(string name)
        {
            // Name Like %
            return _db.Query<Company>("SELECT * FROM tbl_Companies WHERE Name like '%' + @name + '%' ", new { name }).ToList();
            //

        }

        //
        public List<Company> GetAllCompanyWithEmployees()
        {
            var sql = "SELECT C.*,E.* FROM tbl_Employees AS E "
                    + "INNER JOIN tbl_Companies AS C "
                    + "ON E.CompanyId = C.CompanyId ";

            var companyDic = new Dictionary<int, Company>();

            // 1 = Company, 2 = Employee, 3 = 1 + 2 Company
            var company = _db.Query<Company, Employee, Company>(sql, (c, e) =>
            {
                // only will store one Company Record
                if (!companyDic.TryGetValue(c.CompanyId, out var currentCompany))
                {
                    currentCompany = c;
                    companyDic.Add(currentCompany.CompanyId, currentCompany);
                }
                currentCompany.Employees.Add(e);

                return currentCompany;
                
            }, splitOn: "EmployeeId");

            return company.Distinct().ToList();
        }


        //
        public Company GetCompanyWithEmployees(int id)
        {
            var p = new
            {
                CompanyId = id
            };

            var sql = "SELECT * FROM tbl_Companies WHERE CompanyId = @CompanyId; "
                + " SELECT * FROM tbl_Employees WHERE CompanyId = @CompanyId; ";

            Company company;

            using (var lists = _db.QueryMultiple(sql, p))
            {
                company = lists.Read<Company>().ToList().FirstOrDefault();
                company.Employees = lists.Read<Employee>().ToList();
            }

            return company;
        }

        //
        public List<Employee> GetEmployeeWithCompany(int id)
        {
            var sql = "SELECT E.*,C.* FROM tbl_Employees AS E "
                        + "INNER JOIN tbl_Companies AS C "
                        + "ON E.CompanyId = C.CompanyId ";
            if (id != 0)
            {
                sql += " WHERE E.CompanyId = @Id ";
            }

            // 1 = Employee, 2 = Company, 3 = 1 + 2 Employee
            var employee = _db.Query<Employee, Company, Employee>(sql, (e, c) =>
            {
                e.Company = c;
                return e;
            }, new { id }, splitOn: "CompanyId");

            return employee.ToList();
        }

        //
        public void RemoveRange(int[] companyId)
        {
            // remove multiple company id *** used IN at Sql
            _db.Query("DELETE FROM tbl_Companies WHERE CompanyId IN @companyId", new { companyId });
            //
        }

        //
    }

}