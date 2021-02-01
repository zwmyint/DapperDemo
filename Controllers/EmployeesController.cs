using System.Collections.Generic;
using System.Linq;
using DapperDemo.Models;
using DapperDemo.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DapperDemo.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ICompanyRepository _compRepo;
        private readonly IEmployeeRepository _empRepo;

        //data binding for Create Employee
        [BindProperty]
        public Employee Employee { get; set; }


        public EmployeesController(ICompanyRepository compRepo, IEmployeeRepository empRepo)
        {
            _compRepo = compRepo;
            _empRepo = empRepo;
        }

        //
        public IActionResult Index(int companyId=0)
        {
            //List<Employee> employees = _empRepo.GetAll();
            //foreach(Employee obj in employees)
            //{
            //    obj.Company = _compRepo.Find(obj.CompanyId);
            //}

            //List<Employee> employees = _bonRepo.GetEmployeeWithCompany(companyId);
            //return View(employees);
            return View(_empRepo.GetAll());

        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            IEnumerable<SelectListItem> companyList = _compRepo.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.CompanyId.ToString()
            });
            ViewBag.CompanyList = companyList;

            return View();
        }

        //
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Create")]
        public IActionResult CreatePOST()
        {
            if (ModelState.IsValid)
            {
                _empRepo.Add(Employee); // from binding property (not put at parameter)
                return RedirectToAction(nameof(Index));
            }
            return View(Employee);
        }

        //
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Employee = _empRepo.Find(id.GetValueOrDefault());
            
            IEnumerable<SelectListItem> companyList = _compRepo.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.CompanyId.ToString()
            });
            ViewBag.CompanyList = companyList;

            if (Employee == null)
            {
                return NotFound();
            }
            return View(Employee);
        }

        //
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id)
        {
            if (id != Employee.EmployeeId) // from binding property (not put at parameter)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _empRepo.Update(Employee); // from binding property (not put at parameter)
                return RedirectToAction(nameof(Index));
            }
            return View(Employee);
        }

        //
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            _empRepo.Remove(id.GetValueOrDefault());
            return RedirectToAction(nameof(Index));
        }



        //
    }
}