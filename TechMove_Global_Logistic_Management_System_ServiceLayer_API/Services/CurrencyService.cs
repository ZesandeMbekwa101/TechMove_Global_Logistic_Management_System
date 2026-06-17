namespace TechMove_Global_Logistic_Management_System_ServiceLayer_API.Services
{
    public class CurrencyService
    {
        public decimal ConvertToZar(decimal amount, decimal exchangeRate)
        {
            if (amount < 0)
                throw new ArgumentException("Amount cannot be negative.");

            if (exchangeRate <= 0)
                throw new ArgumentException("Exchange rate must be greater than zero.");

            return amount * exchangeRate;
        }

        public decimal CalculateExchangeRate(decimal zarAmount, decimal originalAmount)
        {
            if (originalAmount <= 0)
                throw new ArgumentException("Original amount must be greater than zero.");

            return zarAmount / originalAmount;
        }
    }
}