using Microsoft.AspNetCore.Mvc;
using MiniProject.Common.ViewModel;
using MiniProject.DAL.ScaffoldContainer;
using MiniProject.Services.Master.Interface.Kodepos;

namespace MiniProject.Controllers.Api;

[ApiController]
[Route("api/kodepos")]
public class KodeposApiController : ControllerBase
{
    private readonly IKodeposService _kodeposService;

    public KodeposApiController(IKodeposService kodeposService)
    {
        _kodeposService = kodeposService;
    }

    private static object ToDto(MKodepos x) => new
    {
        x.Id,
        x.Guid,
        x.Kodepos,
        x.IdKelurahan,
        Kelurahan = x.IdKelurahanNavigation?.Nama,
        Kecamatan = x.IdKelurahanNavigation?.IdKecamatanNavigation?.Nama,
        Kota = x.IdKelurahanNavigation?.IdKecamatanNavigation?.IdKotaNavigation?.Nama,
        x.CreatedBy,
        x.CreatedDate
    };

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? idKelurahan, CancellationToken cancellationToken)
    {
        var list = await _kodeposService.GetAllAsync(idKelurahan, cancellationToken);
        return Ok(list.Select(ToDto));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var entity = await _kodeposService.GetByIdAsync(id, cancellationToken);
        if (entity == null)
        {
            return NotFound(new { message = "Kode pos tidak ditemukan." });
        }

        return Ok(ToDto(entity));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] KodeposFormViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            var entity = await _kodeposService.CreateAsync(model, User.Identity?.Name, cancellationToken);
            return Ok(new { message = "Kode pos berhasil ditambahkan.", data = ToDto(entity) });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] KodeposFormViewModel model, CancellationToken cancellationToken)
    {
        model.Id = id;
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            if (!await _kodeposService.UpdateAsync(model, User.Identity?.Name, cancellationToken))
            {
                return NotFound(new { message = "Kode pos tidak ditemukan." });
            }

            return Ok(new { message = "Kode pos berhasil diperbarui." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            if (await _kodeposService.DeleteAsync(id, cancellationToken))
            {
                return Ok(new { message = "Kode pos berhasil dihapus." });
            }

            return NotFound(new { message = "Kode pos tidak ditemukan." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
