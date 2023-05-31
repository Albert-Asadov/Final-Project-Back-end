using System;
using System.ComponentModel.DataAnnotations;

namespace Razor_Final_Project_Code_Academy.ViewModel
{
    public class AccountSettingVM
    {
        public string? UserName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword))]
        public string? ConfirmNewPassword { get; set; }

        [DataType(DataType.Password)]
        public string? CurrentPassword { get; set; }
    }
}

