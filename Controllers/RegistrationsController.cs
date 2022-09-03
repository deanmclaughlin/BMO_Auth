using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BMO_Auth.Data;
using BMO_Auth.Models;
using BMO_Auth.Models.Exceptions;
using Newtonsoft.Json;

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
			// If there are exceptions, store them in the view data/bag
            if (TempData["Exceptions"] != null) {
                ViewData["Exceptions"] =
				     JsonConvert.DeserializeObject(TempData["Exceptions"].ToString(), typeof(ValidationException));				
			}
			
			ViewData["Accounttypes"] = new SelectList(_context.Accounttypes, "Id", "AcctNameAndId");
            return View();
        }

        public IActionResult DoRegistration(string firstName, string lastName, string birthDate, string homeAddress,
                                            string accountType, string openBalance, string theDate)
        {
            // declare ValidationException collection
            ValidationException validationState = new ValidationException();

            if (string.IsNullOrWhiteSpace(firstName))
			{
                validationState.SubExceptions.Add(new Exception("Client First Name must be entered."));
			}

            if (string.IsNullOrWhiteSpace(lastName))
			{
                validationState.SubExceptions.Add(new Exception("Client Last Name must be entered."));
			}
 
            if (string.IsNullOrWhiteSpace(birthDate))
			{
                validationState.SubExceptions.Add(new Exception("Client Birth Date must be entered."));
			}
			
            if (string.IsNullOrWhiteSpace(homeAddress))
			{
                validationState.SubExceptions.Add(new Exception("Client Home Address must be entered."));
			}			
			
            if (string.IsNullOrWhiteSpace(openBalance))
			{
                validationState.SubExceptions.Add(new Exception("Account Opening Balance must be provided."));
            }
			else
			{
                if (!Decimal.TryParse(openBalance, out decimal temp))
				{
                    validationState.SubExceptions.Add(new Exception("Account Opening Balance must be a number."));
				}
                else
				{
                    if (temp < 0)
					{
						validationState.SubExceptions.Add(new Exception("Account Opening Balance cannot be less than zero."));
					}
				}
			}

		
			if (validationState.Any)
			{
                TempData["Exceptions"] = JsonConvert.SerializeObject(validationState);				
			}
			else
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
			}
            return RedirectToAction("Index");
        }
    }
}
