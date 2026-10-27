using Microsoft.AspNetCore.Mvc;
using MiniProject.Common.ViewModel;
using MiniProject.DAL.ScaffoldContainer;
using MiniProject.Services.Master.Interface.Kecamatan;

namespace MiniProject.Controllers.Api;

[ApiController]
[Route("api/kecamatan")]
public class KecamatanApiController : ControllerBase
{
    private readonly IKecamatanService _kecamatanService;

    public KecamatanApiController(IKecamatanService kecamatanService)
    {
        _kecamatanService = kecamatanService;
    }

    private static object ToDto(MKecamatan x) => new
    {
        x.Id,
        x.Guid,
        x.Nama,
        x.IdKota,
        Kota = x.IdKotaNavigation?.Nama,
        x.CreatedBy,
        x.CreatedDate
    };

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? idKota, CancellationToken cancellationToken)
    {
        var list = await _kecamatanService.GetAllAsync(idKota, cancellationToken);
        return Ok(list.Select(ToDto));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var entity = await _kecamatanService.GetByIdAsync(id, cancellationToken);
        if (entity == null)
        {
            return NotFound(new { message = "Kecamatan tidak ditemukan." });
        }

        return Ok(ToDto(entity));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] KecamatanFormViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            var entity = await _kecamatanService.CreateAsync(model, User.Identity?.Name, cancellationToken);
            return Ok(new { message = "Kecamatan berhasil ditambahkan.", data = ToDto(entity) });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] KecamatanFormViewModel model, CancellationToken cancellationToken)
    {
        model.Id = id;
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            if (!await _kecamatanService.UpdateAsync(model, User.Identity?.Name, cancellationToken))
            {
                return NotFound(new { message = "Kecamatan tidak ditemukan." });
            }

            return Ok(new { message = "Kecamatan berhasil diperbarui." });
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
            if (await _kecamatanService.DeleteAsync(id, cancellationToken))
            {
                return Ok(new { message = "Kecamatan berhasil dihapus." });
            }

            return NotFound(new { message = "Kecamatan tidak ditemukan." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
