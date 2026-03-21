using ProductManager.DBModels;
using ProductManager.Storage;

namespace ProductManager.Repostory
{
    // product repository is a thin wrapper around storage context
    // it hides storage implementation from service layer
    public class ProductRepository : IProductRepository
    {
        // storage context provides raw db models
        private readonly IStorageContext _storage;

        public ProductRepository(IStorageContext storage)
        {
            _storage = storage;
        }

        // return products for selected warehouse
        public IEnumerable<ProductDBModel> GetProducts(Guid warehouseId)
        {
            return _storage.GetProducts(warehouseId);
        }

        // return one product by id
        public ProductDBModel GetProduct(Guid productId)
        {
            return _storage.GetProduct(productId);
        }
    }
}