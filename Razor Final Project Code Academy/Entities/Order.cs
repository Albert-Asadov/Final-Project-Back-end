using System;
using System.ComponentModel.DataAnnotations;
using Razor_Final_Project_Code_Academy.Utilities;

namespace Razor_Final_Project_Code_Academy.Entities
{
	public class Order:BaseEntity
	{
        [StringLength(maximumLength: 300)]
        public string Address { get; set; }

        [StringLength(maximumLength: 50)]
        public string Contry { get; set; }

        [StringLength(maximumLength: 50)]
        public string City { get; set; }

        [StringLength(maximumLength: 50)]
        public string Zip { get; set; }

        [StringLength(maximumLength: 50)]
        public string Number { get; set; }

        [Required]
        [StringLength(maximumLength: 20)]
        public string FullName { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public string Email { get; set; }

        [StringLength(maximumLength: 400)]
        public string? Note { get; set; }

        public DateTime CreatedTime { get; set; }

        public Status Status { get; set; }

        public decimal TotalPrice { get; set; }

        public List<OrderItem> OrderItems { get; set; }

        public int BasketId { get; set; }

        public Basket Basket { get; set; }

        public string UserId { get; set; }
    }
}

