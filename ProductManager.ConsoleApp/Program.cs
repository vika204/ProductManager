using ProductManager.DBModels;
using ProductManager.Services;
using ProductManager.UIModels;

namespace ProductManager.ConsoleApp
{
    internal class Program
    {
        private static StorageService _storageService = new StorageService();
        private static List<WarehouseUIModel> _warehouses = new List<WarehouseUIModel>();

        private const int BoxWidth = 42;
        private const char BorderChar = '*';

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            bool isRunning = true;

            while (isRunning)
            {
                ShowMainMenu();
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        LoadAndShowWarehouses();
                        break;
                    case "2":
                        SelectWarehouse();
                        break;
                    case "0":
                        isRunning = false;
                        PrintHeader("Goodbye!");
                        break;
                    default:
                        PrintLine("Invalid command. Please try again.");
                        break;
                }

                PrintEmpty();
            }
        }

        private static void ShowMainMenu()
        {
            PrintHeader("Product Manager");
            PrintLine("1 — Show all warehouses");
            PrintLine("2 — Select a warehouse");
            PrintLine("0 — Exit");
            PrintSeparator();
            Console.Write("  Your choice: ");
        }

        private static void LoadAndShowWarehouses()
        {
            if (_warehouses.Count == 0)
            {
                foreach (WarehouseDBModel dbModel in _storageService.GetWarehouses())
                {
                    _warehouses.Add(new WarehouseUIModel(dbModel));
                }
            }

            PrintHeader("Warehouse List");

            if (_warehouses.Count == 0)
            {
                PrintLine("No warehouses found.");
                return;
            }

            for (int i = 0; i < _warehouses.Count; i++)
            {
                WarehouseUIModel warehouse = _warehouses[i];
                PrintLine($"{i + 1}. {warehouse.Name} | {warehouse.Location}");
            }

            PrintSeparator();
        }

        private static void SelectWarehouse()
        {
            while (true)
            {
                LoadAndShowWarehouses();

                if (_warehouses.Count == 0) return;

                PrintLine("0 — Back to main menu");
                Console.Write("  Enter warehouse number: ");
                string input = Console.ReadLine();

                if (input == "0") return;

                if (!int.TryParse(input, out int index) || index < 1 || index > _warehouses.Count)
                {
                    PrintLine("Invalid warehouse number. Please try again.");
                    PrintEmpty();
                    continue;
                }

                ShowWarehouseDetails(_warehouses[index - 1]);
            }
        }

        private static void ShowWarehouseDetails(WarehouseUIModel warehouse)
        {
            while (true)
            {
                PrintHeader($"Warehouse: {warehouse.Name}");
                PrintLine($"Location:       {warehouse.Location}");

                warehouse.LoadProducts(_storageService);

                PrintLine($"Total products: {warehouse.TotalProducts}");
                PrintSeparator();
                PrintHeader("Products");

                if (warehouse.TotalProducts == 0)
                {
                    PrintLine("No products found.");
                    PrintLine("0 — Back to warehouse list");
                    Console.Write("  Your choice: ");
                    Console.ReadLine();
                    return;
                }

                for (int i = 0; i < warehouse.Products.Count; i++)
                {
                    ProductUIModel product = warehouse.Products[i];
                    PrintLine($"{i + 1}. {product.Name} | {product.Category} | {product.Price} USD");
                }

                PrintSeparator();
                PrintLine("0 — Back to warehouse list");
                Console.Write("  Enter product number for details: ");
                string input = Console.ReadLine();

                if (input == "0") return;

                if (!int.TryParse(input, out int productIndex) || productIndex < 1 || productIndex > warehouse.Products.Count)
                {
                    PrintLine("Invalid product number. Please try again.");
                    PrintEmpty();
                    continue;
                }

                ShowProductDetails(warehouse.Products[productIndex - 1]);
            }
        }

        private static void ShowProductDetails(ProductUIModel product)
        {
            PrintHeader($"Product: {product.Name}");
            PrintLine($"Category:      {product.Category}");
            PrintLine($"Price:         {product.Price} USD");
            PrintLine($"Quantity:      {product.Quantity} pcs");
            PrintLine($"Total value:   {product.TotalValue} USD");
            PrintLine($"Description:   {product.Description}");
            PrintSeparator();
            PrintLine("Press Enter to go back...");
            Console.ReadLine();
        }

        private static void PrintHeader(string title)
        {
            string border = new string(BorderChar, BoxWidth);
            string paddedTitle = title.PadLeft((BoxWidth + title.Length) / 2).PadRight(BoxWidth);

            Console.WriteLine(border);
            Console.WriteLine($"*{paddedTitle}*");
            Console.WriteLine(border);
        }

        private static void PrintSeparator()
        {
            Console.WriteLine(new string('-', BoxWidth));
        }

        private static void PrintLine(string text)
        {
            Console.WriteLine($"  {text}");
        }

        private static void PrintEmpty()
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}