using System;
using Razor_Final_Project_Code_Academy.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Razor_Final_Project_Code_Academy.ViewModel
{
	public class ProductVM
	{
        public int Id { get; set; }
        [StringLength(maximumLength: 20)]
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal? Discount { get; set; }
        public decimal? DiscountPrice { get; set; }
        public string SKU { get; set; }
        public string Desc { get; set; }
        public int BrandId { get; set; }
        [NotMapped]
        public ICollection<int> CategoryIds { get; set; } = null!;
        [NotMapped]
        public IFormFile? MainPhoto { get; set; }
        [NotMapped]
        public List<IFormFile>? Images { get; set; }
        [NotMapped]
        public List<ProductImage>? AllImages { get; set; }
        [NotMapped]
        public List<int>? ImagesId { get; set; }
        [NotMapped]
        public string? ProductRamMemory { get; set; }
        public string? ProductRamMemoryDelete { get; set; }
        public List<ProductRamMemory>? ProductRamMemories { get; set; }

        public ProductVM()
        {
            ProductRamMemories = new();
        }
    }
}

