using ProductManager.Common.Enums;
using ProductManager.DBModels;
using ProductManager.Services;

namespace ProductManager.UIModels
{
    public class WarehouseUIModel
    {
        private WarehouseDBModel _warehouseDBModel;
        private string _name;
        private WarehouseLocation _location;
        private List<ProductUIModel> _products;
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

        public IReadOnlyList<ProductUIModel> Products
        {
            get => _products;

        }

        public int TotalProducts
        {
            get => Products?.Count ?? 0;
        }

        public WarehouseUIModel()
        {

            _products = new List<ProductUIModel>();
        }


        public WarehouseUIModel(WarehouseDBModel dbModel) : this()
        {
            _warehouseDBModel = dbModel;
            _name = dbModel.Name;
            _location = dbModel.Location;
        }

        public void SaveChangesToDBModel()
        {
            if (_warehouseDBModel != null)
            {
                _warehouseDBModel.Name = _name;
                _warehouseDBModel.Location = _location;
            }
            else
            {
                _warehouseDBModel = new WarehouseDBModel(_name, _location);
            }
        }

        public void LoadProducts(StorageService storage)
        {
            if (Id == null || _products.Count > 0)
            {
                return;
            }

            foreach (ProductDBModel productDBModel in storage.GetProducts(Id.Value))
            {
                _products.Add(new ProductUIModel(productDBModel));
            }
        }

    }
}
