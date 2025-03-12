namespace Ghor_Bhubon.Models
{
    public class Transaction
    {
        public string TransactionID { get; set; }
        public int UserID { get; set; }
        public int FlatID { get; set; }
        public int LandlordID { get; set; }
        public decimal Amount { get; set; }
        public string TransactionType { get; set; } // For example, 'Rent Payment', 'Admin Fee', etc.
        public DateTime TransactionDate { get; set; }
        public string Status { get; set; } // Pending, Approved, Declined
        public string Notes { get; set; } // Admin can add any notes for transaction
    }

}