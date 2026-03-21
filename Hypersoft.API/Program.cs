using FluentValidation;
using Hypersoft.API.Middlewares;
using Hypersoft.Domain.Repositories;
using Hypersoft.Infrastructure.Data;
using Hypersoft.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// MongoDB
var mongoConnection = builder.Configuration.GetConnectionString("MongoDB") ?? "mongodb://localhost:27017";
var mongoDatabase = builder.Configuration["MongoDB:Database"] ?? "HypersoftDB";
builder.Services.AddSingleton(new MongoDbContext(mongoConnection, mongoDatabase));

// Repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

// MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Hypersoft.Application.Commands.CreateProductCommand).Assembly));

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<Hypersoft.Application.Validators.CreateProductValidator>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();