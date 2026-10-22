namespace FreightLook.Api.Models;
public sealed record RateSearchRequest(string? Origin, string? Destination, decimal? WeightKg, string? Query);
public sealed record ParsedQuery(string? Origin, string? Destination, decimal? WeightKg, string? ServiceType);
public sealed record RateResult(int Id, string Airline, string Origin, string Destination, string? ServiceType, decimal? MinimumCharge, decimal? NormalRate, decimal? Rate45, decimal? Rate100, decimal? Rate250, decimal? Rate300, decimal? Rate500, decimal? Rate1000, string? Fsc, string? Ssc, string? Xray, string? Misc, decimal? AmsAwb, decimal? AmsHawb, decimal? AmsEawb, decimal? AmsEhawb, string SourceFile);
public sealed record UploadResult(Guid BatchId, string FileName, string Status, int ExtractedCandidates, string Message);
public sealed record StagedRate(int RowNumber, string RawText, string? Airline, string? Origin, string? Destination, string Status, string? ValidationMessage);
