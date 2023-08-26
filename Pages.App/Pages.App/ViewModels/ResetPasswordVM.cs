using System.ComponentModel.DataAnnotations;

namespace Pages.App.ViewModels
{
    public class ResetPasswordVM
    {
        public string Mail { get; set; }
        public string Token { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
