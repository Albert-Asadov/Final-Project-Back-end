using System;
namespace Razor_Final_Project_Code_Academy.Entities
{
	public class Comment:BaseEntity
	{
        public string Name { get; set; }

        public string Email { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public DateTime CreationTime { get; set; }

        public Product? Product { get; set; }

        public Accessory? Accessory { get; set; }
    }
}

