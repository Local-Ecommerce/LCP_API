using BLL.Dtos.Merchant;
using System;

namespace BLL.Dtos.Collection
{
    public class CollectionResponse
    {
        public string CollectionId { get; set; }
        public string CollectionName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? Status { get; set; }
        public MerchantResponse Merchant { get; set; }
    }
}
