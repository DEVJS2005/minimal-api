using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using minimal_api.Dominio.DTOS;
using minimal_api.Dominio.Entidades;
using minimal_api.Dominio.Interfaces;
using minimal_api.Infraestrutura.Db;

namespace minimal_api.Dominio.Servico;

public class veiculoServico : IVeiculoServico
{
    private readonly DbContexto _contexto;
    public veiculoServico(DbContexto contexto){
        _contexto = contexto;
    }

    public void ApagarVeiculo(Veiculo veiculo)
    {
       _contexto.Veiculos.Remove(veiculo);
       _contexto.SaveChanges();
    }

    public void AtualizarVeiculo(Veiculo veiculo)
    {
       _contexto.Veiculos.Update(veiculo);
       _contexto.SaveChanges();
    }

    public Veiculo? BuscaPorId(int id)
    {
        return _contexto.Veiculos.Where(x => x.Id == id).FirstOrDefault();
    }

    public void CadastrarVeiculo(Veiculo veiculo)
    {
        _contexto.Veiculos.Add(veiculo);
        _contexto.SaveChanges();
    }

    public List<Veiculo> Todos(int pagina = 1, string? Nome = null, string? Marca = null)
    {
        var query = _contexto.Veiculos.AsQueryable();
        if(!string.IsNullOrEmpty(Nome)){
            query = query.Where(v => EF.Functions.Like(v.Nome.ToLower(),$"%{Nome.ToLower()}%"));
        
        }
        int itensPorPagina = 10;

        query = query.Skip((pagina -1)* itensPorPagina).Take(itensPorPagina);


        return query.ToList();
    }
}