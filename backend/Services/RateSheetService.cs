
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using UglyToad.PdfPig;
using FreightLook.Api.Models;

namespace FreightLook.Api.Services;

public sealed class RateSheetService(RateRepository repo)
{
    public async Task<UploadResult> ProcessAsync(
        IFormFile file,
        CancellationToken ct)
    {
        if (file.Length == 0)
            throw new InvalidOperationException("File is empty.");

        if (!Path.GetExtension(file.FileName)
            .Equals(".pdf", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                "Only PDF files are supported.");
        }

        if (file.Length > 25 * 1024 * 1024)
            throw new InvalidOperationException(
                "Maximum upload size is 25 MB.");

        var id = await repo.StartBatchAsync(
            Path.GetFileName(file.FileName),
            file.ContentType,
            file.Length,
            ct);

        var temp = Path.Combine(
            Path.GetTempPath(),
            $"{Guid.NewGuid():N}.pdf");

        try
        {
            await using (var fs = File.Create(temp))
            {
                await file.CopyToAsync(fs, ct);
            }

            var lines = new List<string>();

            using (var pdf = PdfDocument.Open(temp))
            {
                foreach (var page in pdf.GetPages())
                {
                    lines.AddRange(
                        page.Text.Split(
                            new[] { "\r\n", "\n", "\r" },
                            StringSplitOptions.RemoveEmptyEntries |
                            StringSplitOptions.TrimEntries));
                }
            }

            // Stage candidate text only. Do not invent or auto-approve rates.
            //var candidates = lines
            //    .Where(x =>
            //        Regex.IsMatch(
            //            x, @"\bDEL\b",
            //            RegexOptions.IgnoreCase) &&
            //        Regex.IsMatch(
            //            x, @"\bINR\b",
            //            RegexOptions.IgnoreCase))
            //    .ToList();

            var candidates = lines
    .Where(x =>
        Regex.IsMatch(
            x,
            @"\b(DEL|INR|USD|EUR|AED|QAR|LON|DXB|DOH|PAR)\b",
            RegexOptions.IgnoreCase))
    .ToList();

            var staged = candidates.Select((raw, i) =>
            {
                var airline = Regex.Match(
                    raw,
                    @"(AIR FRANCE|BRITISH AIRWAYS|EMIRATES|ETIHAD AIRWAYS|FLYDUBAI|QATAR AIRWAYS|LUFTHANSA CARGO|CATHAY CARGO|GULF AIR CARGO|SAUDI ARABIAN AIRLINES|INDIGO AIRLINES|KUWAIT AIRWAYS)",
                    RegexOptions.IgnoreCase)
                    .Groups[1].Value;

                return (
                    RowNumber: i + 1,
                    RawText: raw,
                    Airline: string.IsNullOrWhiteSpace(airline)
                        ? null
                        : airline,
                    Origin: "DEL",
                    Destination: (string?)null,
                    Status: "NeedsReview",
                    ValidationMessage:
    (string?)"Confirm destination, weight slabs and surcharges before approval.");
            }).ToList();

            await repo.SaveCandidatesAsync(id, staged, ct);

            var status = lines.Count == 0
                ? "NeedsOCR"
                : "NeedsReview";

            var message = lines.Count == 0
                ? "No selectable text found. OCR is required. No rates were auto-approved."
                : $"Extracted {lines.Count} text lines and staged " +
                  $"{staged.Count} candidate rows for review. " +
                  "No rates were auto-approved.";

            await repo.FinishBatchAsync(id, status, message, ct);

            return new UploadResult(
                id, file.FileName, status, staged.Count, message);
        }
        catch (Exception ex)
        {
            await repo.FinishBatchAsync(
                id, "Failed", ex.Message, ct);
            throw;
        }
        finally
        {
            if (File.Exists(temp))
                File.Delete(temp);
        }
    }
}
