using System.Threading.Tasks;
using DapperDemo.Models;
using DapperDemo.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DapperDemo.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly ICompanyRepository _compRepo;

        public CompaniesController(ICompanyRepository compRepo)
        {
            _compRepo = compRepo;
        }

        //
        // GET: Companies
        public IActionResult Index()
        {
            return View(_compRepo.GetAll());
            //return View(_dapperRepo.List<Company>("usp_GetALLCompany"));
        }

        //
        // GET: Companies/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = _compRepo.Find(id.GetValueOrDefault());
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }


        //
        // GET: Companies/Create
        public IActionResult Create()
        {
            return View();
        }


        //
        // POST: Companies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("CompanyId,Name,Address,City,State,PostalCode")] Company company)
        {
            if (ModelState.IsValid)
            {
                _compRepo.Add(company);
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        //
        // GET: Companies/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
           // var company = _dapperRepo.Single<Company>("usp_GetCompany", new  { CompanyId = id.GetValueOrDefault() });
            var company = _compRepo.Find(id.GetValueOrDefault());
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        //
        // POST: Companies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("CompanyId,Name,Address,City,State,PostalCode")] Company company)
        {
            if (id != company.CompanyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _compRepo.Update(company);
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        //
        // GET: Companies/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            _compRepo.Remove(id.GetValueOrDefault());
            return RedirectToAction(nameof(Index));
        }


        //
    }
}