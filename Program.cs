using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using minimal_api.Infraestrutura.Db;
using minimal_api.Dominio;
using minimal_api.Dominio.Interfaces;
using Microsoft.AspNetCore.Mvc;
using minimal_api.Dominio.DTOS;
using minimal_api.Dominio.Servicos;
using minimal_api.Dominio.Entidades;
using minimal_api.Dominio.ModelViews;
using minimal_api.Dominio.Servico;

#region builder
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Escopo do Administrador
builder.Services.AddScoped<IAdministradorServico, AdministradorServico>();

// Escopo do Veiculo
builder.Services.AddScoped<IVeiculoServico, veiculoServico>();

builder.Services.AddDbContext<DbContexto>(options =>
{
    options.UseMySql(
        builder.Configuration.GetConnectionString("mysql"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("mysql"))

    ); 
});

var app = builder.Build();
#endregion

#region Home

app.MapGet("/", () => Results.Json(new Home()));
#endregion

#region Login Administradores
app.MapPost("/login",([FromBody] LoginDTO loginDTO, IAdministradorServico administradorServico) => {   
if(administradorServico.Login(loginDTO) != null){
    return Results.Ok("Login com sucesso");
}else{
    return Results.Unauthorized();
}
});
#endregion

#region Veiculos
app.MapPost("/Veiculos",([FromBody] VeiculoDTO veiculoDTO, IVeiculoServico veiculoServico) =>{
    
    var veiculo = new Veiculo(

    );
    veiculoServico.CadastrarVeiculo(veiculo);

    return Results.Created($"/veiculo/{veiculo.Id}", veiculo);
});
#endregion


app.UseSwagger();
app.UseSwaggerUI();
app.Run();
