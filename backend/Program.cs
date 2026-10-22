using FreightLook.Api.Data;
using Microsoft.EntityFrameworkCore;
using FreightLook.Api.Services;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddDbContext<FreightLookDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Default")));
builder.Services.AddCors(o => o.AddPolicy("frontend", p => p.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));
builder.Services.AddScoped<RateRepository>();
builder.Services.AddScoped<RateSheetService>();
builder.Services.AddHttpClient<NaturalLanguageService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("frontend");
app.UseDefaultFiles();
app.UseStaticFiles();
app.MapControllers();
app.MapGet("/api/health", () => Results.Ok(new { status = "ok", service = "FreightLook API" }));
app.Run();