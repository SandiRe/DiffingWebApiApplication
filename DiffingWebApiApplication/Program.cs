using DiffingWebApiApplication;
using DiffingWebApiApplication.Database;
using Microsoft.EntityFrameworkCore;
using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configuring Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// EntityFramework
builder.Services.AddDbContext<DiffingDb>(opt => opt.UseInMemoryDatabase("DiffingItemDatabase"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPut("/v1/diff/{id}/left", async (int id, DiffingData data, DiffingDb db) =>
{
    var diffingItem = await db.DiffingItems.FindAsync(id);

    if (diffingItem is null)
        db.Add(new DiffingItem(id, data.Data, null));
    else
        diffingItem.LeftValue = data.Data;
    
    await db.SaveChangesAsync();

    return Results.Created($"{app.Urls.First()}/v1/diff/{id}/left", null);
});

app.MapGet("/v1/diff/{id}/left", async (int id, DiffingDb db) =>
{
    var diffingItem = await db.DiffingItems.FindAsync(id);

    if (diffingItem is null || diffingItem.LeftValue == null)
        return Results.NotFound();

    var jsonSerializerOptionsIgnoreNullObjects = new JsonSerializerOptions
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    return Results.Json(new DiffingData (diffingItem.LeftValue), jsonSerializerOptionsIgnoreNullObjects, MediaTypeNames.Application.Json, StatusCodes.Status200OK);
});

app.MapPut("/v1/diff/{id}/right", async (int id, DiffingData data, DiffingDb db) =>
{
    var diffingItem = await db.DiffingItems.FindAsync(id);

    if (diffingItem is null)
        db.Add(new DiffingItem(id, data.Data, null));
    else
        diffingItem.RightValue = data.Data;

    await db.SaveChangesAsync();

    return Results.Created($"{app.Urls.First()}/v1/diff/{id}/right", null);
});

app.MapGet("/v1/diff/{id}/right", async (int id, DiffingDb db) =>
{
    var diffingItem = await db.DiffingItems.FindAsync(id);

    if (diffingItem is null || diffingItem.RightValue == null)
        return Results.NotFound();

    var jsonSerializerOptionsIgnoreNullObjects = new JsonSerializerOptions
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    return Results.Json(new DiffingData(diffingItem.RightValue), jsonSerializerOptionsIgnoreNullObjects, MediaTypeNames.Application.Json, StatusCodes.Status200OK);
});

app.MapGet("/v1/diff/{id}", async (int id, DiffingDb db) =>
{
    var diffingItem = await db.DiffingItems.FindAsync(id);

    if (diffingItem is null || diffingItem.LeftValue == null || diffingItem.RightValue == null)
        return Results.NotFound();

    var diffingResult = DiffingHelper.CompareBase64CodedBinaryValues(diffingItem.LeftValue, diffingItem.RightValue);

    var jsonSerializerOptionsIgnoreNullObjects = new JsonSerializerOptions
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    return Results.Json(diffingResult, jsonSerializerOptionsIgnoreNullObjects, MediaTypeNames.Application.Json, StatusCodes.Status200OK);

    // Note: Could not use Ok result since it serializes also null objects to Json.
    // return Results.Ok(diffingResult);
});

app.Run();