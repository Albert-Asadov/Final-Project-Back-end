using System;
using System.ComponentModel.DataAnnotations;

namespace Final_Project_Razor.ViewModel
{
	public class LoginVM
	{
        public string Username { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}

