# ProductManager (Lab 7.3)

.NET MAUI application for managing warehouses and products.

Lab 7.3 implementation:
- MVVM: logic is in ViewModels, Pages code-behind only sets `BindingContext`
- DI/IoC: Storage/Repositories/Services/ViewModels are registered in `MauiProgram`
- Layered architecture: Storage → Repostory → Services → DTOModels → UI
- Shell navigation: Warehouses → Warehouse details (products) → Product details
- Test data: `InMemoryStorageContext`

Run: open `ProductManager.slnx`, set startup project `ProductManager` (MAUI), select `Windows Machine`, press Run.
