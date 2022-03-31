using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using GB.ASPNET.WebStore.Data;
using GB.ASPNET.WebStore.DAL.Context;
using GB.ASPNET.WebStore.Services.Interfaces;

namespace GB.ASPNET.WebStore.Services;

public class DbInitializer : IDbInitializer
{
    private readonly WebStoreDB _dbContext;
    private readonly ILogger<DbInitializer> _logger;

    public DbInitializer(WebStoreDB context, ILogger<DbInitializer> logger)
    {
        _dbContext = context;
        _logger = logger;
    }

    public async Task InitializeAsync(bool removeBefore = false, CancellationToken cancel = default)
    {
        _logger.LogInformation("Инициализация БД...");
        if (removeBefore) await RemoveAsync(cancel).ConfigureAwait(continueOnCapturedContext: false);

        //await _dbContext.Database.EnsureCreatedAsync().ConfigureAwait(false);

        IEnumerable<string>? pending_migrations = await _dbContext.Database.GetPendingMigrationsAsync().ConfigureAwait(false);
        if (pending_migrations.Any())
        {
            _logger.LogInformation("Миграция БД выполняется.");
            await _dbContext.Database.MigrateAsync(cancel);
            _logger.LogInformation("Миграция БД выполнена успешно.");
        }
        else
            _logger.LogInformation("Миграция БД не требуется.");

        await InitializeProductsAsync(cancel).ConfigureAwait(false);

        _logger.LogInformation("Инициализация БД выполнена успешно.");
    }

    private async Task InitializeProductsAsync(CancellationToken cancel = default)
    {
        if (await _dbContext.Products.AnyAsync(cancel).ConfigureAwait(false))
            _logger.LogInformation("Инициализация БД тестовыми данными не требуется.");
        else
        {
            _logger.LogInformation("Инициализация БД тестовыми данными...");

            _logger.LogInformation("Инициализация БД секциями...");
            await using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                await _dbContext.Sections.AddRangeAsync(TestData.Sections, cancel).ConfigureAwait(false);
                await _dbContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Sections] ON", cancel).ConfigureAwait(false);
                await _dbContext.SaveChangesAsync().ConfigureAwait(false);
                await _dbContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Sections] OFF", cancel).ConfigureAwait(false);
                await transaction.CommitAsync(cancel).ConfigureAwait(false);
            }

            _logger.LogInformation("Инициализация БД брендами...");
            await using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                await _dbContext.Brands.AddRangeAsync(TestData.Brands, cancel).ConfigureAwait(false);
                await _dbContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[ProductBrands] ON", cancel).ConfigureAwait(false);
                await _dbContext.SaveChangesAsync().ConfigureAwait(false);
                await _dbContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[ProductBrands] OFF", cancel).ConfigureAwait(false);
                await transaction.CommitAsync(cancel).ConfigureAwait(false);
            }

            _logger.LogInformation("Инициализация БД товарами...");
            await using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                await _dbContext.Products.AddRangeAsync(TestData.Products, cancel).ConfigureAwait(false);
                await _dbContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Products] ON", cancel).ConfigureAwait(false);
                await _dbContext.SaveChangesAsync().ConfigureAwait(false);
                await _dbContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Products] OFF", cancel).ConfigureAwait(false);
                await transaction.CommitAsync(cancel).ConfigureAwait(false);
            }

            _logger.LogInformation("Инициализация БД тестовыми данными завершена.");
        }
    }

    public async Task<bool> RemoveAsync(CancellationToken cancel = default)
    {
        _logger.LogInformation("Удаление БД...");
        bool removed = await _dbContext.Database.EnsureDeletedAsync(cancel).ConfigureAwait(false);

        if (removed) _logger.LogInformation("Удаление БД: успешно.");
        else _logger.LogInformation("Удаление БД не требуется.");
        return removed;
    }
}