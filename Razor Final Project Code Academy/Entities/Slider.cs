using System;
using System.ComponentModel.DataAnnotations.Schema;
using Razor_Final_Project_Code_Academy.Entities;

namespace Razor_Final_Project_Code_Academy.Entities
{
	public class Slider:BaseEntity
	{
        public string Path { get; set; }

        [NotMapped]
        public IFormFile? Image { get; set; }
    }
}

