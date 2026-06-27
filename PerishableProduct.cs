
using System;

namespace Smart_inventory_system
{
    public class PerishableProduct : Product
    {
        public DateTime ExpiryDate { get; set; }

        public PerishableProduct(
            string name,
            string code,
            string category,
            double price,
            int stock,
            DateTime expiryDate)
            : base(name, code, category, price, stock)
        {
            ExpiryDate = expiryDate;
        }

        public override double CalculateInventoryValue()
        {
            TimeSpan remainingDays =
                ExpiryDate.Date - DateTime.Today;

            if (remainingDays.Days >= 0 &&
                remainingDays.Days < 3)
            {
                Console.WriteLine(
                    $"[Expiry Warning] Product {Name} expires in less than 3 days. 50% discount applied.");

                return (Price * 0.5) * Stock;
            }

            return base.CalculateInventoryValue();
        }
    }
}




    






