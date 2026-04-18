# ProductManager (Lab 7.4)

.NET MAUI application for managing warehouses and products.

## Description

`ProductManager` is a multi-layered application for warehouse and product management.

The application supports:
- viewing all warehouses;
- searching and sorting warehouses;
- adding, editing, viewing details, and deleting warehouses;
- viewing products that belong to a selected warehouse;
- searching and sorting products;
- adding, editing, viewing details, and deleting products;
- calculating total value for a product;
- calculating total value of all products stored in a warehouse.

A product always belongs to exactly one warehouse.

## Lab 7.4 implementation

This project includes:

- **MVVM**
  - UI logic is implemented in ViewModels
  - Pages use bindings and commands
  - Code-behind is minimal and used only where necessary

- **Layered architecture**
  - `ProductManager` — UI layer
  - `ProductManager.Services` — service layer
  - `ProductManager.Storage` — storage layer
  - `ProductManager.DTOModels` — DTO models
  - `ProductManager.DBModels` — database models
  - `ProductManager.Repostory` — repository layer

- **Dependency Injection / IoC**
  - storage, repositories, services, pages, and view models are registered in `MauiProgram`

- **Navigation**
  - Shell-based navigation:
    - warehouses list
    - warehouse details
    - warehouse create/edit
    - product details
    - product create/edit

- **Real storage**
  - SQLite is used as persistent local storage
  - on first launch the database is initialized with seed data
  - initial seed data is taken from `InMemoryStorageContext`
  - deleting a warehouse also deletes all related products

- **Asynchronous operations**
  - interaction with storage is implemented with `async/await`
  - UI remains responsive during loading and saving operations

## Main entities

### Warehouse
- Id
- Name
- Location
- Collection of products
- Total value of products in warehouse

### Product
- Id
- WarehouseId
- Name
- Quantity
- Price
- Category
- Description
- Total value (`Price * Quantity`)

## Technologies

- .NET MAUI
- C#
- CommunityToolkit.Mvvm
- SQLite
- Microsoft.Extensions.DependencyInjection

## Project structure

- `ProductManager` — UI
- `ProductManager.Common` — shared helpers
- `ProductManager.DTOModels` — DTOs
- `ProductManager.DBModels` — storage models
- `ProductManager.Repostory` — repositories
- `ProductManager.Services` — business logic
- `ProductManager.Storage` — storage context

## Run

1. Open `ProductManager.slnx`
2. Set startup project to `ProductManager`
3. Select target platform:
   - `Windows Machine`
4. Run the project
