# Freight Look — Air Cargo Rate Finder

Freight Look is a prototype web application for searching air cargo rates by origin, destination, and shipment weight. It also supports PDF rate-sheet uploads, extraction of selectable PDF text, and a review workflow for candidate rate information.

## Highlights

- Search approved air cargo rates by origin airport, destination airport, and shipment weight.
- Upload PDF rate sheets and retain upload and batch information.
- Extract selectable PDF text using PdfPig.
- Stage extracted candidate information for human review.
- Support natural-language shipment queries through an optional OpenAI-compatible chat-completions endpoint, with a fallback parser when AI is not configured.
- Manage database schema through Entity Framework Core Code First migrations.
- Expose an API health endpoint and Swagger/OpenAPI documentation.

## Important extraction limitation

The sample rate sheets may contain scanned pages or images rather than selectable PDF text. This prototype extracts **selectable text only**; it does not claim OCR support. Image-only or scanned PDFs may therefore be marked as requiring OCR or produce no useful candidate rows.

Extracted candidate information is staged for review. Uploading a file does **not** automatically approve or invent rates. A person must review and approve rate data before it is used in rate searches.

## Technology

| Component | Technology |
|---|---|
| Backend | ASP.NET Core Web API, .NET 8, C# |
| Database | Microsoft SQL Server |
| ORM | Entity Framework Core 8, Code First |
| Frontend | HTML5, CSS3, vanilla JavaScript |
| PDF text extraction | PdfPig |
| API documentation | Swagger / OpenAPI |
| Optional AI | OpenAI-compatible chat-completions API |

## Architecture

Browser UI → ASP.NET Core Web API → Rate search service → SQL Server

PDF upload service → PdfPig text extraction → Review staging → Human review and approval → SQL Server

The optional AI-compatible endpoint supports natural-language shipment queries.

## Project structure

- frontend/ — browser interface for rate search and PDF uploads.
- backend/Controllers/ — HTTP API endpoints.
- backend/Services/ — rate search, PDF processing, and application services.
- backend/Data/ — EF Core database context and mappings.
- backend/Entities/ or backend/Models/ — application and persistence models.
- sql/ — reference SQL scripts, where included.

## Prerequisites

- .NET 8 SDK
- SQL Server 2019 or a compatible SQL Server edition
- Python 3 for the simple static frontend server
- Optional: an OpenAI-compatible chat-completions endpoint and its configuration

## Run locally

### 1. Configure the database

Set the SQL Server connection string using the backend configuration or the environment variable named ConnectionStrings__Default. Use a local development database and keep credentials out of source control.

### 2. Restore, migrate, and run the API

From the backend project directory:

    dotnet restore
    dotnet ef database update
    dotnet run --no-launch-profile --urls "http://localhost:5080"

If the project does not yet contain an initial migration, create one before applying it:

    dotnet ef migrations add InitialCreate
    dotnet ef database update

Open the Swagger UI at the address exposed by the running API if Swagger is enabled for that environment.

### 3. Run the frontend

From the frontend directory:

    python -m http.server 5500

Open http://localhost:5500 and ensure the frontend API base URL matches the backend address.

## API overview

| Method | Endpoint | Purpose |
|---|---|---|
| GET | /api/health | Check API availability |
| GET | /api/rates?origin=DEL&destination=DXB&weightKg=250 | Search approved rates |
| POST | /api/rates/natural-language | Submit a plain-language shipment query |
| POST | /api/uploads | Upload a PDF rate sheet |
| GET | /api/uploads/{batchId}/staging | Retrieve staged candidate rows |

Confirm request and response details in Swagger for the version you are running.

## Typical workflow

1. Search for an approved rate or upload a PDF rate sheet.
2. The API validates and records the upload.
3. PdfPig extracts selectable text from the PDF.
4. Candidate information is staged for review where extraction supports it.
5. A reviewer checks and approves valid rate data.
6. Search results use approved records only.

## Responsible use and security

- Never commit database passwords, API keys, or production connection strings.
- Treat uploaded files and extracted text as untrusted input.
- Validate and review rate candidates before approval.
- See SECURITY.md for responsible vulnerability disclosure.

## Current scope

This is a prototype and should be evaluated with representative airline rate sheets before production use. OCR, automatic interpretation of image-only PDFs, and automatic approval of extracted rates are not claimed features.
