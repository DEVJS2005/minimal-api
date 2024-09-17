using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using minimal_api.Infraestrutura.Db;
using minimal_api.Dominio;
using minimal_api.Dominio.Interfaces;
using Microsoft.AspNetCore.Mvc;
using minimal_api.Dominio.DTOS;
using minimal_api.Dominio.Servico;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAdministradorServico, AdministradorServico>();

builder.Services.AddDbContext<DbContexto>(options =>
{
    options.UseMySql(
        builder.Configuration.GetConnectionString("mysql"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("mysql"))

    ); 
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost("/login",([FromBody] LoginDTO loginDTO, IAdministradorServico administradorServico) => {
    
if(administradorServico.Login(loginDTO) != null){
    return Results.Ok("Login com sucesso");
}else{
    return Results.Unauthorized();
}
});

app.Run();
