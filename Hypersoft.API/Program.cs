using AspNetCoreRateLimit;
using FluentValidation;
using Hypersoft.API.Middlewares;
using Hypersoft.Domain.Repositories;
using Hypersoft.Infrastructure.Data;
using Hypersoft.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Rate Limiting
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
builder.Services.AddInMemoryRateLimiting();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Hypersoft API", Version = "v1", Description = "API de gerenciamento de produtos e categorias do ShopSense." });
    c.EnableAnnotations();
});

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

// AutoMapper
builder.Services.AddAutoMapper(typeof(Hypersoft.Application.Mappings.MappingProfile));

var app = builder.Build();

app.UseIpRateLimiting();
app.UseMiddleware<SecurityHeadersMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAll");

app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();