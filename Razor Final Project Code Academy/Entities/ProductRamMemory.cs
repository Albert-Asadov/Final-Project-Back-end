using System;
namespace Razor_Final_Project_Code_Academy.Entities
{
	public class ProductRamMemory:BaseEntity
	{
		public int ProductId { get; set; }
		public Product Product { get; set; }

		public int RamId { get; set; }
		public Ram Ram { get; set; }

		public int MemoryId { get; set; }
		public Memory Memory { get; set; }

		public byte Quantity { get; set; }
	}
}

