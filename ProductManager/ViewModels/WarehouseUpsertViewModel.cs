using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProductManager.Common;
using ProductManager.Common.Enums;
using ProductManager.DTOModels.Warehouse;
using ProductManager.Services;
using System.ComponentModel.DataAnnotations;

namespace ProductManager.ViewModels
{
    public partial class WarehouseUpsertViewModel : BaseViewModel, IQueryAttributable
    {
        private readonly IWarehouseService _warehouseService;

        // if id is null, the page works in create mode.
        // if id has value, the page works in edit mode.
        private Guid? _warehouseId;

        [ObservableProperty]
        private string _pageTitle;

        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private EnumWithName<WarehouseLocation>? _location;

        [ObservableProperty]
        private string _nameError;

        [ObservableProperty]
        private string _locationError;

        // values for Picker
        public EnumWithName<WarehouseLocation>[] Locations { get; }

        public WarehouseUpsertViewModel(IWarehouseService warehouseService)
        {
            _warehouseService = warehouseService;

            _pageTitle = "Create Warehouse";
            _name = string.Empty;
            _nameError = string.Empty;
            _locationError = string.Empty;

            Locations = EnumExtensions.GetValuesWithNames<WarehouseLocation>();
            Location = Locations.FirstOrDefault();
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            // if WarehouseId exists, page switches to edit mode
            if (query.TryGetValue("WarehouseId", out object? warehouseId))
            {
                _warehouseId = (Guid)warehouseId;
                PageTitle = "Edit Warehouse";
            }
        }

        [RelayCommand]
        internal async Task RefreshData()
        {
            // in create mode there is nothing to preload
            if (_warehouseId is null || IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                WarehouseUpsertDTO warehouse = await _warehouseService.GetWarehouseForEditAsync(_warehouseId.Value)
                    ?? throw new InvalidOperationException("Warehouse does not exist.");

                Name = warehouse.Name;
                Location = Locations.FirstOrDefault(item => item.Value.Equals(warehouse.Location));
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", $"Failed to load warehouse form: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task SaveWarehouse()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            // clear old validation messages before new validation pass
            ResetErrors();

            try
            {
                List<ValidationResult> validationErrors = Validators.ValidateWarehouse(Name, Location?.Value);

                if (validationErrors.Count > 0)
                {
                    FillErrors(validationErrors);
                    return;
                }

                WarehouseUpsertDTO warehouse = new(Name.Trim(), Location!.Value);

                if (_warehouseId is null)
                {
                    await _warehouseService.CreateWarehouseAsync(warehouse);
                }
                else
                {
                    await _warehouseService.UpdateWarehouseAsync(_warehouseId.Value, warehouse);
                }

                // return to previous page after successful save
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", $"Failed to save warehouse: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task Back()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", $"Failed to navigate back: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void ResetErrors()
        {
            NameError = string.Empty;
            LocationError = string.Empty;
        }

        private void FillErrors(IEnumerable<ValidationResult> validationErrors)
        {
            foreach (ValidationResult error in validationErrors)
            {
                foreach (string memberName in error.MemberNames)
                {
                    switch (memberName)
                    {
                        case nameof(Name):
                            NameError = AppendError(NameError, error.ErrorMessage);
                            break;

                        case nameof(Location):
                            LocationError = AppendError(LocationError, error.ErrorMessage);
                            break;
                    }
                }
            }
        }

        private static string AppendError(string currentValue, string? newError)
        {
            if (string.IsNullOrWhiteSpace(newError))
            {
                return currentValue;
            }

            if (string.IsNullOrWhiteSpace(currentValue))
            {
                return newError;
            }

            return currentValue + Environment.NewLine + newError;
        }
    }
}