

using System;
using System.Collections.Generic;
using System.IO;

namespace Smart_inventory_system
{
    public static class FileManager
    {
        private static readonly string filePath = "inventory.txt";

        public static void SaveProductToFile(Product product)
        {
            try
            {
                string line;

                if (product is PerishableProduct perishable)
                {
                    line =
                        $"Perishable,{perishable.Name},{perishable.Code}," +
                        $"{perishable.Category},{perishable.Price}," +
                        $"{perishable.Stock},{perishable.ExpiryDate:yyyy-MM-dd}";
                }
                else
                {
                    line =
                        $"Normal,{product.Name},{product.Code}," +
                        $"{product.Category},{product.Price}," +
                        $"{product.Stock},N/A";
                }

                using (StreamWriter sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine(line);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving file: {ex.Message}");
            }
        }

        public static List<Product> ReadProductsFromFile()
        {
            List<Product> products = new List<Product>();

            if (!File.Exists(filePath))
                return products;

            try
            {
                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    string[] parts = line.Split(',');

                    if (parts.Length < 7)
                        continue;

                    string type = parts[0];
                    string name = parts[1];
                    string code = parts[2];
                    string category = parts[3];

                    double price = double.Parse(parts[4]);
                    int stock = int.Parse(parts[5]);

                    if (type == "Perishable")
                    {
                        DateTime expiryDate = DateTime.Parse(parts[6]);

                        products.Add(
                            new PerishableProduct(
                                name,
                                code,
                                category,
                                price,
                                stock,
                                expiryDate));
                    }
                    else
                    {
                        products.Add(
                            new Product(
                                name,
                                code,
                                category,
                                price,
                                stock));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
            }

            return products;
        }

        public static void UpdateInventoryFile(List<Product> products)
        {
            try
            {
                File.WriteAllText(filePath, string.Empty);

                foreach (var product in products)
                {
                    SaveProductToFile(product);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating file: {ex.Message}");
            }
        }
    }
}
