using Microsoft.AspNetCore.Mvc;
using MiniProject.Common.ViewModel;
using MiniProject.DAL.ScaffoldContainer;
using MiniProject.Services.Master.Interface.Kelurahan;

namespace MiniProject.Controllers.Api;

[ApiController]
[Route("api/kelurahan")]
public class KelurahanApiController : ControllerBase
{
    private readonly IKelurahanService _kelurahanService;

    public KelurahanApiController(IKelurahanService kelurahanService)
    {
        _kelurahanService = kelurahanService;
    }

    private static object ToDto(MKelurahan x) => new
    {
        x.Id,
        x.Guid,
        x.Nama,
        x.IdKecamatan,
        Kecamatan = x.IdKecamatanNavigation?.Nama,
        Kota = x.IdKecamatanNavigation?.IdKotaNavigation?.Nama,
        x.CreatedBy,
        x.CreatedDate
    };

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? idKecamatan, CancellationToken cancellationToken)
    {
        var list = await _kelurahanService.GetAllAsync(idKecamatan, cancellationToken);
        return Ok(list.Select(ToDto));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var entity = await _kelurahanService.GetByIdAsync(id, cancellationToken);
        if (entity == null)
        {
            return NotFound(new { message = "Kelurahan tidak ditemukan." });
        }

        return Ok(ToDto(entity));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] KelurahanFormViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            var entity = await _kelurahanService.CreateAsync(model, User.Identity?.Name, cancellationToken);
            return Ok(new { message = "Kelurahan berhasil ditambahkan.", data = ToDto(entity) });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] KelurahanFormViewModel model, CancellationToken cancellationToken)
    {
        model.Id = id;
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            if (!await _kelurahanService.UpdateAsync(model, User.Identity?.Name, cancellationToken))
            {
                return NotFound(new { message = "Kelurahan tidak ditemukan." });
            }

            return Ok(new { message = "Kelurahan berhasil diperbarui." });
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
            if (await _kelurahanService.DeleteAsync(id, cancellationToken))
            {
                return Ok(new { message = "Kelurahan berhasil dihapus." });
            }

            return NotFound(new { message = "Kelurahan tidak ditemukan." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
