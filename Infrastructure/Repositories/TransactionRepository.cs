using Financer.Domain.Entities;
using Financer.Domain.Repositories;
using Financer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Financer.Infrastructure.Repositories;

public class TransactionRepository(AppDbContext db) : ITransactionRepository
{
    public async Task AddAsync(Transaction transaction)
    {
        ArgumentNullException.ThrowIfNull(transaction);

        await db.Transactions.AddAsync(transaction);
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await db.Transactions.FindAsync(id);
        if (entity == null)
            return; // idempotent - nothing to delete

        db.Transactions.Remove(entity);
        await db.SaveChangesAsync();
    }

    public async Task<IEnumerable<Transaction>> GetAllAsync()
    {
        return await db.Transactions.AsNoTracking().ToListAsync();
    }

    public async Task<Transaction?> GetByIdAsync(Guid id)
    {
        return await db.Transactions.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<Transaction>> GetByAccountIdAsync(Guid accountId)
    {
        return await db
            .Transactions.AsNoTracking()
            .Where(t => t.AccountId == accountId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Transaction>> GetByDateRangeAsync(
        DateTime startDate,
        DateTime endDate
    )
    {
        return await db
            .Transactions.AsNoTracking()
            .Where(t => t.Date >= startDate && t.Date <= endDate)
            .ToListAsync();
    }

    public async Task UpdateAsync(Transaction transaction)
    {
        ArgumentNullException.ThrowIfNull(transaction);

        db.Transactions.Update(transaction);
        await db.SaveChangesAsync();
    }
}
