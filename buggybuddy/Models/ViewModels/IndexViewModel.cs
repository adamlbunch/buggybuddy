using System.ComponentModel.DataAnnotations;

namespace buggybuddy.Models.ViewModels
{
    public class IndexViewModel
    {
		[Display(Name = "User Name", Prompt = "Enter your user name.")]
		[StringLength(16, MinimumLength = 3)]
		[RegularExpression(@"^[a-zA-Z0-9]*$")]
		[Required]
		public string LoginUserName { get; set; }

		[Display(Name = "Password", Prompt = "Enter your password.")]
		[DataType(DataType.Password)]
		[Required]
		public string LoginPassword { get; set; }

		[Display(Name = "First Name", Prompt = "Enter a 1 to 25 alpha name.")]
		[StringLength(25, MinimumLength = 1)]
		[RegularExpression(@"^[a-zA-Z]+$")]
		[Required]
		public string RegisterFirstName { get; set; }

		[Display(Name = "Last Name", Prompt = "Enter a 1 to 25 alpha name.")]
		[StringLength(25, MinimumLength = 1)]
		[RegularExpression(@"^[a-zA-Z]+$")]
		[Required]
		public string RegisterLastName { get; set; }

		[Required]
		public string RegisterGender { get; set; }

		[Required]
		public string RegisterInterest { get; set; }

		[Required]
		public string RegisterInfo { get; set; }

		[Display(Name = "User Name", Prompt = "Enter a unique user name.")]
		[StringLength(16, MinimumLength = 3)]
		[RegularExpression(@"^[a-zA-Z0-9]*$")]
		[Required]
		public string RegisterUserName { get; set; }

		[Display(Name = "Password", Prompt = "Enter a 4 to 16 key password.")]
		[StringLength(16, MinimumLength = 4)]
		[DataType(DataType.Password)]
		[Required]
		public string RegisterPassword { get; set; }

		/*[Display(Name = "Confirm Password")]
		[StringLength(16, MinimumLength = 4)]
		[DataType(DataType.Password)]
		[Compare(nameof(RegisterPassword))]
		[Required]
		public string RegisterConfirmPassword { get; set; }*/
	}
}
