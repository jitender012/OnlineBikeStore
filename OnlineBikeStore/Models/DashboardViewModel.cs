using System.Collections.Generic;

namespace OnlineBikeStore.Models
{
    public class DashboardViewModel
    {
        public decimal TotalSales{ get; set; }
        public decimal ItemsDelivered{ get; set; }
        public int OrderCancelled{ get; set; }
        public int TotalCustomers{ get; set; }

        public List<OrderViewModel> PendingOrders{ get; set; }
        public List<StockViewModel> LowStocks { get; set; }        

    }
}