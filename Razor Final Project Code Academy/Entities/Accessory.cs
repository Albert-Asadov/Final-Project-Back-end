using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Razor_Final_Project_Code_Academy.Entities;
using Razor_Final_Project_Code_Academy.ViewModel;

namespace Razor_Final_Project_Code_Academy.Entities
{
	public class Accessory:BaseEntity
	{
        [StringLength(maximumLength: 100)]
        public string Name { get; set; }

		public decimal Price { get; set; }

		public decimal Discount { get; set; }

		public decimal DiscountPrice { get; set; }

		public string SKU { get; set; }

        public bool InStock { get; set; }

		public int Count { get; set; }

        [StringLength(maximumLength: 500)]
        public string Descr { get; set; }

		public List<AccessoryImage> AccessoryImages { get; set; }

		public List<AccessoryCategory> AccessoryCategories { get; set; }

		public Brand Brand { get; set; }

		public int BrandId { get; set; }

        public List<Comment> AccessoryComments { get; set; } = null;

        public List<AccessoryColor> accessoryColors { get; set; }

        [NotMapped]
        public BasketAcc Adding { get; set; }


        public Accessory()
		{
			AccessoryImages = new();
            accessoryColors = new();
            AccessoryComments = new();
			AccessoryCategories = new();
        }
    }
}

