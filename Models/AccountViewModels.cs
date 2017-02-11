using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tutorat.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        /*AJOUT : **/

        [StringLength(50)]
        [Required]
        [Display(Name = "Prénom")]
        public string prenom { get; set; }

        [StringLength(50)]
        [Required]
        [Display(Name = "Nom")]
        public string nom { get; set; }

        [StringLength(8)]
        [Required]
        [Display(Name = "Matricule")]
        public string matricule { get; set; }

        [Required]
        [Display(Name = "Section")]
        public string section { get; set; }

        [Display(Name = "Année actuelle")]
        [Required]
        public int annee { get; set; }

        [StringLength(4)]
        [Required]
        [Display(Name = "Groupe")]
        public string groupe { get; set; }

        [Required]
        [EmailAddress]
        /** REGEX "normal" : (?i)^[_a-z]+([-._a-z]+)*@(students.ephec.be|ephec.be) 
         * Explanation :    1. Not Case Sensitive with (?i)
         *                  2. Start of string with ^
         *                  3. With the string contains characters from [a-z_]+ (+ means one of more of the previous chain of string)
         *                  4. Start of a group with the ()
         *                  4.a The group can contain character type of -._a-z  (the upper case are already checked with 1.)
         *                  5. The * say that we can have the previous group zero or more times.
         *                  6. The @ character is going to be mandatory
         *                  7. The last group can be students.ephec.be OR ephec.be (thats the most important part
         * Conclusion : only the characters a to z, A to Z, _, . and - are allowed before the @
         *              and after the @, only the domain name students.ephec.be OR ephec.be are allowed.
         * Note : [_a-z]+  is like \w+             
         **/
        [RegularExpression("(?i)^[_a-z]+([-._a-z]+)*@(students.ephec.be|ephec.be)", ErrorMessage = "Veuillez utiliser votre adresse EPHEC")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        
        /** Ajouter les cours !! PAS ICI !! Après s'être enregistré dans l'onglet EPERSO !! **/
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
