# ProductManager

A console application for managing warehouse products, built with C# following layered architecture and Single Responsibility principles.

---

## Project Structure

```
ProductManager/
├── ProductManager.Common/        # Shared enums
├── ProductManager.DBModels/      # Storage models
├── ProductManager.UIModels/      # UI / interaction models
├── ProductManager.Services/      # FakeStorage and StorageService
└── ProductManager.ConsoleApp/    # Console application entry point
```

### ProductManager.Common
Contains shared enumerations used across the solution:
- `ProductCategory` — Electronics, Clothing, Books, Toys, Food
- `WarehouseLocation` — Kyiv, Lviv, Odesa, Kharkiv, Dnipro

### ProductManager.DBModels
Contains classes responsible for **data storage**:
- `WarehouseDBModel` — stores warehouse Id, Name, Location
- `ProductDBModel` — stores product Id, WarehouseId, Name, Quantity, Price, Category, Description

These classes do not contain computed fields or references to each other's collections.

### ProductManager.UIModels
Contains classes responsible for **display, creation and editing**:
- `WarehouseUIModel` — wraps `WarehouseDBModel`, holds a list of products, exposes computed `TotalProducts`, supports lazy-loading via `LoadProducts()`
- `ProductUIModel` — wraps `ProductDBModel`, exposes computed `TotalValue` (Price × Quantity), supports editing and saving changes back to the DB model via `SaveChangesToDBModel()`

### ProductManager.Services
Contains:
- `FakeStorage` — internal static class that initializes and holds in-memory data (warehouses and products). Accessible only within the Services project.
- `StorageService` — public class that provides access to data from `FakeStorage`. Acts as the single entry point for data retrieval.

### ProductManager.ConsoleApp
The console application that ties everything together.

---

## Application Logic

### On startup
The main menu is displayed immediately with the following options:
- `1` — Show all warehouses
- `2` — Select a warehouse
- `0` — Exit

### Warehouse list
Warehouses are loaded from `StorageService` on first access and cached in memory. Each warehouse is displayed with its name and location.

### Warehouse details
When a warehouse is selected by number:
- Its name, location and total product count are displayed
- Products are loaded lazily — only when the warehouse is first opened
- Each product is listed with name, category and price

### Product details
When a product is selected by number, full details are shown:
- Category, Price, Quantity, Total Value (Price × Quantity), Description

### Navigation
- At every level the user can type `0` to go back
- The application loops until the user exits via `0` from the main menu

---

## Data

The fake storage contains:
- **3 warehouses**: Books (Lviv), Electronics (Kyiv), Clothing (Odesa)
- **20 products** in total:
  - 10 products in the Books warehouse
  - 5 products in the Electronics warehouse
  - 5 products in the Clothing warehouse

---

## How to Run

1. Clone the repository
2. Open `ProductManager.sln` in Visual Studio
3. Set `ProductManager.ConsoleApp` as the startup project
4. Press `F5` or run `dotnet run` from the `ProductManager.ConsoleApp` directory