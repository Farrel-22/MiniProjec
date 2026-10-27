using Microsoft.EntityFrameworkCore;
using MiniProject.Common.ViewModel;
using MiniProject.DAL.ScaffoldContainer;
using MiniProject.Services.Transaction.Interface;

namespace MiniProject.Services.Transaction.Service;

public class BudgetService : IBudgetService
{
    private readonly Latihan1Context _context;

    public BudgetService(Latihan1Context context)
    {
        _context = context;
    }

    public async Task<List<TrBudget>> GetAllAsync(int? idKodepos = null, CancellationToken cancellationToken = default)
    {
        var query = _context.TrBudgets
            .AsNoTracking()
            .Include(x => x.IdKodeposNavigation!)
                .ThenInclude(k => k.IdKelurahanNavigation!)
                    .ThenInclude(kel => kel.IdKecamatanNavigation!)
                        .ThenInclude(kec => kec.IdKotaNavigation)
            .AsQueryable();

        if (idKodepos.HasValue)
        {
            query = query.Where(x => x.IdKodepos == idKodepos.Value);
        }

        return await query.OrderByDescending(x => x.CreatedDate ?? DateTime.MinValue).ToListAsync(cancellationToken);
    }

    public async Task<TrBudget?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.TrBudgets
            .AsNoTracking()
            .Include(x => x.IdKodeposNavigation!)
                .ThenInclude(k => k.IdKelurahanNavigation!)
                    .ThenInclude(kel => kel.IdKecamatanNavigation!)
                        .ThenInclude(kec => kec.IdKotaNavigation)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<TrBudget?> GetByGuidAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        return await _context.TrBudgets
            .AsNoTracking()
            .Include(x => x.IdKodeposNavigation!)
                .ThenInclude(k => k.IdKelurahanNavigation!)
                    .ThenInclude(kel => kel.IdKecamatanNavigation!)
                        .ThenInclude(kec => kec.IdKotaNavigation)
            .FirstOrDefaultAsync(x => x.Guid == guid, cancellationToken);
    }

    public async Task<TrBudget> CreateAsync(BudgetFormViewModel model, string? user = null, CancellationToken cancellationToken = default)
    {
        var openBalance = model.OpenBalance ?? 0;
        var addition = model.Addition ?? 0;
        var deduction = model.Deduction ?? 0;

        if (deduction > (openBalance + addition))
        {
            throw new InvalidOperationException("Pengeluaran (Deduction) tidak boleh melebihi saldo awal ditambah penambahan anggaran.");
        }

        var endingBalance = openBalance + addition - deduction;

        var entity = new TrBudget
        {
            Guid = model.Guid ?? Guid.NewGuid(),
            JudulTransaksi = model.JudulTransaksi.Trim(),
            Description = model.Description?.Trim(),
            OpenBalance = openBalance,
            Addition = addition,
            Deduction = deduction,
            Nominal = endingBalance,
            IdKodepos = model.IdKodepos,
            CreatedBy = user ?? "System",
            CreatedDate = DateTime.UtcNow
        };

        _context.TrBudgets.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<bool> UpdateAsync(BudgetFormViewModel model, string? user = null, CancellationToken cancellationToken = default)
    {
        TrBudget? entity = null;
        if (model.Id.HasValue)
        {
            entity = await _context.TrBudgets.FirstOrDefaultAsync(x => x.Id == model.Id.Value, cancellationToken);
        }
        else if (model.Guid.HasValue)
        {
            entity = await _context.TrBudgets.FirstOrDefaultAsync(x => x.Guid == model.Guid.Value, cancellationToken);
        }

        if (entity == null)
        {
            return false;
        }

        var openBalance = model.OpenBalance ?? entity.OpenBalance ?? 0;
        var addition = model.Addition ?? 0;
        var deduction = model.Deduction ?? 0;

        if (deduction > (openBalance + addition))
        {
            throw new InvalidOperationException("Pengeluaran (Deduction) tidak boleh melebihi total saldo yang tersedia.");
        }

        entity.JudulTransaksi = model.JudulTransaksi.Trim();
        entity.Description = model.Description?.Trim();
        entity.OpenBalance = openBalance;
        entity.Addition = addition;
        entity.Deduction = deduction;
        entity.Nominal = openBalance + addition - deduction;
        entity.IdKodepos = model.IdKodepos;
        entity.UpdatedBy = user ?? "System";
        entity.UpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.TrBudgets.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (entity == null)
        {
            return false;
        }

        _context.TrBudgets.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteByGuidAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        var entity = await _context.TrBudgets.FirstOrDefaultAsync(x => x.Guid == guid, cancellationToken);
        if (entity == null)
        {
            return false;
        }

        _context.TrBudgets.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<decimal> GetTotalBudgetAsync(int? idKodepos = null, CancellationToken cancellationToken = default)
    {
        var query = _context.TrBudgets.AsNoTracking().AsQueryable();
        if (idKodepos.HasValue)
        {
            query = query.Where(x => x.IdKodepos == idKodepos.Value);
        }

        return await query.SumAsync(x => x.Nominal ?? 0, cancellationToken);
    }
}
