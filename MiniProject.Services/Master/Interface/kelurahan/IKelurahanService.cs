using MiniProject.Common.ViewModel;
using MiniProject.DAL.ScaffoldContainer;

namespace MiniProject.Services.Master.Interface.Kelurahan;

public interface IKelurahanService
{
    Task<List<MKelurahan>> GetAllAsync(int? idKecamatan = null, CancellationToken cancellationToken = default);
    Task<List<MKelurahan>> GetByKecamatanIdAsync(int idKecamatan, CancellationToken cancellationToken = default);
    Task<MKelurahan?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<MKelurahan> CreateAsync(KelurahanFormViewModel model, string? user = null, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(KelurahanFormViewModel model, string? user = null, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(string nama, int idKecamatan, int? excludeId = null, CancellationToken cancellationToken = default);
    Task<bool> HasChildKodeposAsync(int idKelurahan, CancellationToken cancellationToken = default);
}
