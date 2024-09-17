using minimal_api.Dominio.DTOS;
using minimal_api.Dominio.Entidades;
using minimal_api.Dominio.Servicos;

namespace minimal_api.Dominio.Interfaces;

public interface IVeiculoServico{
    List<Veiculo> Todos(int pagina = 1, string? Nome= null, string? Marca=null);
    Veiculo? BuscaPorId(int id);
    void CadastrarVeiculo(Veiculo veiculo);
    void AtualizarVeiculo(Veiculo veiculo);
    void ApagarVeiculo(Veiculo veiculo);
}