using Microsoft.EntityFrameworkCore;
using MiniProject.Common.ViewModel;
using MiniProject.DAL.ScaffoldContainer;
using MiniProject.Services.Master.Interface.Kecamatan;

namespace MiniProject.Services.Master.Service.Kecamatan;

public class KecamatanService : IKecamatanService
{
    private readonly Latihan1Context _context;

    public KecamatanService(Latihan1Context context)
    {
        _context = context;
    }

    public async Task<List<MKecamatan>> GetAllAsync(int? idKota = null, CancellationToken cancellationToken = default)
    {
        var query = _context.MKecamatans
            .AsNoTracking()
            .Include(x => x.IdKotaNavigation)
            .AsQueryable();

        if (idKota.HasValue)
        {
            query = query.Where(x => x.IdKota == idKota.Value);
        }

        return await query.OrderBy(x => x.Nama).ToListAsync(cancellationToken);
    }

    public async Task<List<MKecamatan>> GetByKotaIdAsync(int idKota, CancellationToken cancellationToken = default)
    {
        return await GetAllAsync(idKota, cancellationToken);
    }

    public async Task<MKecamatan?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.MKecamatans
            .AsNoTracking()
            .Include(x => x.IdKotaNavigation)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsByNameAsync(string nama, int idKota, int? excludeId = null, CancellationToken cancellationToken = default)
    {
        var normalized = nama.Trim().ToLower();
        var query = _context.MKecamatans
            .AsNoTracking()
            .Where(x => x.IdKota == idKota && x.Nama.ToLower() == normalized);

        if (excludeId.HasValue)
        {
            query = query.Where(x => x.Id != excludeId.Value);
        }

        return await query.AnyAsync(cancellationToken);
    }

    public async Task<bool> HasChildKelurahanAsync(int idKecamatan, CancellationToken cancellationToken = default)
    {
        return await _context.MKelurahans
            .AsNoTracking()
            .AnyAsync(x => x.IdKecamatan == idKecamatan, cancellationToken);
    }

    public async Task<MKecamatan> CreateAsync(KecamatanFormViewModel model, string? user = null, CancellationToken cancellationToken = default)
    {
        if (await ExistsByNameAsync(model.Nama, model.IdKota, null, cancellationToken))
        {
            throw new InvalidOperationException($"Kecamatan dengan nama '{model.Nama.Trim()}' sudah ada di kota yang dipilih.");
        }

        var entity = new MKecamatan
        {
            Guid = Guid.NewGuid(),
            Nama = model.Nama.Trim(),
            IdKota = model.IdKota,
            CreatedBy = user ?? "System",
            CreatedDate = DateTime.UtcNow
        };

        _context.MKecamatans.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<bool> UpdateAsync(KecamatanFormViewModel model, string? user = null, CancellationToken cancellationToken = default)
    {
        if (!model.Id.HasValue)
        {
            return false;
        }

        var entity = await _context.MKecamatans
            .FirstOrDefaultAsync(x => x.Id == model.Id.Value, cancellationToken);

        if (entity == null)
        {
            return false;
        }

        if (await ExistsByNameAsync(model.Nama, model.IdKota, model.Id.Value, cancellationToken))
        {
            throw new InvalidOperationException($"Kecamatan dengan nama '{model.Nama.Trim()}' sudah ada di kota yang dipilih.");
        }

        entity.Nama = model.Nama.Trim();
        entity.IdKota = model.IdKota;
        entity.UpdatedBy = user ?? "System";
        entity.UpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.MKecamatans
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (entity == null)
        {
            return false;
        }

        if (await HasChildKelurahanAsync(id, cancellationToken))
        {
            throw new InvalidOperationException("Kecamatan tidak dapat dihapus karena masih memiliki data Kelurahan di bawahnya.");
        }

        _context.MKecamatans.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
