using ProductManager.DBModels;

namespace ProductManager.Services
{
    public class StorageService: IStorageService
    {
        // data is loaded only once and then reused
        private List<WarehouseDBModel>? _warehouses;
        private List<ProductDBModel>? _products;

        // loads fake data only on first request
        private void LoadData()
        {
            // skip loading if data is already in memory
            if (_warehouses != null && _products != null)
            {
                return;
            }
            // copy fake storage data into local collections
            _warehouses = FakeStorage.Warehouses.ToList();
            _products = FakeStorage.Products.ToList();
        }

        // returns a separate list of warehouses
        public IEnumerable<WarehouseDBModel> GetWarehouses()
        {
            LoadData();

            // create a new list to avoid returning internal storage directly
            List<WarehouseDBModel> resultList = new List<WarehouseDBModel>();

            foreach (WarehouseDBModel warehouse in _warehouses!)
            {
                resultList.Add(warehouse);
            }

            return resultList;
        }

        // returns products only for the selected warehouse
        public IEnumerable<ProductDBModel> GetProducts(Guid warehouseId)
        {
            LoadData();

            List<ProductDBModel> resultList = new List<ProductDBModel>();

            foreach (ProductDBModel product in _products!)
            {
                // filter products by warehouse id
                if (product.WarehouseId == warehouseId)
                {
                    resultList.Add(product);
                }
            }

            return resultList;
        }
    }
}