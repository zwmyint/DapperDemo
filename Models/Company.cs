
using System.Collections.Generic;
using Dapper.Contrib.Extensions;
//using System.ComponentModel.DataAnnotations;

namespace DapperDemo.Models
{
    [Dapper.Contrib.Extensions.Table("tbl_Companies")] // for Dapper Contrib Table
    public class Company
    {
        
        // from bonusrepo - GetAllCompanyWithEmployees
        public Company()
        {
            Employees = new List<Employee>();
        }


        [Key] // this key using Dapper.Contrib.Extensions not using DataAnnotations
        public int CompanyId { get; set; }
        public string Name { get; set; }

        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }

        [Write(false)] // Dapper.Contrib.Extensions, this not write to table
        public List<Employee> Employees { get; set; }
        
    }
}