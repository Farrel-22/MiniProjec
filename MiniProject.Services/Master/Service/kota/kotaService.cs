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

    public async Task<MKota> CreateAsync(KotaFormViewModel model, string? user = null, CancellationToken cancellationToken = default)
    {
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

        _context.MKota.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
