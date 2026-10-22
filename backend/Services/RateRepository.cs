using FreightLook.Api.Data;
using FreightLook.Api.Entities;
using FreightLook.Api.Models;
using Microsoft.EntityFrameworkCore;
namespace FreightLook.Api.Services;
public sealed class RateRepository(FreightLookDbContext db)
{
 public async Task<IReadOnlyList<RateResult>> SearchAsync(string? origin,string? destination,decimal? weight,CancellationToken ct) {
  var q=db.Rates.AsNoTracking().Include(r=>r.Airline).Include(r=>r.OriginAirport).Include(r=>r.DestinationAirport).Include(r=>r.RateSheet).Where(r=>r.IsApproved);
  if(!string.IsNullOrWhiteSpace(origin)) q=q.Where(r=>r.OriginAirport.Code==origin.ToUpper());
  if(!string.IsNullOrWhiteSpace(destination)) q=q.Where(r=>r.DestinationAirport.Code==destination.ToUpper());
  var rows=await q.OrderBy(r=>r.Airline.Name).ThenBy(r=>r.ServiceType).ToListAsync(ct);
  return rows.Select(r=> {var slab=weight is null? r.NormalRate:SelectSlab(r,weight.Value); return new RateResult(r.Id,r.Airline.Name,r.OriginAirport.Code,r.DestinationAirport.Code,r.ServiceType,r.MinimumCharge,slab,r.Rate45,r.Rate100,r.Rate250,r.Rate300,r.Rate500,r.Rate1000,r.Fsc,r.Ssc,r.Xray,r.Misc,r.AmsAwb,r.AmsHawb,r.AmsEawb,r.AmsEhawb,r.RateSheet.OriginalFileName);}).ToList();
 }
 private static decimal? SelectSlab(Rate r,decimal kg) => kg switch { <45=>r.NormalRate, <100=>r.Rate45, <250=>r.Rate100, <300=>r.Rate250??r.Rate100, <500=>r.Rate300??r.Rate250, <1000=>r.Rate500??r.Rate300??r.Rate250, _=>r.Rate1000??r.Rate500??r.Rate300 };
 public async Task<Guid> StartBatchAsync(string filename,string contentType,long size,CancellationToken ct){var row=new RateSheet{OriginalFileName=filename,ContentType=contentType,FileSizeBytes=size,Status="Processing"};db.RateSheets.Add(row);await db.SaveChangesAsync(ct);return row.Id;}
 public async Task FinishBatchAsync(Guid id,string status,string? message,CancellationToken ct){var row=await db.RateSheets.FindAsync(new object?[]{id},ct);if(row is null)return;row.Status=status;row.ProcessingMessage=message;await db.SaveChangesAsync(ct);}
 public async Task SaveCandidatesAsync(Guid batchId,IEnumerable<(int row,string raw,string? airline,string? origin,string? destination,string status,string? message)> rows,CancellationToken ct){foreach(var x in rows)db.RateStaging.Add(new RateStaging{RateSheetId=batchId,RowNumber=x.row,RawText=x.raw,AirlineText=x.airline,OriginCode=x.origin,DestinationCode=x.destination,Status=x.status,ValidationMessage=x.message});await db.SaveChangesAsync(ct);}
 public async Task<IReadOnlyList<StagedRate>> GetStagingAsync(Guid batchId,CancellationToken ct)=>await db.RateStaging.AsNoTracking().Where(x=>x.RateSheetId==batchId).OrderBy(x=>x.RowNumber).Select(x=>new StagedRate(x.RowNumber,x.RawText,x.AirlineText,x.OriginCode,x.DestinationCode,x.Status,x.ValidationMessage)).ToListAsync(ct);
}
