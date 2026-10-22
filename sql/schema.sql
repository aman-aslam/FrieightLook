/* Freight Look normalized schema. Run in SQL Server after creating the FreightLook database. */
IF DB_ID(N'FreightLook') IS NULL CREATE DATABASE FreightLook;
GO
USE FreightLook;
GO
IF OBJECT_ID('dbo.RateStaging','U') IS NOT NULL DROP TABLE dbo.RateStaging;
IF OBJECT_ID('dbo.Rate','U') IS NOT NULL DROP TABLE dbo.Rate;
IF OBJECT_ID('dbo.RateSheet','U') IS NOT NULL DROP TABLE dbo.RateSheet;
IF OBJECT_ID('dbo.Airport','U') IS NOT NULL DROP TABLE dbo.Airport;
IF OBJECT_ID('dbo.Airline','U') IS NOT NULL DROP TABLE dbo.Airline;
GO
CREATE TABLE dbo.Airline(Id int IDENTITY PRIMARY KEY, Name nvarchar(160) NOT NULL UNIQUE, IataCode varchar(3) NULL);
CREATE TABLE dbo.Airport(Id int IDENTITY PRIMARY KEY, Code char(3) NOT NULL UNIQUE, Name nvarchar(120) NULL);
CREATE TABLE dbo.RateSheet(Id uniqueidentifier NOT NULL PRIMARY KEY, OriginalFileName nvarchar(260) NOT NULL, ContentType nvarchar(100) NULL, FileSizeBytes bigint NOT NULL, Status varchar(30) NOT NULL, ProcessingMessage nvarchar(2000) NULL, UploadedAtUtc datetime2 NOT NULL, CONSTRAINT CK_RateSheet_Status CHECK(Status IN ('Processing','NeedsReview','NeedsOCR','Approved','Failed')));
CREATE TABLE dbo.Rate(Id int IDENTITY PRIMARY KEY, RateSheetId uniqueidentifier NOT NULL, AirlineId int NOT NULL, OriginAirportId int NOT NULL, DestinationAirportId int NOT NULL, ServiceType nvarchar(80) NULL, MinimumCharge decimal(12,3) NULL, NormalRate decimal(12,3) NULL, Rate45 decimal(12,3) NULL, Rate100 decimal(12,3) NULL, Rate250 decimal(12,3) NULL, Rate300 decimal(12,3) NULL, Rate500 decimal(12,3) NULL, Rate1000 decimal(12,3) NULL, Fsc nvarchar(80) NULL, Ssc nvarchar(80) NULL, Xray nvarchar(80) NULL, Misc nvarchar(80) NULL, AmsAwb decimal(12,3) NULL, AmsHawb decimal(12,3) NULL, AmsEawb decimal(12,3) NULL, AmsEhawb decimal(12,3) NULL, IsApproved bit NOT NULL CONSTRAINT DF_Rate_IsApproved DEFAULT(0), CreatedAtUtc datetime2 NOT NULL CONSTRAINT DF_Rate_Created DEFAULT SYSUTCDATETIME(), CONSTRAINT FK_Rate_Sheet FOREIGN KEY(RateSheetId) REFERENCES dbo.RateSheet(Id), CONSTRAINT FK_Rate_Airline FOREIGN KEY(AirlineId) REFERENCES dbo.Airline(Id), CONSTRAINT FK_Rate_Origin FOREIGN KEY(OriginAirportId) REFERENCES dbo.Airport(Id), CONSTRAINT FK_Rate_Destination FOREIGN KEY(DestinationAirportId) REFERENCES dbo.Airport(Id));
CREATE INDEX IX_Rate_Route_Approved ON dbo.Rate(OriginAirportId,DestinationAirportId,IsApproved) INCLUDE(AirlineId,ServiceType,NormalRate,Rate45,Rate100,Rate250,Rate300,Rate500,Rate1000);
CREATE TABLE dbo.RateStaging(Id bigint IDENTITY PRIMARY KEY, RateSheetId uniqueidentifier NOT NULL, RowNumber int NOT NULL, RawText nvarchar(max) NOT NULL, AirlineText nvarchar(160) NULL, OriginCode char(3) NULL, DestinationCode char(3) NULL, Status varchar(30) NOT NULL, ValidationMessage nvarchar(1000) NULL, CreatedAtUtc datetime2 NOT NULL CONSTRAINT DF_Staging_Created DEFAULT SYSUTCDATETIME(), CONSTRAINT FK_Staging_Sheet FOREIGN KEY(RateSheetId) REFERENCES dbo.RateSheet(Id));
GO
/* Intentionally no sample rate rows: values must come from source rate sheets and human review. */
