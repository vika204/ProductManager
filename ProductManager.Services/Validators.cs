using ProductManager.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProductManager.Services
{
    public static class Validators
    {
        public static List<ValidationResult> ValidateWarehouse(string? name, WarehouseLocation? location)
        {
            List<ValidationResult> errors = new();

            // warehouse name must be present and meaningful.
            if (string.IsNullOrWhiteSpace(name))
            {
                errors.Add(new ValidationResult("Warehouse name is required.", new[] { "Name" }));
            }
            else if (name.Trim().Length < 2)
            {
                errors.Add(new ValidationResult("Warehouse name must contain at least 2 characters.", new[] { "Name" }));
            }

            // location must always be selected because it is part of business model.
            if (location is null)
            {
                errors.Add(new ValidationResult("Warehouse location is required.", new[] { "Location" }));
            }

            return errors;
        }

        public static List<ValidationResult> ValidateProduct(
            Guid warehouseId,
            string? name,
            int? quantity,
            decimal? price,
            ProductCategory? category,
            string? description)
        {
            List<ValidationResult> errors = new();

            // product cannot exist without warehouse.
            if (warehouseId == Guid.Empty)
            {
                errors.Add(new ValidationResult("Warehouse must be selected.", new[] { "WarehouseId" }));
            }

            // product name is required for identification and search.
            if (string.IsNullOrWhiteSpace(name))
            {
                errors.Add(new ValidationResult("Product name is required.", new[] { "Name" }));
            }
            else if (name.Trim().Length < 2)
            {
                errors.Add(new ValidationResult("Product name must contain at least 2 characters.", new[] { "Name" }));
            }

            // quantity cannot be missing or negative.
            if (quantity is null)
            {
                errors.Add(new ValidationResult("Quantity is required.", new[] { "Quantity" }));
            }
            else if (quantity < 0)
            {
                errors.Add(new ValidationResult("Quantity cannot be negative.", new[] { "Quantity" }));
            }

            // price cannot be missing or negative.
            if (price is null)
            {
                errors.Add(new ValidationResult("Price is required.", new[] { "Price" }));
            }
            else if (price < 0)
            {
                errors.Add(new ValidationResult("Price cannot be negative.", new[] { "Price" }));
            }

            // category is required
            if (category is null)
            {
                errors.Add(new ValidationResult("Category is required.", new[] { "Category" }));
            }

            //description is required
            if (string.IsNullOrWhiteSpace(description))
            {
                errors.Add(new ValidationResult("Description is required.", new[] { "Description" }));
            }

            return errors;
        }
    }
}