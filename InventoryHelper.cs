
namespace Smart_inventory_system
{
    public static class InventoryHelper
    {
        public static bool ValidateProductCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return false;

            code = code.Trim().ToUpper();

            return code.StartsWith("PROD-") &&
                   code.Length > 5 &&
                   code.Substring(5).Length > 0;
        }
    }
}
