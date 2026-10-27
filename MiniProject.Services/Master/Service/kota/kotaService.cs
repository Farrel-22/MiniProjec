using Microsoft.EntityFrameworkCore;
using MiniProject.Common.ViewModel;
using MiniProject.DAL.ScaffoldContainer;
using MiniProject.Services.Master.Interface.Kota;

namespace MiniProject.Services.Master.Service.Kota;

public class KotaService : IKotaService
{
    private readonly Latihan1Context _context;

    public KotaService(Latihan1Context context)
    {
        _context = context;
    }

    public async Task<List<MKota>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.MKota
            .AsNoTracking()
            .OrderBy(x => x.Nama)
            .ToListAsync(cancellationToken);
    }

    public async Task<MKota?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.MKota
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsByNameAsync(string nama, int? excludeId = null, CancellationToken cancellationToken = default)
    {
        var normalized = nama.Trim().ToLower();
        var query = _context.MKota.AsNoTracking().Where(x => x.Nama.ToLower() == normalized);
        if (excludeId.HasValue)
        {
            query = query.Where(x => x.Id != excludeId.Value);
        }
        return await query.AnyAsync(cancellationToken);
    }

    public async Task<bool> HasChildKecamatanAsync(int idKota, CancellationToken cancellationToken = default)
    {
        return await _context.MKecamatans
            .AsNoTracking()
            .AnyAsync(x => x.IdKota == idKota, cancellationToken);
    }

    public async Task<MKota> CreateAsync(KotaFormViewModel model, string? user = null, CancellationToken cancellationToken = default)
    {
        if (await ExistsByNameAsync(model.Nama, null, cancellationToken))
        {
            throw new InvalidOperationException($"Kota dengan nama '{model.Nama.Trim()}' sudah ada.");
        }

        var entity = new MKota
        {
            Guid = Guid.NewGuid(),
            Nama = model.Nama.Trim(),
            CreatedBy = user ?? "System",
            CreatedDate = DateTime.UtcNow
        };

        _context.MKota.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<bool> UpdateAsync(KotaFormViewModel model, string? user = null, CancellationToken cancellationToken = default)
    {
        if (!model.Id.HasValue)
        {
            return false;
        }

        var entity = await _context.MKota
            .FirstOrDefaultAsync(x => x.Id == model.Id.Value, cancellationToken);

        if (entity == null)
        {
            return false;
        }

        if (await ExistsByNameAsync(model.Nama, model.Id.Value, cancellationToken))
        {
            throw new InvalidOperationException($"Kota dengan nama '{model.Nama.Trim()}' sudah ada.");
        }

        entity.Nama = model.Nama.Trim();
        entity.UpdatedBy = user ?? "System";
        entity.UpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.MKota
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (entity == null)
        {
            return false;
        }

        if (await HasChildKecamatanAsync(id, cancellationToken))
        {
            throw new InvalidOperationException("Kota tidak dapat dihapus karena masih memiliki data Kecamatan di bawahnya.");
        }

        _context.MKota.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
