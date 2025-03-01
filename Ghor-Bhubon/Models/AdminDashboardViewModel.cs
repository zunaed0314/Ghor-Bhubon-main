namespace Ghor_Bhubon.Models
{
    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalLandlords { get; set; }
        public int TotalTenants { get; set; }
        public int TotalPosts { get; set; }
        public int TotalHomesRented { get; set; }
        public decimal TotalTransactions { get; set; }
        public decimal TotalRevenue { get; set; }
        public int NewUsersThisMonth { get; set; }
        public int NewPostsThisMonth { get; set; }
        public int HousesRentedThisMonth { get; set; }
        public decimal TransactionsThisMonth { get; set; }
        public decimal RevenueThisMonth { get; set; }
    }
}
