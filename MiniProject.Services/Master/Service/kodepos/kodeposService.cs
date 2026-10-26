using Microsoft.EntityFrameworkCore;
using MiniProject.Common.ViewModel;
using MiniProject.DAL.ScaffoldContainer;
using MiniProject.Services.Master.Interface.Kodepos;

namespace MiniProject.Services.Master.Service.Kodepos;

public class KodeposService : IKodeposService
{
    private readonly Latihan1Context _context;

    public KodeposService(Latihan1Context context)
    {
        _context = context;
    }

    public async Task<List<MKodepos>> GetAllAsync(int? idKelurahan = null, CancellationToken cancellationToken = default)
    {
        var query = _context.MKodepos
            .AsNoTracking()
            .Include(x => x.IdKelurahanNavigation)
            .AsQueryable();

        if (idKelurahan.HasValue)
        {
            query = query.Where(x => x.IdKelurahan == idKelurahan.Value);
        }

        return await query.OrderBy(x => x.Kodepos).ToListAsync(cancellationToken);
    }

    public async Task<MKodepos?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.MKodepos
            .AsNoTracking()
            .Include(x => x.IdKelurahanNavigation)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<MKodepos> CreateAsync(KodeposFormViewModel model, string? user = null, CancellationToken cancellationToken = default)
    {
        var entity = new MKodepos
        {
            Guid = Guid.NewGuid(),
            Kodepos = model.Kodepos.Trim(),
            IdKelurahan = model.IdKelurahan,
            CreatedBy = user ?? "System",
            CreatedDate = DateTime.UtcNow
        };

        _context.MKodepos.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<bool> UpdateAsync(KodeposFormViewModel model, string? user = null, CancellationToken cancellationToken = default)
    {
        if (!model.Id.HasValue)
        {
            return false;
        }

        var entity = await _context.MKodepos
            .FirstOrDefaultAsync(x => x.Id == model.Id.Value, cancellationToken);

        if (entity == null)
        {
            return false;
        }

        entity.Kodepos = model.Kodepos.Trim();
        entity.IdKelurahan = model.IdKelurahan;
        entity.UpdatedBy = user ?? "System";
        entity.UpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.MKodepos
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (entity == null)
        {
            return false;
        }

        _context.MKodepos.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
