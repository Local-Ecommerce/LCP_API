﻿using BLL.Dtos.ProductInMenu;
using BLL.Dtos.Resident;
using BLL.Dtos.StoreMenuDetail;
using System;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace BLL.Dtos.Menu
{
    public class MenuResponse
    {
        public string MenuId { get; set; }
        public string MenuName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? Status { get; set; }
        public string ResidentId { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public ResidentResponse Resident { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Collection<ProductInMenuResponse> ProductInMenus { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Collection<StoreMenuDetailResponse> StoreMenuDetails { get; set; }
    }
}
