using HotelListingSql.Configurations;
using HotelListingSql.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<HotelListingDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("HotelListingDbConnectionString");
    //var connectionString = builder.Configuration.GetValue<string>("ConnectionString:HotelListingDbConnectionString");
    options.UseNpgsql(connectionString);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyCorsPolicy", p => p.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
});

// context ... things you can access through the builder ?!
builder.Host.UseSerilog((context, configuration) => configuration.WriteTo.Console().ReadFrom.Configuration(context.Configuration));

builder.Services.AddAutoMapper(typeof(AutoMapperConfig));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("MyCorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
