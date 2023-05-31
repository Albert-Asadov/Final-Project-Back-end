using System;
using Razor_Final_Project_Code_Academy.Entities;

namespace Razor_Final_Project_Code_Academy.Entities
{
	public class Category:BaseEntity
	{
        public string Name { get; set; }

        public List<ProductCategory> productCategories { get; set; }

        public Category()
        {
            productCategories = new();
        }
    }
}

