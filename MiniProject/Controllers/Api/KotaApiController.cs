using Microsoft.AspNetCore.Mvc;
using MiniProject.Common.ViewModel;
using MiniProject.DAL.ScaffoldContainer;
using MiniProject.Services.Master.Interface.Kota;

namespace MiniProject.Controllers.Api;

[ApiController]
[Route("api/kota")]
public class KotaApiController : ControllerBase
{
    private readonly IKotaService _kotaService;

    public KotaApiController(IKotaService kotaService)
    {
        _kotaService = kotaService;
    }

    private static object ToDto(MKota x) => new
    {
        x.Id,
        x.Guid,
        x.Nama,
        x.CreatedBy,
        x.CreatedDate
    };

    // GET: api/kota
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var list = await _kotaService.GetAllAsync(cancellationToken);
        return Ok(list.Select(ToDto));
    }

    // GET: api/kota/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var entity = await _kotaService.GetByIdAsync(id, cancellationToken);
        if (entity == null)
        {
            return NotFound(new { message = "Kota tidak ditemukan." });
        }

        return Ok(ToDto(entity));
    }

    // POST: api/kota
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] KotaFormViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var entity = await _kotaService.CreateAsync(model, User.Identity?.Name, cancellationToken);
        return Ok(new { message = "Kota berhasil ditambahkan.", data = ToDto(entity) });
    }

    // PUT: api/kota/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] KotaFormViewModel model, CancellationToken cancellationToken)
    {
        model.Id = id;

        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        if (!await _kotaService.UpdateAsync(model, User.Identity?.Name, cancellationToken))
        {
            return NotFound(new { message = "Kota tidak ditemukan." });
        }

        return Ok(new { message = "Kota berhasil diperbarui." });
    }

    // DELETE: api/kota/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        if (await _kotaService.DeleteAsync(id, cancellationToken))
        {
            return Ok(new { message = "Kota berhasil dihapus." });
        }

        return NotFound(new { message = "Kota tidak ditemukan." });
    }
}
