using ProductManager.DBModels;
using ProductManager.Storage;

namespace ProductManager.Repostory
{
    // product repository is a thin wrapper around storage context
    // it hides storage implementation from service layer
    public class ProductRepository : IProductRepository
    {
        private readonly IStorageContext _storage;

        public ProductRepository(IStorageContext storage)
        {
            _storage = storage;
        }

        // return products for selected warehouse
        public Task<IEnumerable<ProductDBModel>> GetProductsByWarehouseAsync(Guid warehouseId)
        {
            return _storage.GetProductsByWarehouseAsync(warehouseId);
        }

        // return one product by id
        public Task<ProductDBModel?> GetProductAsync(Guid productId)
        {
            return _storage.GetProductAsync(productId);
        }

        // save product
        public Task SaveProductAsync(ProductDBModel product)
        {
            return _storage.SaveProductAsync(product);
        }

        // delete product by id
        public Task DeleteProductAsync(Guid productId)
        {
            return _storage.DeleteProductAsync(productId);
        }
    }
}