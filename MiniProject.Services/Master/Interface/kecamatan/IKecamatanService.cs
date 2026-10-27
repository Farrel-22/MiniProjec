using MiniProject.Common.ViewModel;
using MiniProject.DAL.ScaffoldContainer;

namespace MiniProject.Services.Master.Interface.Kecamatan;

public interface IKecamatanService
{
    Task<List<MKecamatan>> GetAllAsync(int? idKota = null, CancellationToken cancellationToken = default);
    Task<List<MKecamatan>> GetByKotaIdAsync(int idKota, CancellationToken cancellationToken = default);
    Task<MKecamatan?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<MKecamatan> CreateAsync(KecamatanFormViewModel model, string? user = null, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(KecamatanFormViewModel model, string? user = null, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(string nama, int idKota, int? excludeId = null, CancellationToken cancellationToken = default);
    Task<bool> HasChildKelurahanAsync(int idKecamatan, CancellationToken cancellationToken = default);
}
