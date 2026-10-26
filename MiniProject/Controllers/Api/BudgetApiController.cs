using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniProject.Common.Entity.Transaction;
using MiniProject.Common.ViewModel;
using MiniProject.Services.Master;
using MiniProject.Services.Transaction.Service;

namespace MiniProject.Controllers.Api;

[ApiController]
[Route("api/budget")]
[Authorize]
public class BudgetApiController : ControllerBase
{
    private readonly IBudgetService _budgetService;
    private readonly IBranchService _branchService;

    public BudgetApiController(IBudgetService budgetService, IBranchService branchService)
    {
        _budgetService = budgetService;
        _branchService = branchService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var list = await _budgetService.GetAllAsync(cancellationToken);
        return Ok(list.Select(ToDto));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var e = await _budgetService.GetByIdAsync(id, cancellationToken);
        if (e == null)
        {
            return NotFound(new { message = "Budget not found." });
        }

        return Ok(ToDto(e));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BudgetFormViewModel model, CancellationToken cancellationToken)
    {
        await ValidateBranchAsync(model, cancellationToken);

        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        await _budgetService.CreateAsync(model, User.Identity?.Name, cancellationToken);
        return Ok(new { message = "Budget created." });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] BudgetFormViewModel model, CancellationToken cancellationToken)
    {
        model.Uid = id;
        await ValidateBranchAsync(model, cancellationToken);

        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var updated = await _budgetService.UpdateAsync(model, User.Identity?.Name, cancellationToken);
        if (!updated)
        {
            return NotFound(new { message = "Budget not found." });
        }

        return Ok(new { message = "Budget updated." });
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await _budgetService.DeleteAsync(id, User.Identity?.Name, cancellationToken);
        if (deleted)
        {
            return Ok(new { message = "Budget deleted." });
        }

        return NotFound(new { message = "Budget not found." });
    }

    private async Task ValidateBranchAsync(BudgetFormViewModel model, CancellationToken ct)
    {
        if (model.RefBranchId.HasValue)
        {
            var branch = await _branchService.GetByIdAsync(model.RefBranchId.Value, ct);
            if (branch == null || (model.RefRegionId.HasValue && branch.RefRegionId != model.RefRegionId))
            {
                ModelState.AddModelError(nameof(model.RefBranchId), "Selected branch is not valid for the selected region.");
            }
        }
    }

    private static object ToDto(TBudget x) => new
    {
        x.Uid,
        RefRegionId = x.RefBranch?.RefRegionId,
        RegionName = x.RefBranch?.RefRegion?.TxtName,
        x.RefBranchId,
        BranchName = x.RefBranch?.TxtName,
        x.IntYear,
        x.DecAmount,
        x.TxtDescription
    };
}
