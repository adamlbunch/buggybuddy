using System.ComponentModel.DataAnnotations;
using Microsoft.Azure.KeyVault.Models;

namespace buggybuddy.Models.ViewModels
{
    public class IndexViewModel
    {
		[StringLength(16, MinimumLength = 3)]
		[RegularExpression(@"^[a-zA-Z0-9]*$")]
        [Required(ErrorMessage = "Enter your username")]
        public string LoginUserName { get; set; }

		[Display(Name = "Password", Prompt = "Enter your password")]
		[DataType(DataType.Password)]
        [Required(ErrorMessage = "Please enter a password")]
        public string LoginPassword { get; set; }

        [StringLength(25, MinimumLength = 1)]
		[RegularExpression(@"^[a-zA-Z]+$")]
        [Required(ErrorMessage = "Please enter your first name")]
        public string RegisterFirstName { get; set; }

		[Display(Name = "Last Name", Prompt = "Enter a 1 to 25 alpha name")]
		[StringLength(25, MinimumLength = 1)]
		[RegularExpression(@"^[a-zA-Z]+$")]
		[Required]
		public string RegisterLastName { get; set; }

        [Required(ErrorMessage = "Select your gender")]
		public string RegisterGender { get; set; }

        [Required(ErrorMessage = "Select the gender you are Interested in")]
		public string RegisterInterest { get; set; }

		[StringLength(16, MinimumLength = 4)]
		[RegularExpression(@"^[a-zA-Z0-9]*$")]
		[Required(ErrorMessage = "Enter a unique user name.")]
		public string RegisterUserName { get; set; }

		[StringLength(16, MinimumLength = 4)]
		[DataType(DataType.Password)]
		[Required (ErrorMessage = "Enter a valid password")]
		public string RegisterPassword { get; set; }

        [Required(ErrorMessage = "Tell us something about yourself")]
        public string RegisterInfo { get; set; }
    }
}
