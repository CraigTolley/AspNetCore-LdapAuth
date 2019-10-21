using System.ComponentModel.DataAnnotations;

namespace TestAuth2Mvc.ViewModels
{
    public class SigninViewModel
    {
        [Display(Name = "User name")]
        [Required(ErrorMessage ="You must enter your username!")]        
        public string UserName { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "You must enter your password!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}