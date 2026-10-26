using Microsoft.AspNetCore.Mvc;
using MiniProject.Services.Master;

namespace MiniProject.Controllers.Api;

[ApiController]
[Route("api/sample")]
public class SampleApiController : ControllerBase
{
    private readonly ISampleService _sampleService;

    public SampleApiController(ISampleService sampleService)
    {
        _sampleService = sampleService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken) =>
        Ok(await _sampleService.GetAllAsync(cancellationToken));
}
