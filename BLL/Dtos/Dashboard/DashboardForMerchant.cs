namespace BLL.Dtos.Dashboard
{
    public class DashboardForMerchant
    {
        public DashboardForMerchant()
        {
            this.TotalOrder = 0;
            this.CompletedOrder = 0;
            this.CanceledOrder = 0;
            this.TotalRevenue = 0;
        }
        public int TotalOrder { get; set; }
        public int CompletedOrder { get; set; }
        public int CanceledOrder { get; set; }
        public double TotalRevenue { get; set; }
    }
}
