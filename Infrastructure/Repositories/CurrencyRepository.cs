using Financer.Domain.Entities;
using Financer.Domain.Repositories;
using Financer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Financer.Infrastructure.Repositories;

public class CurrencyRepository(AppDbContext db) : ICurrencyRepository
{
    public async Task AddAsync(Currency currency)
    {
        ArgumentNullException.ThrowIfNull(currency);

        await db.Currencies.AddAsync(currency);
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await db.Currencies.FindAsync(id);
        if (entity == null)
            return;

        db.Currencies.Remove(entity);
        await db.SaveChangesAsync();
    }

    public async Task<IEnumerable<Currency>> GetAllAsync()
    {
        return await db.Currencies.AsNoTracking().ToListAsync();
    }

    public async Task<Currency?> GetByIdAsync(Guid id)
    {
        return await db.Currencies.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Currency?> GetByCodeAsync(string code)
    {
        return await db.Currencies.AsNoTracking().FirstOrDefaultAsync(c => c.Code == code);
    }

    public async Task UpdateAsync(Currency currency)
    {
        ArgumentNullException.ThrowIfNull(currency);

        db.Currencies.Update(currency);
        await db.SaveChangesAsync();
    }
}
