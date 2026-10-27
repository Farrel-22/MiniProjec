using MiniProject.Common.ViewModel;
using MiniProject.DAL.ScaffoldContainer;

namespace MiniProject.Services.Master.Interface.Kota;

public interface IKotaService
{
    Task<List<MKota>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<MKota?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<MKota> CreateAsync(KotaFormViewModel model, string? user = null, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(KotaFormViewModel model, string? user = null, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(string nama, int? excludeId = null, CancellationToken cancellationToken = default);
    Task<bool> HasChildKecamatanAsync(int idKota, CancellationToken cancellationToken = default);
}
