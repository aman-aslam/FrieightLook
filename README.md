Freight Look --- Air Cargo Rate Finder
Project Overview
Freight Look is a web application for searching air cargo rates by
origin, destination, and shipment weight. It also allows users to upload
PDF rate sheets and review extracted information.
Technology Stack and Versions
- Backend: ASP.NET Core Web API --- .NET 8
- Programming Language: C#
- Database: Microsoft SQL Server
- ORM: Entity Framework Core 8 --- Code First
- Frontend: HTML5, CSS3, JavaScript
- PDF Text Extraction: PdfPig
- API Documentation: Swagger / OpenAPI
- Optional AI Integration: OpenAI-compatible chat-completions API
Main Features
Rate Search
Users can search for air cargo rates using origin airport, destination
airport, and shipment weight. The application returns approved rates
stored in the database.
PDF Rate Sheet Upload
Users can upload a PDF rate sheet through the website. The backend
validates the file and records the upload details in SQL Server.
PDF Text Extraction
The backend uses PdfPig to extract selectable text from uploaded PDF
files.
Rate Review Staging
The application can store extracted candidate information in a staging
table for review before rates are approved.
Natural-Language Search
The application supports plain-language rate queries through an optional
AI service. A fallback query parser can be used when AI integration is
not configured.
Database Management
Entity Framework Core Code First manages the database models and schema
through migrations.
API Health Check
A health endpoint allows users to check whether the backend API is
running.
Application Structure
- frontend/ --- website interface for rate search and PDF uploads.
- backend/Controllers/ --- API endpoints for rates and uploads.
- backend/Services/ --- rate search, PDF processing, and application
  services.
- backend/Data/ --- EF Core database context and mappings.
- backend/Models/ or backend/Entities/ --- application and
  database models.
- sql/ --- reference SQL scripts, if included in the project.
Database Entities
The database includes these main entities:
- Airline: Stores airline details.
- Airport: Stores airport codes and details.
- RateSheet: Stores uploaded PDF details and processing status.
- RateStaging: Stores candidate rate information for review.
- Rate: Stores rate records used by the search feature.
API Endpoints
  Method                  Endpoint                                               Purpose
  GET                     /api/health                                          Checks API
                                                                                 availability.

  GET                     /api/rates?origin=DEL&destination=DXB&weightKg=250   Searches rates by route
                                                                                 and weight.

  POST                    /api/rates/natural-language                          Accepts a
                                                                                 plain-language rate
                                                                                 query.

  POST                    /api/uploads                                         Uploads a PDF rate
                                                                                 sheet.

  GET                     /api/uploads/{batchId}/staging                       Retrieves candidate
                                                                             rows for review.
Database Approach
The application uses Entity Framework Core Code First. Entity classes
define the data structure, and EF Core migrations create and update the
SQL Server database.

Main database operations include: - Saving uploaded rate-sheet
details. - Saving candidate rows for review. - Retrieving staging rows
for a batch. - Searching approved rate records.
Typical Workflow
1. A user opens the Freight Look website.
2. The user searches for rates or uploads a PDF rate sheet.
3. The API validates the uploaded file and records the upload.
4. PdfPig extracts selectable text from the PDF.
5. The application identifies possible candidate rows for review.
6. Candidate rows can be retrieved through the staging API.
7. Rate search uses approved records stored in SQL Server.
Running the Application
Backend
From the backend project directory:
dotnet restore
dotnet ef database update
dotnet run --no-launch-profile --urls "http://localhost:5080"
If the project has no initial migration yet, create one before updating
the database:
dotnet ef migrations add InitialCreate
dotnet ef database update
Frontend
From the frontend directory:
python -m http.server 5500
Open http://localhost:5500 in a browser. The frontend API URL should
match the backend address.
Version Summary
  Component               Version
  .NET                    8
  ASP.NET Core            8
  Entity Framework Core   8
  C#                      Version supported by the .NET 8 project
  SQL Server              SQL Server 2019+ or compatible edition
  HTML                    HTML5
  CSS                     CSS3
  JavaScript              Vanilla JavaScript
  PdfPig                  Version specified in the project file
  Swagger / OpenAPI       Version specified in the project file