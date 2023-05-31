using System;
namespace Razor_Final_Project_Code_Academy.Entities
{
	public class Brand:BaseEntity
	{
        public string Name { get; set; }

		public List<Product> Products { get; set; }

        public List<Accessory> accessories { get; set; }


        public Brand()
		{
			accessories = new();
            Products = new();
		}
	}
}

