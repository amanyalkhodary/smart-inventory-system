

using System;
using System.Collections.Generic;
using static Smart_inventory_system.InventoryHelper;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_inventory_system
{
    class Program { }


        public class Product
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }





        public Product(string name, string code, string category, double price, int stock)
        {
            Name = name;
            Code = code;
            Category = category;
            Price = price;
            Stock = stock;
        }

        public virtual double CalculateInventoryValue()
        {
            return Price * Stock;
        }


        static void Main(string[] args)
        {
           
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            while (true)
            {
                Console.WriteLine("\n=================== Smart Inventory System ===================");
                Console.WriteLine("1. Add New Product");
                Console.WriteLine("2. Display Inventory Report");
                Console.WriteLine("3. Search and Update Stock Quantity");
                Console.WriteLine("4. Exit System");
                Console.Write("Please choose an option (1-4): ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddNewProduct();
                        break;
                    case "2":
                        ShowInventoryReport();
                        break;
                    case "3":
                        SearchAndUpdateStock();
                        break;
                    case "4":
                        Console.WriteLine("\nData saved successfully. Thank you for using the system. Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice! Please enter a number between 1 and 4.");
                        break;
                }
            }
        }

     
        static void AddNewProduct()
        {
            try
            {
                Console.WriteLine("\n--- Add New Product ---");
                Console.Write("Enter product name: ");
                string name = Console.ReadLine();

               
                string code = "";
                while (true)
                {
                    Console.Write("Enter product code (Must start with PROD-): ");
                    code = Console.ReadLine().Trim().ToUpper(); 

                    if (InventoryHelper.ValidateProductCode(code))
                    {
                        break; 
                    }
                    Console.WriteLine("Format Error! Make sure it looks like: PROD-101");
                }

                Console.Write("Enter Category (e.g., Dairy, Meat, Beverages): ");
                string category = Console.ReadLine();

                Console.Write("Enter Price: ");
                double price = double.Parse(Console.ReadLine());

                Console.Write("Enter Stock Quantity: ");
                int stock = int.Parse(Console.ReadLine());

                Console.Write("Is this product perishable / has an expiry date? (yes / no): ");
                string response = Console.ReadLine().Trim().ToLower();

                if (response == "yes" || response == "y")
                {
                    Console.Write("Enter Expiry Date (YYYY-MM-DD, e.g., 2026-06-15): ");
                    DateTime expiryDate = DateTime.Parse(Console.ReadLine());

                    PerishableProduct pProd = new PerishableProduct(name, code, category, price, stock, expiryDate);
                    FileManager.SaveProductToFile(pProd);
                }
                else
                {
                    Product prod = new Product(name, code, category, price, stock);
                    FileManager.SaveProductToFile(prod);
                }

                Console.WriteLine("Product saved successfully to the text file!");
            }
            catch (FormatException)
            {
                Console.WriteLine("Input Error: Please ensure numbers and dates are typed correctly.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        static void ShowInventoryReport()
        {
            Console.WriteLine("\n--- Current Inventory Status Report ---");
            List<Product> products = FileManager.ReadProductsFromFile();

            if (products.Count == 0)
            {
                Console.WriteLine("The inventory database is currently empty or file not found.");
                return;
            }

            double totalInventoryValue = 0;

            Console.WriteLine("-------------------------");
            Console.WriteLine($"{"Type",-12} | {"Name",-15} | {"Code",-10} | {"Category",-12} | {"Price",-8} | {"Stock",-8} | {"Total Value",-15}");
            Console.WriteLine("-------");

            foreach (var prod in products)
            {
                string typeStr = (prod is PerishableProduct) ? "Perishable" : "Normal";


                double itemValue = prod.CalculateInventoryValue();
                totalInventoryValue += itemValue;

                Console.WriteLine($"{typeStr,-12} | {prod.Name,-15} | {prod.Code,-10} | {prod.Category,-12} | {prod.Price,-8} | {prod.Stock,-8} | {itemValue,-15}");
            }

            Console.WriteLine("------------------------");
            Console.WriteLine($"Total Financial Inventory Value: ${totalInventoryValue}");
            Console.WriteLine("-----------------");
        }

     
        static void SearchAndUpdateStock()
        {
            try
            {
                Console.WriteLine("\n--- Search & Update Product Stock ---");
                Console.Write("Enter product name to search: ");

               
                string searchName = Console.ReadLine().Trim();

                List<Product> products = FileManager.ReadProductsFromFile();
                Product foundProduct = null;

                foreach (var prod in products)
                {
                   
                    if (prod.Name.Trim().Equals(searchName, StringComparison.OrdinalIgnoreCase))
                    {
                        foundProduct = prod;
                        break;
                    }
                }

                if (foundProduct != null)
                {
                    Console.WriteLine($"\nProduct Found! Current quantity in stock: {foundProduct.Stock}");
                    Console.Write("Enter new stock level to update: ");
                    int newStock = int.Parse(Console.ReadLine());

                    if (newStock < 0)
                    {
                        Console.WriteLine("Stock quantity cannot be negative. Operation cancelled.");
                        return;
                    }

                
                    foundProduct.Stock = newStock;

               
                    FileManager.UpdateInventoryFile(products);
                    Console.WriteLine("Product stock level updated successfully in the file!");
                }
                else
                {
                    Console.WriteLine("Product not found. Please verify the exact spelling.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing operation: {ex.Message}");
            }
        }
    }

}





