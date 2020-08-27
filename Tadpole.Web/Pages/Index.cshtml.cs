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
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [StringLength(100, MinimumLength = 12, ErrorMessage = "Password must be between 12 and 100 characters long.")]
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
