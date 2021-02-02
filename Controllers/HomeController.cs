using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DapperDemo.Models;
using DapperDemo.Repository;
using System.Linq;

namespace DapperDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBonusRepository _bonRepo;


        public HomeController(ILogger<HomeController> logger, IBonusRepository bonRepo)
        {
            _logger = logger;
            _bonRepo = bonRepo;
        }

        public IActionResult Index()
        {
            // from bonusrepo - GetAllCompanyWithEmployees
            IEnumerable<Company> companies = _bonRepo.GetAllCompanyWithEmployees();
            return View(companies);
        }

        //
        public IActionResult AddTestRecords()
        {
            // Comp 1
            Company company = new Company()
            {
                Name = "Test 123" + Guid.NewGuid().ToString(),
                Address = "test address",
                City = "test city",
                PostalCode = "test postalCode",
                State = "test state",
                Employees = new List<Employee>()
            };
                // Emp 1
                company.Employees.Add(new Employee()
                {
                    Email = "test Email 123",
                    Name = "Test Name " + Guid.NewGuid().ToString(),
                    Phone = " test phone",
                    Title = "Test Manager"
                });
                // Emp 2
                company.Employees.Add(new Employee()
                {
                    Email = "test Email 2 123",
                    Name = "Test Name 2" + Guid.NewGuid().ToString(),
                    Phone = " test phone 2",
                    Title = "Test Manager 2"
                });

            _bonRepo.AddTestCompanyWithEmployees(company);
            //_bonRepo.AddTestCompanyWithEmployeesWithTransaction(company);

            return RedirectToAction(nameof(Index));
        }

        //
        public IActionResult RemoveTestRecords()
        {
            // filter company "Test" and remove all
            int[] companyIdToRemove = _bonRepo.FilterCompanyByName("Test").Select(i => i.CompanyId).ToArray();
            _bonRepo.RemoveRange(companyIdToRemove);
            return RedirectToAction(nameof(Index));
        }

        //
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
