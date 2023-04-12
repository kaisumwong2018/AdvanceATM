namespace AdvanceATM.Model
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }  
        public string AccountNumber { get; set; }
        public decimal AccountBalance { get; set; }
        public string CardNumber { get; set; }
        public string NRIC { get; set; }
        public string Password { get; set; }
        public int LoginTry { get; set; }
        public string? Status { get; set; }
        public bool IsLockedOut { get; set; }
        public decimal DailySpend { get; set; }
        //public bool IsTransferAllowed { get; set; }
        public ICollection<Transaction> transactions { get; set; }

        public Customer()
        {
            LoginTry = 3;
            IsLockedOut = false;
        }

    }
}









