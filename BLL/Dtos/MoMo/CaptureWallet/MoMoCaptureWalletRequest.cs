using System;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Linq;

namespace BLL.Dtos.MoMo.CaptureWallet
{
    [Serializable]
    public class MoMoCaptureWalletRequest
    {
        public string PartnerCode { get; set; }
        public string PartnerName { get; set; }
        public string StoreId { get; set; }
        public string RequestId { get; set; }
        public long Amount { get; set; }
        public string OrderId { get; set; }
        public string OrderInfo { get; set; }
        public string RedirectUrl { get; set; }
        public string IpnUrl { get; set; }
        public string RequestType { get; set; }
        public string ExtraData { get; set; }
        public string Lang { get; set; } = "vi";
        public string Signature { get; set; }

        [JsonIgnore]
        public JObject ToJson
        {
            get
            {
                return new JObject
                {
                { "partnerCode", this.PartnerCode },
                { "partnerName", "Test" },
                { "storeId", "MomoTestStore" },
                { "requestId", this.RequestId },
                { "amount", this.Amount },
                { "orderId", this.OrderId },
                { "orderInfo", this.OrderInfo },
                { "redirectUrl", this.RedirectUrl },
                { "ipnUrl", this.IpnUrl },
                { "lang", "en" },
                { "extraData", this.ExtraData },
                { "requestType", this.RequestType },
                { "signature", this.Signature }
                };
            }
        }
    }
}
