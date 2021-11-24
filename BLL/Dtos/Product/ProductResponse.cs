﻿using System;

namespace BLL.Dtos.Product
{
    public class ProductResponse
    {
        public string ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductType { get; set; }
        public double DefaultPrice { get; set; }
        public string Image { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public double Weight { get; set; }
    }
}