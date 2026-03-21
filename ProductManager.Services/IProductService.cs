using ProductManager.DTOModels.Product;

namespace ProductManager.Services
{
    // product service provides product data for ui
    // it returns dto models and does not expose db models
    public interface IProductService
    {
        // list products for a selected warehouse
        IEnumerable<ProductListDTO> GetProductsByWarehouse(Guid warehouseId);

        // product details page data
        ProductDetailsDTO GetProduct(Guid productId);
    }
}