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
using System.Text.Json;
using minimal_api.Dominio.Enuns;

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

app.MapGet("/", () => Results.Json(new Home())).WithTags("Home");
#endregion

#region Login Administradores
// LoginDTO
app.MapPost("/login",([FromBody] LoginDTO loginDTO, IAdministradorServico administradorServico) => {   
if(administradorServico.Login(loginDTO) != null){
    return Results.Ok("Login com sucesso");
}else{
    return Results.Unauthorized();
}
}).WithTags("Login Administradores");

// Cadastro Administradores
app.MapPost("/Administradores/Cadastro",([FromBody] AdministradorDTO administradorDTO, IAdministradorServico administradorServico) => {   
  var validacao = new ErrosDeValidacao{
    Mensagens = new List<string>()
  };

    if(string.IsNullOrEmpty(administradorDTO.Email)){
        validacao.Mensagens.Add("O campo email não pode ser vazio");
    };

    if(string.IsNullOrEmpty(administradorDTO.Senha)){
        validacao.Mensagens.Add("O campo senha não pode ser vazio");
    };

    if(administradorDTO.Perfil == null){
        validacao.Mensagens.Add("O campo perfil não pode ser vazio");
    };

    if(validacao.Mensagens.Count > 0){
        return Results.BadRequest(validacao);
    }

    var administrador = new Administrador{
        Email = administradorDTO.Email,
        Senha = administradorDTO.Senha,
        Perfil = administradorDTO.Perfil.ToString() ?? Perfil.Editor.ToString(),
    }; 
    administradorServico.CadastrarAdm(administrador);


    return Results.Ok(new AdministradorModelView{
            Id = administrador.Id,
            Email = administrador.Email,
            Perfil = administrador.Perfil
        });
}).WithTags("Login Administradores");

//Listagem de Administradores
app.MapGet($"/Administradores/Listar",([FromQuery] int? pagina,IAdministradorServico administradorServico) =>{
    
    var adms = new List<AdministradorModelView>();
    var Administradores = administradorServico.Todos(pagina);
    foreach(var adm in Administradores){
        adms.Add(new AdministradorModelView{
            Id = adm.Id,
            Email = adm.Email,
            Perfil = adm.Perfil
        });
    }

    return Results.Ok(adms);
}).WithTags("Login Administradores");

// Buscar por ID
app.MapGet("/Administradores/BuscarPorID/{Id}",([FromRoute] int Id, IAdministradorServico administradorServico )=>{
    var administrador = administradorServico.BuscaPorId(Id);

    if(administrador == null) return Results.NotFound();

    return Results.Ok(new AdministradorModelView{
            Id = administrador.Id,
            Email = administrador.Email,
            Perfil = administrador.Perfil
        });
    }).WithTags("Login Administradores");



#endregion

#region Veiculos
ErrosDeValidacao validaDTO(VeiculoDTO veiculoDTO){
var validacao = new ErrosDeValidacao{
    Mensagens = new List<string>()
};

    if(string.IsNullOrEmpty(veiculoDTO.Nome)){
        validacao.Mensagens.Add("O nome não pode ser vazio");
    };
     if(string.IsNullOrEmpty(veiculoDTO.Marca)){
        validacao.Mensagens.Add("A Marca não pode ficar em branco");
    };
     if(veiculoDTO.Ano < 1950){
        validacao.Mensagens.Add("Veiculo muito antigo, aceito somente veiculos acima de 1950.");
    };
    return validacao;
};

// Cadastro de veiculos
app.MapPost("/Veiculos/Cadastrar",([FromBody] VeiculoDTO veiculoDTO, IVeiculoServico veiculoServico) =>{
    
    var validacao = validaDTO(veiculoDTO);
    if(validacao.Mensagens.Count > 0){
        return Results.BadRequest(validacao);
    }
    var veiculo = new Veiculo{
        Nome = veiculoDTO.Nome,
        Marca = veiculoDTO.Marca,
        Ano = veiculoDTO.Ano,
    }; 
    veiculoServico.CadastrarVeiculo(veiculo);


    return Results.Created($"/veiculo/{veiculo.Id}", veiculo);
}).WithTags("Veiculos");

app.MapGet("/Veiculos/Listar",([FromQuery] int? pagina,IVeiculoServico veiculoServico) =>{
    var veiculos = veiculoServico.Todos(pagina);
    return Results.Ok(veiculos);
}).WithTags("Veiculos");

app.MapGet("/Veiculos/{Id}",([FromRoute] int Id, IVeiculoServico veiculoServico )=>{
    var veiculo = veiculoServico.BuscaPorId(Id);

    if(veiculo == null) return Results.NotFound();

    return Results.Ok(veiculo);
    }).WithTags("Veiculos");

app.MapPut("/Veiculos/{Id}",([FromRoute] int Id, VeiculoDTO veiculoDTO, IVeiculoServico veiculoServico )=>{
    
    var validacao = validaDTO(veiculoDTO);
    if(validacao.Mensagens.Count > 0){
        return Results.BadRequest(validacao);
    }

    var veiculo = veiculoServico.BuscaPorId(Id);
    if( veiculo == null ){ return Results.NotFound(); }

    veiculo.Nome = veiculoDTO.Nome;
    veiculo.Marca = veiculoDTO.Marca;
    veiculo.Ano = veiculoDTO.Ano;
    
    veiculoServico.AtualizarVeiculo(veiculo);
    
    return Results.Ok(veiculo);
    }).WithTags("Veiculos");

app.MapDelete("/Veiculos/{Id}",([FromRoute] int Id, IVeiculoServico veiculoServico )=>{
    
    var veiculo = veiculoServico.BuscaPorId(Id);
    if( veiculo == null ){ return Results.NotFound(); }

    veiculoServico.ApagarVeiculo(veiculo);
    
    return Results.NoContent();
    }).WithTags("Veiculos");

#endregion




app.UseSwagger();
app.UseSwaggerUI();
app.Run();