using System.Collections.Generic;
using System.Linq;
using DapperDemo.Data;
using DapperDemo.Models;

namespace DapperDemo.Repository
{
    public class CompanyRepository_EF : ICompanyRepository
    {
        // EF
        private readonly ApplicationDbContext _db;

        public CompanyRepository_EF(ApplicationDbContext db)
        {
            _db = db;
        }

        //
        public Company Add(Company company)
        {
             _db.tbl_Companies.Add(company);
            _db.SaveChanges();
            return company;
            //
        }

        public Company Find(int id)
        {
            return _db.tbl_Companies.FirstOrDefault( c=> c.CompanyId == id);
            //
        }

        public List<Company> GetAll()
        {
            return _db.tbl_Companies.ToList();
            //
        }

        public void Remove(int id)
        {
            Company company = _db.tbl_Companies.FirstOrDefault(c => c.CompanyId == id);
            _db.tbl_Companies.Remove(company);
            _db.SaveChanges();
            return;
            //
        }

        public Company Update(Company company)
        {
            _db.tbl_Companies.Update(company);
            _db.SaveChanges();
            return company;
        }
    }

}