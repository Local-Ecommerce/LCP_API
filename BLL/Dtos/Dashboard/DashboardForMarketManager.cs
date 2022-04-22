namespace BLL.Dtos.Dashboard
{
    public class DashboardForMarketManager
    {
        public DashboardForMarketManager()
        {
            this.TotalCompletedOrder = 0;
            this.TotalCanceledOrder = 0;
            this.TotalFeedback = 0;
            this.TotalProduct = 0;
            this.TotalStore = 0;
        }

        public int TotalCompletedOrder { get; set; }
        public int TotalCanceledOrder { get; set; }
        public int TotalFeedback { get; set; }
        public int TotalProduct { get; set; }
        public int TotalStore { get; set; }
    }
}