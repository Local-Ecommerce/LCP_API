using System;

namespace BLL.Dtos.MoMo.CaptureWallet
{
    public class MoMoCaptureWalletResponse
    {
        public string PartnerCode { get; set; }
        public string RequestId { get; set; }
        public string OrderId { get; set; }
        public long Amount { get; set; }
        public long ResponseTime { get; set; }
        public string Message { get; set; }
        public int ResultCode { get; set; }
        public string PayUrl { get; set; }
        public string Deeplink { get; set; }
        public string QrCodeUrl { get; set; }
        public string DeeplinkWebInApp { get; set; }
        public string DeeplinkMiniApp { get; set; }
    }
}
