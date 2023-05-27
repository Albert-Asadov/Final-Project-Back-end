using System;
using Razor_Final_Project_Code_Academy.Entities;

namespace Razor_Final_Project_Code_Academy.Entities
{
	public class AccessoryImage:BaseEntity
	{
        public bool IsMain { get; set; }

        public string Image { get; set; }

        public Accessory Accessory { get; set; }

        public int AccessoryId { get; set; }
    }
}

