﻿using System;
using Razor_Final_Project_Code_Academy.Entities;

namespace Razor_Final_Project_Code_Academy.Entities
{
	public class AccessoryCategory:BaseEntity
	{
        public Accessory Accessory { get; set; }

        public Category Category { get; set; }
    }
}

