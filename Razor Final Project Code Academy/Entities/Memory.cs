using System;
namespace Razor_Final_Project_Code_Academy.Entities
{
	public class Memory:BaseEntity
	{
		public int MemoryName { get; set; }

        public List<ProductRamMemory> ProductRamMemories { get; set; }

		public Memory()
		{
			ProductRamMemories = new();
		}
    }
}

