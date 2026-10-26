using Microsoft.EntityFrameworkCore;
using MiniProject.Common.ViewModel;
using MiniProject.DAL.ScaffoldContainer;
using MiniProject.Services.Master.Interface.Kelurahan;

namespace MiniProject.Services.Master.Service.Kelurahan;

public class KelurahanService : IKelurahanService
{
    private readonly Latihan1Context _context;

    public KelurahanService(Latihan1Context context)
    {
        _context = context;
    }

    public async Task<List<MKelurahan>> GetAllAsync(int? idKecamatan = null, CancellationToken cancellationToken = default)
    {
        var query = _context.MKelurahans
            .AsNoTracking()
            .Include(x => x.IdKecamatanNavigation)
            .AsQueryable();

        if (idKecamatan.HasValue)
        {
            query = query.Where(x => x.IdKecamatan == idKecamatan.Value);
        }

        return await query.OrderBy(x => x.Nama).ToListAsync(cancellationToken);
    }

    public async Task<MKelurahan?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.MKelurahans
            .AsNoTracking()
            .Include(x => x.IdKecamatanNavigation)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<MKelurahan> CreateAsync(KelurahanFormViewModel model, string? user = null, CancellationToken cancellationToken = default)
    {
        var entity = new MKelurahan
        {
            Guid = Guid.NewGuid(),
            Nama = model.Nama.Trim(),
            IdKecamatan = model.IdKecamatan,
            CreatedBy = user ?? "System",
            CreatedDate = DateTime.UtcNow
        };

        _context.MKelurahans.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<bool> UpdateAsync(KelurahanFormViewModel model, string? user = null, CancellationToken cancellationToken = default)
    {
        if (!model.Id.HasValue)
        {
            return false;
        }

        var entity = await _context.MKelurahans
            .FirstOrDefaultAsync(x => x.Id == model.Id.Value, cancellationToken);

        if (entity == null)
        {
            return false;
        }

        entity.Nama = model.Nama.Trim();
        entity.IdKecamatan = model.IdKecamatan;
        entity.UpdatedBy = user ?? "System";
        entity.UpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.MKelurahans
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (entity == null)
        {
            return false;
        }

        _context.MKelurahans.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
