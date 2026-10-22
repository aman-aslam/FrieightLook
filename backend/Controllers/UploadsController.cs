using Microsoft.AspNetCore.Mvc;using FreightLook.Api.Services;
namespace FreightLook.Api.Controllers;
[ApiController][Route("api/uploads")]
public sealed class UploadsController(RateSheetService service,RateRepository repo):ControllerBase
{
 [HttpPost] [RequestSizeLimit(26_214_400)] public async Task<IActionResult> Upload(IFormFile file,CancellationToken ct){if(file is null)return BadRequest("Attach a PDF using the 'file' field.");try{return Ok(await service.ProcessAsync(file,ct));}catch(InvalidOperationException ex){return BadRequest(new{error=ex.Message});}}
 [HttpGet("{batchId:guid}/staging")] public async Task<IActionResult> Staging(Guid batchId,CancellationToken ct)=>Ok(await repo.GetStagingAsync(batchId,ct));
}
