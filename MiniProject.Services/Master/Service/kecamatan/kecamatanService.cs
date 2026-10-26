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

    public async Task<MKecamatan?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.MKecamatans
            .AsNoTracking()
            .Include(x => x.IdKotaNavigation)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<MKecamatan> CreateAsync(KecamatanFormViewModel model, string? user = null, CancellationToken cancellationToken = default)
    {
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

        _context.MKecamatans.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
