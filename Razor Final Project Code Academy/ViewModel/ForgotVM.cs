using System;
using Final_Project_Razor.Entities;
using System.ComponentModel.DataAnnotations;
using Razor_Final_Project_Code_Academy.Entities;

namespace Final_Project_Razor.ViewModel
{
	public class ForgotVM
	{
        public User User { get; set; }

        public string Token { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}

