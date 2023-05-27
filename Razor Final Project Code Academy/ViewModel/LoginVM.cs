using System;
using System.ComponentModel.DataAnnotations;

namespace Razor_Final_Project_Code_Academy.ViewModel
{
	public class LoginVM
	{
        public string Username { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}

