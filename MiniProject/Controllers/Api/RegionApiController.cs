using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniProject.Common.Entity.Master;
using MiniProject.Common.ViewModel;
using MiniProject.Services.Master;

namespace MiniProject.Controllers.Api;

[ApiController]
[Route("api/region")]
[Authorize]
public class RegionApiController : ControllerBase
{
    private readonly IRegionService _regionService;

    public RegionApiController(IRegionService regionService)
    {
        _regionService = regionService;
    }

    private static object ToDto(MRegion x) => new { x.Uid, x.TxtCode, x.TxtName, x.DtCreated };

    // GET: api/region
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        return Ok((await _regionService.GetAllAsync(cancellationToken)).Select(ToDto));
    }

    // GET: api/region/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _regionService.GetByIdAsync(id, cancellationToken);
        if (entity == null)
        {
            return NotFound(new { message = "Region not found." });
        }

        return Ok(ToDto(entity));
    }

    // POST: api/region
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RegionFormViewModel model, CancellationToken cancellationToken)
    {
        if (await _regionService.IsCodeExistsAsync(model.TxtCode, null, cancellationToken))
        {
            ModelState.AddModelError(nameof(model.TxtCode), "Code already exists.");
            return ValidationProblem(ModelState);
        }

        await _regionService.CreateAsync(model, User.Identity?.Name, cancellationToken);
        return Ok(new { message = "Region created." });
    }

    // PUT: api/region/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] RegionFormViewModel model, CancellationToken cancellationToken)
    {
        model.Uid = id;

        if (await _regionService.IsCodeExistsAsync(model.TxtCode, model.Uid, cancellationToken))
        {
            ModelState.AddModelError(nameof(model.TxtCode), "Code already exists.");
            return ValidationProblem(ModelState);
        }

        if (!await _regionService.UpdateAsync(model, User.Identity?.Name, cancellationToken))
        {
            return NotFound(new { message = "Region not found." });
        }

        return Ok(new { message = "Region updated." });
    }

    // DELETE: api/region/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        if (await _regionService.HasActiveBranchesAsync(id, cancellationToken))
        {
            return Conflict(new { message = "Region cannot be deleted because it still has active branches." });
        }

        if (await _regionService.DeleteAsync(id, User.Identity?.Name, cancellationToken))
        {
            return Ok(new { message = "Region deleted." });
        }

        return NotFound(new { message = "Region not found." });
    }
}
