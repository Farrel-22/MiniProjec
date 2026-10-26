using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniProject.Common.Entity.Master;
using MiniProject.Common.ViewModel;
using MiniProject.Services.Master;

namespace MiniProject.Controllers.Api;

[ApiController]
[Route("api/branch")]
[Authorize]
public class BranchApiController : ControllerBase
{
    private readonly IBranchService _branchService;
    private readonly IRegionService _regionService;

    public BranchApiController(IBranchService branchService, IRegionService regionService)
    {
        _branchService = branchService;
        _regionService = regionService;
    }

    private static object ToDto(MBranch x) => new { x.Uid, x.RefRegionId, RegionName = x.RefRegion?.TxtName, x.TxtCode, x.TxtName, x.TxtAddress };

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var list = await _branchService.GetAllAsync(cancellationToken);
        return Ok(list.Select(ToDto));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _branchService.GetByIdAsync(id, cancellationToken);
        if (entity == null)
        {
            return NotFound(new { message = "Branch not found." });
        }

        return Ok(ToDto(entity));
    }

    [HttpGet("by-region/{regionUid:guid}")]
    public async Task<IActionResult> GetByRegion(Guid regionUid, CancellationToken cancellationToken)
    {
        return Ok((await _branchService.GetByRegionAsync(regionUid, cancellationToken)).Select(ToDto));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BranchFormViewModel model, CancellationToken cancellationToken)
    {
        if (await _branchService.IsCodeExistsAsync(model.TxtCode, null, cancellationToken))
        {
            ModelState.AddModelError(nameof(model.TxtCode), "Code already exists.");
        }

        if (model.RefRegionId.HasValue && await _regionService.GetByIdAsync(model.RefRegionId.Value, cancellationToken) == null)
        {
            ModelState.AddModelError(nameof(model.RefRegionId), "Selected region is not valid.");
        }

        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        await _branchService.CreateAsync(model, User.Identity?.Name, cancellationToken);
        return Ok(new { message = "Branch created." });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] BranchFormViewModel model, CancellationToken cancellationToken)
    {
        model.Uid = id;

        if (await _branchService.IsCodeExistsAsync(model.TxtCode, model.Uid, cancellationToken))
        {
            ModelState.AddModelError(nameof(model.TxtCode), "Code already exists.");
        }

        if (model.RefRegionId.HasValue && await _regionService.GetByIdAsync(model.RefRegionId.Value, cancellationToken) == null)
        {
            ModelState.AddModelError(nameof(model.RefRegionId), "Selected region is not valid.");
        }

        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var updated = await _branchService.UpdateAsync(model, User.Identity?.Name, cancellationToken);
        if (!updated)
        {
            return NotFound(new { message = "Branch not found." });
        }

        return Ok(new { message = "Branch updated." });
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        if (await _branchService.HasActiveBudgetsAsync(id, cancellationToken))
        {
            return Conflict(new { message = "Branch cannot be deleted because it still has active budgets." });
        }

        if (await _branchService.DeleteAsync(id, User.Identity?.Name, cancellationToken))
        {
            return Ok(new { message = "Branch deleted." });
        }

        return NotFound(new { message = "Branch not found." });
    }
}
