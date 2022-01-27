using BLL.Dtos.OrderDetail;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace BLL.Dtos.Order
{
    public class ExtendOrderResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Collection<OrderDetailResponse> OrderDetails { get; set; }
    }
}
