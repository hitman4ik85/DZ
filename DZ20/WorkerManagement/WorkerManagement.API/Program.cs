using WorkerManagement.Storage.Data;
using WorkerManagement.Core.Interfaces;
using WorkerManagement.Core.Services;
using Microsoft.EntityFrameworkCore;
using WorkerManagement.Storage;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Local");
builder.Services.AddDbContext<WorkerManagementContext>(opt =>
    opt.UseSqlServer(connectionString));

builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IManagerService, ManagerService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
