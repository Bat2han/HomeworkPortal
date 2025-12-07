using System.ComponentModel.DataAnnotations;

namespace HomeworkPortal.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string EmailOrUserName { get; set; } = null!;

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
