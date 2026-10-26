using MiniProject.Common.ViewModel;
using MiniProject.DAL.ScaffoldContainer;

namespace MiniProject.Services.Master.Interface.Kodepos;

public interface IKodeposService
{
    Task<List<MKodepos>> GetAllAsync(int? idKelurahan = null, CancellationToken cancellationToken = default);
    Task<MKodepos?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<MKodepos> CreateAsync(KodeposFormViewModel model, string? user = null, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(KodeposFormViewModel model, string? user = null, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
