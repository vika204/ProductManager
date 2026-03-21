using ProductManager.DBModels;

namespace ProductManager.Repostory
{
    // product repository provides access to product db models
    // it does not contain business logic and does not return dto
    public interface IProductRepository
    {
        // return products for selected warehouse
        IEnumerable<ProductDBModel> GetProducts(Guid warehouseId);

        // return one product by id
        ProductDBModel GetProduct(Guid productId);
    }
}