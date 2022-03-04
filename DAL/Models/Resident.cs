using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class Resident
    {
        public Resident()
        {
            MerchantStores = new HashSet<MerchantStore>();
            News = new HashSet<News>();
            Orders = new HashSet<Order>();
            Pois = new HashSet<Poi>();
            ProductCategories = new HashSet<ProductCategory>();
            Products = new HashSet<Product>();
        }

        public string ResidentId { get; set; }
        public string ResidentName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public int? Status { get; set; }
        public string DeliveryAddress { get; set; }
        public string ApproveBy { get; set; }
        public string Type { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string AccountId { get; set; }
        public string ApartmentId { get; set; }

        public virtual Account Account { get; set; }
        public virtual Apartment Apartment { get; set; }
        public virtual ICollection<MerchantStore> MerchantStores { get; set; }
        public virtual ICollection<News> News { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Poi> Pois { get; set; }
        public virtual ICollection<ProductCategory> ProductCategories { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
