using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Tadpole.Web.Pages
{
    public class IndexModel : PageModel
    {
        public bool IsRegistered { get; set; }

        public class RegistrationModel
        {
            //TODO: validate email
            [Required]
            public string Email { get; set; }

            //TODO: min/max lengths
            [Required]
            public string Password { get; set; }
        }

        [BindProperty]
        public RegistrationModel Input { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //TODO: check for duplicate emails
            //TODO: save user to database with encrypted password

            IsRegistered = true;

            return Page();
        }
    }
}
