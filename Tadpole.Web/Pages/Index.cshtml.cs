using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Tadpole.Web.Services;

namespace Tadpole.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IEncryption _encryption;
        private readonly IRegister _register;

        public IndexModel(IEncryption encryption, IRegister register)
        {
            _encryption = encryption;
            _register = register;
        }


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

            if (_register.IsEmailAlreadyRegistered(Input.Email))
            {
                //don't tell the current user this email is already registered in case they are a malicious user snoping
                //TODO: send email to user informing them of an attempt to register again with help to login/reset password etc.
                IsRegistered = true;
                return Page();
            }

            //encrypt password ASAP to reduce risk of plain text passwords creeping up the stack
            var encryptionResult = _encryption.Encrypt(Input.Password);

            RegistrationUser registrationUser = new RegistrationUser { 
                Email = Input.Email,
                PasswordHash = encryptionResult.Hash,
                PasswordSalt = encryptionResult.Salt
            };

            _register.Save(registrationUser);

            IsRegistered = true;

            return Page();
        }
    }
}
