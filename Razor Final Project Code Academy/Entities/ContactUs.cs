using System;
namespace Razor_Final_Project_Code_Academy.Entities
{
	public class ContactUs:BaseEntity
	{
		public DateTime SendingTime { get; set; }

		public string FullName { get; set; }

		public string PhoneNumber { get; set; }

		public string Email { get; set; }

		public string Comments { get; set; }
	}
}

