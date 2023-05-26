using System;
using Microsoft.AspNetCore.Identity;

namespace Razor_Final_Project_Code_Academy.Entities
{
	public class User:IdentityUser
	{
		public string Fullname { get; set; }

	}
}

