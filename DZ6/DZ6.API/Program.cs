using DZ6.Core.Interfaces;
using DZ6.Core.Services;
using DZ6.Storage.Repositories;
using DZ6.Storage;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
//��� ������ ������ �� ��� ���� ������������������, ������ ��� ������� �������������� ����� ����� ��� DbContext
//���������� ��� DZ6Context �� �������� �������(opt), �� ������ ��� ����������� � ������� opt.UseSqlServer,
//��� ����, �� ��� ������� ��������������� ��� ������� SQL Server, �� �������� ������, ��� ��'�������� �� ���� ���� �����
builder.Services.AddDbContext<DZ6Context>(
    opt => opt.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=DZ6DB;Integrated Security=True;"));

builder.Services.AddScoped<IRepository, GenericRepository>(); //���'����� ����������, �� ����� �� ����� ��� IRepository, ������������ GenericRepository
builder.Services.AddScoped<IProductService, ProductService>(); //��� ������
builder.Services.AddScoped<IClientService, ClientService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();