using System;
using System.ComponentModel.DataAnnotations;

namespace Final_Project_Razor.ViewModel
{
	public class RegisterVM
	{
		public string FirstName { get; set; }

		public string LastNAme { get; set; }

		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }

		public string UserName { get; set; }

		[DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

		public bool Terms { get; set; }

	}
}

