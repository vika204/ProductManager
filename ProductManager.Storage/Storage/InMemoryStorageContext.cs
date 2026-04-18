using ProductManager.Common.Enums;
using ProductManager.DBModels;

namespace ProductManager.Storage
{
    //inmemory storage context 
    public class InMemoryStorageContext : IStorageContext
    {
        // temporary in-memory collections are used only to prepare initial seed data.
        private static readonly List<WarehouseDBModel> _warehouses = new();
        private static readonly List<ProductDBModel> _products = new();

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
        public Task<IEnumerable<WarehouseDBModel>> GetWarehousesAsync()
        {
            // we return copies to avoid accidental mutation of seeds
            IEnumerable<WarehouseDBModel> warehouses = _warehouses
                .Select(warehouse => new WarehouseDBModel(warehouse.Id, warehouse.Name, warehouse.Location))
                .ToList();

            return Task.FromResult(warehouses);
        }

        public Task<WarehouseDBModel?> GetWarehouseAsync(Guid warehouseId)
        {
            WarehouseDBModel? warehouse = _warehouses.FirstOrDefault(item => item.Id == warehouseId);

            if (warehouse is null)
            {
                return Task.FromResult<WarehouseDBModel?>(null);
            }

            return Task.FromResult<WarehouseDBModel?>(new WarehouseDBModel(warehouse.Id, warehouse.Name, warehouse.Location));
        }

        public Task SaveWarehouseAsync(WarehouseDBModel warehouse)
        {
            WarehouseDBModel? existingWarehouse = _warehouses.FirstOrDefault(item => item.Id == warehouse.Id);

            if (existingWarehouse is null)
            {
                _warehouses.Add(new WarehouseDBModel(warehouse.Id, warehouse.Name, warehouse.Location));
                return Task.CompletedTask;
            }

            existingWarehouse.Name = warehouse.Name;
            existingWarehouse.Location = warehouse.Location;

            return Task.CompletedTask;
        }

        // when we delete a warehouse, we also delete all products linked to that warehouse.
        public Task DeleteWarehouseAsync(Guid warehouseId)
        {
            _warehouses.RemoveAll(item => item.Id == warehouseId);
            _products.RemoveAll(item => item.WarehouseId == warehouseId);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<ProductDBModel>> GetProductsByWarehouseAsync(Guid warehouseId)
        {
            // we return copies for safety, not original references.
            IEnumerable<ProductDBModel> products = _products
                .Where(product => product.WarehouseId == warehouseId)
                .Select(product => new ProductDBModel(
                    product.Id,
                    product.WarehouseId,
                    product.Name,
                    product.Quantity,
                    product.Price,
                    product.Category,
                    product.Description))
                .ToList();

            return Task.FromResult(products);
        }

        public Task<ProductDBModel?> GetProductAsync(Guid productId)
        {
            ProductDBModel? product = _products.FirstOrDefault(item => item.Id == productId);

            if (product is null)
            {
                return Task.FromResult<ProductDBModel?>(null);
            }

            return Task.FromResult<ProductDBModel?>(new ProductDBModel(
                product.Id,
                product.WarehouseId,
                product.Name,
                product.Quantity,
                product.Price,
                product.Category,
                product.Description));
        }

        public Task SaveProductAsync(ProductDBModel product)
        {
            ProductDBModel? existingProduct = _products.FirstOrDefault(item => item.Id == product.Id);

            if (existingProduct is null)
            {
                _products.Add(new ProductDBModel(
                    product.Id,
                    product.WarehouseId,
                    product.Name,
                    product.Quantity,
                    product.Price,
                    product.Category,
                    product.Description));

                return Task.CompletedTask;
            }

            existingProduct.WarehouseId = product.WarehouseId;
            existingProduct.Name = product.Name;
            existingProduct.Quantity = product.Quantity;
            existingProduct.Price = product.Price;
            existingProduct.Category = product.Category;
            existingProduct.Description = product.Description;

            return Task.CompletedTask;
        }

        public Task DeleteProductAsync(Guid productId)
        {
            _products.RemoveAll(item => item.Id == productId);
            return Task.CompletedTask;
        }
    }
}