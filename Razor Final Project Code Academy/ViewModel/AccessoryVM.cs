using System;
using Razor_Final_Project_Code_Academy.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Razor_Final_Project_Code_Academy.ViewModel
{
	public class AccessoryVM
	{
        public int Id { get; set; }
        [StringLength(maximumLength: 100)]
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal? Discount { get; set; }
        public decimal? DiscountPrice { get; set; }
        public string SKU { get; set; }
        [StringLength(maximumLength: 500)]
        public string Desc { get; set; }
        public int BrandId { get; set; }
        [NotMapped]
        public ICollection<int> CategoryIds { get; set; } = null!;
        [NotMapped]
        public IFormFile? MainPhoto { get; set; }
        [NotMapped]
        public List<IFormFile>? Images { get; set; }
        [NotMapped]
        public List<AccessoryImage>? AllImages { get; set; }
        [NotMapped]
        public List<int>? ImagesId { get; set; }
        [NotMapped]
        public string? AccessoryColor { get; set; }
        public string? AccessoryColorDelete { get; set; }
        public List<AccessoryColor>? AccessoryColors { get; set; }

        public AccessoryVM()
        {
            AccessoryColors = new();
        }
    }
}

