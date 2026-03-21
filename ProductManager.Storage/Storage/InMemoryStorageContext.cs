using ProductManager.Common.Enums;
using ProductManager.DBModels;

namespace ProductManager.Storage
{
    //inmemory storage context 
    public class InMemoryStorageContext : IStorageContext
    {
        // keeps test data in static lists so app can reuse the same instances

        // warehouses list used as a primary source for warehouse data
        private static readonly List<WarehouseDBModel> _warehouses;

        // products list used as a primary source for product data
        private static readonly List<ProductDBModel> _products;

        // initialize test data once
        static InMemoryStorageContext()
        {
            // create warehouses
            WarehouseDBModel warehouseOfBooks = new WarehouseDBModel("Books", WarehouseLocation.Lviv);
            WarehouseDBModel warehouseOfElectronics = new WarehouseDBModel("Electronics", WarehouseLocation.Kyiv);
            WarehouseDBModel warehouseOfClothing = new WarehouseDBModel("Clothing", WarehouseLocation.Odesa);

            _warehouses = new List<WarehouseDBModel>
            {
                warehouseOfBooks,
                warehouseOfElectronics,
                warehouseOfClothing
            };

            // create products and link them to warehouses via warehouse id
            _products = new List<ProductDBModel>
            {
                new ProductDBModel(warehouseOfBooks.Id, "Harry Potter and the Sorcerer's Stone", 100, 19.99m, ProductCategory.Books, "The first book in the Harry Potter series."),
                new ProductDBModel(warehouseOfBooks.Id, "Pride and Prejudice", 50, 14.99m, ProductCategory.Books, "A classic novel by Jane Austen."),
                new ProductDBModel(warehouseOfBooks.Id, "Fahrenheit 451", 75, 12.99m, ProductCategory.Books, "A dystopian novel by Ray Bradbury."),
                new ProductDBModel(warehouseOfBooks.Id, "The Great Gatsby", 30, 10.99m, ProductCategory.Books, "A novel by F. Scott Fitzgerald."),
                new ProductDBModel(warehouseOfBooks.Id, "To Kill a Mockingbird", 60, 15.99m, ProductCategory.Books, "A novel by Harper Lee."),
                new ProductDBModel(warehouseOfBooks.Id, "1984", 80, 13.99m, ProductCategory.Books, "A dystopian novel by George Orwell."),
                new ProductDBModel(warehouseOfBooks.Id, "The Catcher in the Rye", 40, 11.99m, ProductCategory.Books, "A novel by J.D. Salinger."),
                new ProductDBModel(warehouseOfBooks.Id, "Brave New World", 55, 12.49m, ProductCategory.Books, "A dystopian novel by Aldous Huxley."),
                new ProductDBModel(warehouseOfBooks.Id, "The Hobbit", 90, 16.99m, ProductCategory.Books, "A fantasy novel by J.R.R. Tolkien."),
                new ProductDBModel(warehouseOfBooks.Id, "The Lord of the Rings", 70, 29.99m, ProductCategory.Books, "An epic fantasy novel by J.R.R. Tolkien."),

                new ProductDBModel(warehouseOfElectronics.Id, "iPhone 17 Pro Max", 20, 999.99m, ProductCategory.Electronics, "The latest iPhone model with advanced features."),
                new ProductDBModel(warehouseOfElectronics.Id, "EcoFlow Delta 3", 10, 499.99m, ProductCategory.Electronics, "A portable power station with high capacity."),
                new ProductDBModel(warehouseOfElectronics.Id, "Samsung Galaxy S24 Ultra", 15, 899.99m, ProductCategory.Electronics, "The latest Samsung Galaxy model with cutting-edge technology."),
                new ProductDBModel(warehouseOfElectronics.Id, "Solar Panel Kit", 25, 299.99m, ProductCategory.Electronics, "A complete solar panel kit for off-grid power solutions."),
                new ProductDBModel(warehouseOfElectronics.Id, "Eurolamp A60", 50, 9.99m, ProductCategory.Electronics, "An energy-efficient LED light bulb."),

                new ProductDBModel(warehouseOfClothing.Id, "T-shirt", 200, 19.99m, ProductCategory.Clothing, "A comfortable cotton t-shirt available in various colors."),
                new ProductDBModel(warehouseOfClothing.Id, "Jeans", 150, 49.99m, ProductCategory.Clothing, "Classic denim jeans with a modern fit."),
                new ProductDBModel(warehouseOfClothing.Id, "Jacket", 100, 89.99m, ProductCategory.Clothing, "A stylish and warm jacket for the winter season."),
                new ProductDBModel(warehouseOfClothing.Id, "Sneakers", 80, 69.99m, ProductCategory.Clothing, "Comfortable and trendy sneakers for everyday wear."),
                new ProductDBModel(warehouseOfClothing.Id, "Dress", 60, 59.99m, ProductCategory.Clothing, "An elegant dress suitable for special occasions.")
            };
        }

        // return all warehouses as a copy
        public IEnumerable<WarehouseDBModel> GetWarehouses()
        {
            return _warehouses.ToList();
        }

        // return one warehouse by id
        public WarehouseDBModel GetWarehouse(Guid warehouseId)
        {
            return _warehouses.First(w => w.Id == warehouseId);
        }

        // return products for a given warehouse
        public IEnumerable<ProductDBModel> GetProducts(Guid warehouseId)
        {
            return _products.Where(p => p.WarehouseId == warehouseId).ToList();
        }

        // return one product by id
        public ProductDBModel GetProduct(Guid productId)
        {
            return _products.First(p => p.Id == productId);
        }
    }
}