using Application.Features.Queries.GetAllBooks;
using Application.Mapping;
using Application.Validators;
using Domain.Interfaces;
using FluentValidation;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add FluentValidation and Validators
builder.Services.AddControllers();
//builder.Services
//    .AddFluentValidationAutoValidation()
//    .AddFluentValidationClientsideAdapters();

//builder.Services.AddValidatorsFromAssemblyContaining<AddBookCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<AddBookCommandValidator>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AppConnectionString")));
//Add CORS
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyHeader()
        .AllowAnyMethod()
        .WithOrigins("http://localhost:3000");
    });
});
builder.Services.AddAutoMapper(typeof(MappingProfiles));
builder.Services.AddMediatR(options =>
{
    options.RegisterServicesFromAssemblies(typeof(GetAllBooksQueryHandler).Assembly);
});
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

var app = builder.Build();

// Apply migrations at startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy"); // enable CORS => This should come before UseHttpsRedirection

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
