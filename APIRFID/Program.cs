using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using APIRFID.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<APIRFIDContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("APIRFIDContext") ?? throw new InvalidOperationException("Connection string 'APIRFIDContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
using (var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
{
    var context = serviceScope.ServiceProvider.GetRequiredService<APIRFIDContext>();
    // context.Database.EnsureDeleted();
    context.Database.EnsureCreated();
}


app.Run();
