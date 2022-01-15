#nullable disable

namespace DAL.Models
{
    public partial class CollectionMapping
    {
        public string CollectionId { get; set; }
        public string ProductId { get; set; }
        public int? Status { get; set; }

        public virtual Collection Collection { get; set; }
        public virtual Product Product { get; set; }
    }
}
