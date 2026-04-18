using ProductManager.DBModels;
using SQLite;
using System.Threading;

namespace ProductManager.Storage
{
    public class SQLiteStorageContext : IStorageContext
    {
        private const string DatabaseFileName = "product_manager.db3";

        // localApplicationData is used so the database is stored locally
        // and does not require any external database server.
        private static readonly string DatabasePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "ProductManagerStorage",
            DatabaseFileName);

        // semaphore protects database access from concurrent async calls.
        private readonly SemaphoreSlim _locker = new(1, 1);

        private SQLiteAsyncConnection? _databaseConnection;

        private async Task Init()
        {
            // database connection is created only once.
            if (_databaseConnection is not null)
            {
                return;
            }

            bool isFirstLaunch = !File.Exists(DatabasePath);

            string? directory = Path.GetDirectoryName(DatabasePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory!);
            }

            _databaseConnection = new SQLiteAsyncConnection(DatabasePath);

            // create database tables if they do not exist yet.
            await _databaseConnection.CreateTableAsync<WarehouseDBModel>();
            await _databaseConnection.CreateTableAsync<ProductDBModel>();

            // initial seed is executed only on first launch.
            if (isFirstLaunch)
            {
                await CreateMockStorage();
            }
        }

        private async Task CreateMockStorage()
        {
            // initial test data is prepared in memory and then copied to SQLite.
            InMemoryStorageContext inMemoryStorage = new();

            IEnumerable<WarehouseDBModel> warehouses = await inMemoryStorage.GetWarehousesAsync();
            foreach (WarehouseDBModel warehouse in warehouses)
            {
                await _databaseConnection!.InsertAsync(warehouse);

                IEnumerable<ProductDBModel> products = await inMemoryStorage.GetProductsByWarehouseAsync(warehouse.Id);
                await _databaseConnection.InsertAllAsync(products);
            }
        }

        public async Task<IEnumerable<WarehouseDBModel>> GetWarehousesAsync()
        {
            await _locker.WaitAsync();

            try
            {
                await Init();
                return await _databaseConnection!.Table<WarehouseDBModel>().ToListAsync();
            }
            finally
            {
                _locker.Release();
            }
        }

        public async Task<WarehouseDBModel?> GetWarehouseAsync(Guid warehouseId)
        {
            await _locker.WaitAsync();

            try
            {
                await Init();

                return await _databaseConnection!.Table<WarehouseDBModel>()
                    .FirstOrDefaultAsync(warehouse => warehouse.Id == warehouseId);
            }
            finally
            {
                _locker.Release();
            }
        }

        public async Task SaveWarehouseAsync(WarehouseDBModel warehouse)
        {
            await _locker.WaitAsync();

            try
            {
                await Init();

                WarehouseDBModel? existingWarehouse = await _databaseConnection!.Table<WarehouseDBModel>()
                    .FirstOrDefaultAsync(item => item.Id == warehouse.Id);

                if (existingWarehouse is null)
                {
                    await _databaseConnection.InsertAsync(warehouse);
                    return;
                }

                await _databaseConnection.UpdateAsync(warehouse);
            }
            finally
            {
                _locker.Release();
            }
        }

        // when warehouse is deleted, all products that belong to that warehouse are also deleted
        public async Task DeleteWarehouseAsync(Guid warehouseId)
        {
            await _locker.WaitAsync();
            try
            {
                await Init();
                await _databaseConnection!.Table<ProductDBModel>()
                    .DeleteAsync(p => p.WarehouseId == warehouseId);
                await _databaseConnection.DeleteAsync<WarehouseDBModel>(warehouseId);
            }
            finally { _locker.Release(); }
        }


        // products are queried by warehouse id, so we can get all products for a specific warehouse.
        public async Task<IEnumerable<ProductDBModel>> GetProductsByWarehouseAsync(Guid warehouseId)
        {
            await _locker.WaitAsync();

            try
            {
                await Init();

                return await _databaseConnection!.Table<ProductDBModel>()
                    .Where(product => product.WarehouseId == warehouseId)
                    .ToListAsync();
            }
            finally
            {
                _locker.Release();
            }
        }

        public async Task<ProductDBModel?> GetProductAsync(Guid productId)
        {
            await _locker.WaitAsync();

            try
            {
                await Init();

                return await _databaseConnection!.Table<ProductDBModel>()
                    .FirstOrDefaultAsync(product => product.Id == productId);
            }
            finally
            {
                _locker.Release();
            }
        }

        public async Task SaveProductAsync(ProductDBModel product)
        {
            await _locker.WaitAsync();

            try
            {
                await Init();

                ProductDBModel? existingProduct = await _databaseConnection!.Table<ProductDBModel>()
                    .FirstOrDefaultAsync(item => item.Id == product.Id);

                if (existingProduct is null)
                {
                    await _databaseConnection.InsertAsync(product);
                    return;
                }

                await _databaseConnection.UpdateAsync(product);
            }
            finally
            {
                _locker.Release();
            }
        }

        public async Task DeleteProductAsync(Guid productId)
        {
            await _locker.WaitAsync();

            try
            {
                await Init();
                await _databaseConnection!.DeleteAsync<ProductDBModel>(productId);
            }
            finally
            {
                _locker.Release();
            }
        }
    }
}