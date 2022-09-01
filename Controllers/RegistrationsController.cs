using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BMO_Auth.Data;
using BMO_Auth.Models;

namespace BMO_Auth.Controllers
{
    public class RegistrationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RegistrationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
			ViewData["Accounttypes"] = new SelectList(_context.Accounttypes, "Id", "AcctNameAndId");
            return View();
        }

        public IActionResult DoRegistration(string firstName, string lastName, string birthDate, string homeAddress,
                                            string accountType, string openBalance, string theDate)
        {
            Client newClient = new()
            {
                FirstName = firstName,
                LastName = lastName,
                BirthDate = DateOnly.Parse(birthDate),
                HomeAddress = homeAddress,
                Accounts = new List<Account> {
                    new Account() {
                        AccountTypeId = int.Parse(accountType),
                        Balance = Math.Round(Decimal.Parse(openBalance), 2),
                        InterestAppliedDate = DateOnly.Parse(theDate)
                    }
                }
            };
            _context.Clients.Add(newClient);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
		
    }
}
