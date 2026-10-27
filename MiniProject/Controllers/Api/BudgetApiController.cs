using Microsoft.AspNetCore.Mvc;
using MiniProject.Common.ViewModel;
using MiniProject.DAL.ScaffoldContainer;
using MiniProject.Services.Transaction.Interface;

namespace MiniProject.Controllers.Api;

[ApiController]
[Route("api/budget")]
public class BudgetApiController : ControllerBase
{
    private readonly IBudgetService _budgetService;

    public BudgetApiController(IBudgetService budgetService)
    {
        _budgetService = budgetService;
    }

    private static object ToDto(TrBudget x) => new
    {
        x.Id,
        x.Guid,
        x.JudulTransaksi,
        x.Description,
        x.OpenBalance,
        x.Addition,
        x.Deduction,
        x.Nominal,
        x.IdKodepos,
        Kodepos = x.IdKodeposNavigation?.Kodepos,
        Kelurahan = x.IdKodeposNavigation?.IdKelurahanNavigation?.Nama,
        Kecamatan = x.IdKodeposNavigation?.IdKelurahanNavigation?.IdKecamatanNavigation?.Nama,
        Kota = x.IdKodeposNavigation?.IdKelurahanNavigation?.IdKecamatanNavigation?.IdKotaNavigation?.Nama,
        x.CreatedBy,
        x.CreatedDate,
        x.UpdatedBy,
        x.UpdatedDate
    };

    // GET: api/budget
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? idKodepos, CancellationToken cancellationToken)
    {
        var list = await _budgetService.GetAllAsync(idKodepos, cancellationToken);
        return Ok(list.Select(ToDto));
    }

    // GET: api/budget/total
    [HttpGet("total")]
    public async Task<IActionResult> GetTotal([FromQuery] int? idKodepos, CancellationToken cancellationToken)
    {
        var total = await _budgetService.GetTotalBudgetAsync(idKodepos, cancellationToken);
        return Ok(new { total });
    }

    // GET: api/budget/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var entity = await _budgetService.GetByIdAsync(id, cancellationToken);
        if (entity == null)
        {
            return NotFound(new { message = "Data transaksi budget tidak ditemukan." });
        }

        return Ok(ToDto(entity));
    }

    // POST: api/budget
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BudgetFormViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            var entity = await _budgetService.CreateAsync(model, User.Identity?.Name, cancellationToken);
            return Ok(new { message = "Transaksi budget berhasil ditambahkan.", data = ToDto(entity) });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // PUT: api/budget/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] BudgetFormViewModel model, CancellationToken cancellationToken)
    {
        model.Id = id;
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            if (!await _budgetService.UpdateAsync(model, User.Identity?.Name, cancellationToken))
            {
                return NotFound(new { message = "Data transaksi budget tidak ditemukan." });
            }

            return Ok(new { message = "Transaksi budget berhasil diperbarui." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // DELETE: api/budget/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            if (await _budgetService.DeleteAsync(id, cancellationToken))
            {
                return Ok(new { message = "Transaksi budget berhasil dihapus." });
            }

            return NotFound(new { message = "Data transaksi budget tidak ditemukan." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
