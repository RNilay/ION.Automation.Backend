using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MySqlConnector; // ✅ MySQL exception handling

namespace IonFiltra.BagFilters.Infrastructure.Data
{
    public class TransactionHelper
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory;

        public TransactionHelper(IDbContextFactory<AppDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<T> ExecuteAsync<T>(Func<AppDbContext, Task<T>> action)
        {
            await using var dbContext = await _contextFactory.CreateDbContextAsync();
            await using var transaction = await dbContext.Database.BeginTransactionAsync();

            try
            {
                var result = await action(dbContext);
                await transaction.CommitAsync();
                return result;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<(int? entityId, string errorMessage)> ExecuteAsync(Func<AppDbContext, Task<(int? userId, string errorMessage)>> action)
        {
            await using var dbContext = await _contextFactory.CreateDbContextAsync();
            await using var transaction = await dbContext.Database.BeginTransactionAsync();

            try
            {
                var result = await action(dbContext);
                await transaction.CommitAsync();
                return result;
            }
            catch (DbUpdateException ex) when (ex.InnerException is MySqlException mySqlEx && mySqlEx.Number == 1062)
            {
                // ✅ Catch unique constraint violation (duplicate key in MySQL)
                await transaction.RollbackAsync();
                return (null, "Duplicate entry detected. The record already exists.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (null, $"An error occurred: {ex.Message}");
            }
        }

        public async Task ExecuteAsync(Func<AppDbContext, Task> action)
        {
            await using var dbContext = await _contextFactory.CreateDbContextAsync();
            await using var transaction = await dbContext.Database.BeginTransactionAsync();

            try
            {
                await action(dbContext);
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
