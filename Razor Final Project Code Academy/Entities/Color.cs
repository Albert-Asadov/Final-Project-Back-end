using System;
namespace Razor_Final_Project_Code_Academy.Entities
{
	public class Color:BaseEntity
	{
		public string ColorName { get; set; }

		public List<AccessoryColor> accessoryColors { get; set; }

		public Color()
		{
			accessoryColors = new();

        }
	}
}

