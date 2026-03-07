using ProductManager.Common.Enums;
using ProductManager.DBModels;

namespace ProductManager.UIModels
{
    public class ProductUIModel
    {
        // keeps reference to the original db model if it already exists
        private ProductDBModel? _productDBModel;
        private Guid _warehouseId;
        private string _name = string.Empty;
        private int _quantity;
        private decimal _price;
        private ProductCategory _productCategory;
        private string _description = string.Empty;

        public Guid? Id
        {
            get => _productDBModel?.Id;
        }
        public Guid WarehouseId
        {
            get => _warehouseId;
            set => _warehouseId = value;
        }
        public string Name
        {
            get => _name;
            set => _name = value;
        }
        public int Quantity
        {
            get => _quantity;
            set => _quantity = value;
        }
        public decimal Price
        {
            get => _price;
            set => _price = value;
        }

        public ProductCategory Category
        {
            get => _productCategory;
            set => _productCategory = value;
        }
        public string Description
        {
            get => _description;
            set => _description = value;
        }

        public decimal TotalValue
        {
            get => _quantity * _price;
        }
        public string PriceDesc
        {
            get => Price.ToString("0.00");
        }

        public string TotalValueDesc
        {
            get => TotalValue.ToString("0.00");
        }


        // this constructor is used when a new product is created for a warehouse
        public ProductUIModel(Guid warehouseId)
        {
            _warehouseId = warehouseId;
        }

        // this constructor wraps an existing db model for ui usage
        public ProductUIModel(ProductDBModel dbModel)
        {
            _productDBModel = dbModel;
            _warehouseId = dbModel.WarehouseId;
            _name = dbModel.Name;
            _quantity = dbModel.Quantity;
            _price = dbModel.Price;
            _productCategory = dbModel.Category;
            _description = dbModel.Description;
        }


        // updates an existing db model or creates a new one
        public void SaveChangesToDBModel()
        {
            // if db model already exists, update its values
            if (_productDBModel != null)
            {
                _productDBModel.WarehouseId = _warehouseId;
                _productDBModel.Name = _name;
                _productDBModel.Quantity = _quantity;
                _productDBModel.Price = _price;
                _productDBModel.Category = _productCategory;
                _productDBModel.Description = _description;

            }
            // otherwise create a new db model from ui data
            else
            {
                _productDBModel = new ProductDBModel(_warehouseId, _name, _quantity, _price, _productCategory, _description);
            }
        }
        public override string ToString()
        {
            return $"Product: {Name}, Category: {Category}, Total value: {TotalValue}";
        }
    }
}
