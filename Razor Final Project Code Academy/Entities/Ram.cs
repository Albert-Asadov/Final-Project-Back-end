using System;
namespace Razor_Final_Project_Code_Academy.Entities
{
	public class Ram:BaseEntity
	{
		public byte RamName { get; set; }

		public List<ProductRamMemory> ProductRamMemories { get; set; }

		public Ram()
		{
			ProductRamMemories = new();

        }
	}
}

