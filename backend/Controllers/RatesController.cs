using Microsoft.AspNetCore.Mvc;using FreightLook.Api.Models;using FreightLook.Api.Services;
namespace FreightLook.Api.Controllers;
[ApiController][Route("api/rates")]
public sealed class RatesController(RateRepository repo,NaturalLanguageService nl) : ControllerBase
{
 [HttpGet] public async Task<IActionResult> Search([FromQuery]string? origin,[FromQuery]string? destination,[FromQuery]decimal? weightKg,CancellationToken ct){if(origin is not null&&(origin.Length!=3||!origin.All(char.IsLetter)))return BadRequest("Origin must be a 3-letter airport code.");if(destination is not null&&(destination.Length!=3||!destination.All(char.IsLetter)))return BadRequest("Destination must be a 3-letter airport code.");if(weightKg<0)return BadRequest("Weight cannot be negative.");return Ok(await repo.SearchAsync(origin?.ToUpperInvariant(),destination?.ToUpperInvariant(),weightKg,ct));}
 [HttpPost("natural-language")] public async Task<IActionResult> Natural([FromBody]NaturalRequest request,CancellationToken ct){var parsed=await nl.ParseAsync(request.Query,ct);var rates=await repo.SearchAsync(parsed.Origin,parsed.Destination,parsed.WeightKg,ct);return Ok(new{parsed,rates});}
 public sealed record NaturalRequest(string Query);
}
