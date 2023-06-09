using System;
using System.ComponentModel.DataAnnotations;

namespace Razor_Final_Project_Code_Academy.ViewModel
{
	public class RegisterVM
	{
		public string FirstName { get; set; }

		public string LastName { get; set; }

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

