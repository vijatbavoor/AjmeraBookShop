using AjmeraBookShopAPI.DataRepository;
using AjmeraBookShopAPI.DataRepository.Interface;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

#region Service Cnfiguration
builder.Services.AddCors(options =>
{
    options.AddPolicy("CrosPlatformPolicy", builder =>
        builder.AllowAnyOrigin()
       .AllowAnyMethod()
       .AllowAnyHeader());
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
//Adding database context tothe DI container with Scoped mode
builder.Services.AddDbContext<ApplicationDBContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection"))
);

// Adding Unit of work tothe DI container with Scoped mode
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//Add Swagger documentation
builder.Services.AddSwaggerGen(sd =>
{
    sd.SwaggerDoc("v1", new OpenApiInfo { Title = "AjmeraBookShopBooks", Version = "v1" });
});

#endregion Service Cnfiguration

#region App Configuration
var app = builder.Build();
// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CrosPlatformPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
#endregion