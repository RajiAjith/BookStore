using BookStoresWebAPI.Controllers;
using BookStoresWebAPI.Handlers;
using BookStoresWebAPI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Use the connection string name that exists in appsettings.json ("BookStoresDB")
builder.Services.AddDbContext<BookStoresDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BookStoresDB")));

//ingnore loop handling for use include and theninclude on get data from database
builder.Services.AddMvc(option => option.EnableEndpointRouting = false)
    .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

//Authentication set up
builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthentication>("BasicAuthentication", null);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

//use authentication for users
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
