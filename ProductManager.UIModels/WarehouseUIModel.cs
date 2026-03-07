using ProductManager.Common.Enums;
using ProductManager.DBModels;
using ProductManager.Services;

namespace ProductManager.UIModels
{
    public class WarehouseUIModel
    {
        private WarehouseDBModel? _warehouseDBModel;
        private string _name = string.Empty;
        private WarehouseLocation _location;

        // null means products were not loaded yet
        private List<ProductUIModel>? _products;

        // storage is needed to load related products for this warehouse
        private readonly IStorageService _storage;
        public Guid? Id
        {
            get => _warehouseDBModel?.Id;
        }
        public string Name
        {
            get => _name;
            set => _name = value;
        }
        public WarehouseLocation Location
        {
            get => _location;
            set => _location = value;
        }

        // products are exposed as read-only for the ui layer
        public IReadOnlyList<ProductUIModel>? Products
        {
            get => _products;

        }

        public int TotalProducts
        {
            get => Products?.Count ?? -1;
        }

        // shows a friendly text when products are not loaded yet
        public string TotalProductsDesc
        {
            get => TotalProducts == -1 ? "Not Loaded" : TotalProducts.ToString();
        }

        public decimal TotalValue
        {
            get => Products?.Sum(product => product.TotalValue) ?? -1m;
        }

        // shows a friendly text when total value cannot be calculated yet
        public string TotalValueDesc
        {
            get => TotalValue == -1m ? "Not Loaded" : TotalValue.ToString("0.00");
        }

        // this constructor is used when a new warehouse is created in ui
        public WarehouseUIModel(IStorageService storage)
        {
            _storage = storage;
        }


        // this constructor wraps an existing db model for ui usage
        public WarehouseUIModel(IStorageService storage, WarehouseDBModel dbModel)
        {
            _storage = storage;
            _warehouseDBModel = dbModel;
            _name = dbModel.Name;
            _location = dbModel.Location;
        }

        // updates an existing db model or creates a new one
        public void SaveChangesToDBModel()
        {
            // if db model already exists, update its values
            if (_warehouseDBModel != null)
            {
                _warehouseDBModel.Name = _name;
                _warehouseDBModel.Location = _location;
            }
            // otherwise create a new db model from ui data
            else
            {
                _warehouseDBModel = new WarehouseDBModel(_name, _location);
            }
        }

        // loads related products only once
        public void LoadProducts()
        {
            // skip loading if warehouse id is missing or products are already loaded
            if (Id == null || _products != null)
            {
                return;
            }

            _products = new List<ProductUIModel>();

            // convert db models into ui models for display
            foreach (ProductDBModel productDBModel in _storage.GetProducts(Id.Value))
            {
                _products.Add(new ProductUIModel(productDBModel));
            }
        }
        public override string ToString()
        {
            return $"Warehouse: {Name}, Location: {Location}, Products: {TotalProducts}";
        }

    }
}
