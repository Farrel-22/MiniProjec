using MiniProject.Common.ViewModel;
using MiniProject.DAL.ScaffoldContainer;

namespace MiniProject.Services.Transaction.Interface;

public interface IBudgetService
{
    Task<List<TrBudget>> GetAllAsync(int? idKodepos = null, CancellationToken cancellationToken = default);
    Task<TrBudget?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<TrBudget?> GetByGuidAsync(Guid guid, CancellationToken cancellationToken = default);
    Task<TrBudget> CreateAsync(BudgetFormViewModel model, string? user = null, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(BudgetFormViewModel model, string? user = null, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> DeleteByGuidAsync(Guid guid, CancellationToken cancellationToken = default);
    Task<decimal> GetTotalBudgetAsync(int? idKodepos = null, CancellationToken cancellationToken = default);
}
