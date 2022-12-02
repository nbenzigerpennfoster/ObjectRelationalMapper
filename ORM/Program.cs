using Domain;
using EntityFramework;
using Microsoft.Data.SqlClient;
using ORM.Controllers;
using ORM.Dapper;
using ORM.EntityFramework;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/* 
 * Uncomment for application to use Entity Framework ORM
 */
builder.Services.AddScoped(p => new TinkeringDbContext(builder.Configuration.GetConnectionString("tinkering")));
builder.Services.AddScoped<IPersonRepository, PersonRepositoryEf>();
builder.Services.AddScoped<IPersonAggregateRepository, PersonAggregateRepositoryEf>();
builder.Services.AddScoped<IUnitOfWork>(p => p.GetService<TinkeringDbContext>());

/*
 * Uncomment for application to use Dapper micro-ORM
 */
//builder.Services.AddScoped<IDbConnection>(p => new SqlConnection(builder.Configuration.GetConnectionString("tinkering")));
//builder.Services.AddScoped<IPersonRepository, PersonRepositoryDapper>();
//builder.Services.AddScoped<IPersonAggregateRepository, PersonAggregateRepositoryDapper>();
//builder.Services.AddScoped<IUnitOfWork, UnitOfWorkDapper>();

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

app.Run();
