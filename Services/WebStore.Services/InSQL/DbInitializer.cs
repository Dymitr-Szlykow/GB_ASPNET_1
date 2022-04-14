using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using GB.ASPNET.WebStore.Data;
using GB.ASPNET.WebStore.DAL.Context;
using GB.ASPNET.WebStore.Domain.Entities.Identity;
using GB.ASPNET.WebStore.Interfaces;

namespace GB.ASPNET.WebStore.Services;

public class DbInitializer : IDbInitializer
{
    private readonly WebStoreDB _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly ILogger<DbInitializer> _logger;

    public DbInitializer(
        WebStoreDB dbContext,
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        ILogger<DbInitializer> logger)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    public async Task InitializeAsync(bool removeBefore = false, CancellationToken cancel = default)
    {
        _logger.LogInformation("Инициализация БД...");
        if (removeBefore) await RemoveAsync(cancel).ConfigureAwait(continueOnCapturedContext: false);

        IEnumerable<string>? pending_migrations = await _dbContext.Database.GetPendingMigrationsAsync(cancel).ConfigureAwait(false);
        if (pending_migrations.Any())
        {
            _logger.LogInformation("Миграция БД выполняется.");
            await _dbContext.Database.MigrateAsync(cancel);
            _logger.LogInformation("Миграция БД выполнена успешно.");
        }
        else
            _logger.LogInformation("Миграция БД не требуется.");

        await InitializeCatalogAsync(cancel).ConfigureAwait(false);
        await InitializeEmployeesAsync(cancel).ConfigureAwait(false);
        await InitializeIdentitiesAsync(cancel).ConfigureAwait(false);
        _logger.LogInformation("Инициализация БД выполнена успешно.");
    }

    private async Task InitializeCatalogAsync(CancellationToken cancel = default)
    {
        if (await _dbContext.Products.AnyAsync(cancel).ConfigureAwait(false))
            _logger.LogInformation("Инициализация БД тестовыми данными не требуется.");
        else
        {
            _logger.LogInformation("Инициализация БД тестовыми данными...");
            var (sectionsForSql, brandsForSql, productsForSql) = TestData.CleanHardCodeAndGetCatalogForSql();
            await using (var transaction = await _dbContext.Database.BeginTransactionAsync(cancel))
            {
                await _dbContext.AddRangeAsync(sectionsForSql, cancel).ConfigureAwait(false);
                await _dbContext.AddRangeAsync(brandsForSql, cancel).ConfigureAwait(false);
                await _dbContext.AddRangeAsync(productsForSql, cancel).ConfigureAwait(false);
                await _dbContext.SaveChangesAsync(cancel).ConfigureAwait(false);
                await transaction.CommitAsync(cancel).ConfigureAwait(false);
            }
            _logger.LogInformation("Инициализация БД тестовыми данными завершена.");
        }
    }

    private async Task InitializeEmployeesAsync(CancellationToken cancel = default)
    {
        if (await _dbContext.Employees.AnyAsync(cancel).ConfigureAwait(false))
            _logger.LogInformation("Инициализация БД сотрудниками не требуется.");
        else
        {
            _logger.LogInformation("Инициализация БД сотрудниками...");
            TestData.CleanHardCodeInEmployees();
            await using (var transaction = await _dbContext.Database.BeginTransactionAsync(cancel))
            {
                await _dbContext.AddRangeAsync(TestData.Employees, cancel).ConfigureAwait(false);
                await _dbContext.SaveChangesAsync(cancel).ConfigureAwait(false);
                await transaction.CommitAsync(cancel).ConfigureAwait(false);
            }
            _logger.LogInformation("Инициализация БД сотрудниками завершена.");
        }
    }

    private async Task InitializeIdentitiesAsync(CancellationToken cancel = default)
    {
        await InitializeIdentitiesRoleAsync(Role.administrators);
        await InitializeIdentitiesRoleAsync(Role.users);
        await InitializeIdentitiesUserAsync(User.administrator, User.defaultAdminPassword, Role.administrators, dieOnError: true);
    }

    private async Task InitializeIdentitiesRoleAsync(string roleName)
    {
        if (await _roleManager.RoleExistsAsync(roleName))
            _logger.LogInformation("Инициализация БД личностями: роль \"{0}\" существует.", roleName);
        else
        {
            _logger.LogInformation("Инициализация БД личностями: создание роли \"{0}\"...", roleName);
            IdentityResult? result = await _roleManager.CreateAsync(new Role { Name = roleName });
            _logger.LogInformation(
                result.Succeeded
                    ? "Инициализация БД личностями: роль \"{0}\" создана."
                    : "Инициализация БД личностями: создание роли \"{0}\": безуспешно.",
                roleName);
        }
    }

    private async Task InitializeIdentitiesUserAsync(string userName, string password, string? role = null, bool dieOnError = false)
    {
        if (await _userManager.FindByNameAsync(userName) is not null)
            _logger.LogInformation("Инициализация БД личностями: пользователь \"{0}\" существует.", userName);
        else
        {
            _logger.LogInformation("Инициализация БД личностями: создание пользователя \"{0}\"...", userName);
            User? admin = new User { UserName = userName };

            var creationResult = await _userManager.CreateAsync(admin, password);
            if (creationResult.Succeeded)
            {
                _logger.LogInformation("Инициализация БД личностями: пользователь \"{0}\" создан.", userName);
                if (role is not null)
                {
                    var addResult = await _userManager.AddToRoleAsync(admin, role);
                    _logger.LogInformation(
                        addResult.Succeeded
                            ? "Инициализация БД личностями: пользователь \"{0}\" наделен правами \"{1}\"."
                            : "Инициализация БД личностями: пользователя \"{0}\" не удалось наделить правами \"{1}\".",
                        userName, role);
                }
            }
            else if (dieOnError)
            {
                var errors = string.Join(" ", creationResult.Errors.Select(exc => exc.Description));
                _logger.LogError("Ошибюка при инициализации БД - при создании пользователя \"{0}\". Ошибки: {1}", userName, errors);
                throw new InvalidOperationException($"Невозможно создать пользователя {userName}. Ошибки: {errors}");
            }
        }
    }

    public async Task<bool> RemoveAsync(CancellationToken cancel = default)
    {
        _logger.LogInformation("Удаление БД...");
        bool removed = await _dbContext.Database.EnsureDeletedAsync(cancel).ConfigureAwait(false);
        _logger.LogInformation(
            removed
                ? "Удаление БД: успешно."
                : "Удаление БД не требуется.");
        return removed;
    }
}