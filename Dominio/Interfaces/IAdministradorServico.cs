using minimal_api.Dominio.DTOS;
using minimal_api.Dominio.Entidades;

namespace minimal_api.Dominio.Interfaces;
    public interface  IAdministradorServico{
        Administrador? Login(LoginDTO loginDTO);
        Administrador CadastrarAdm(Administrador administrador);
        List<Administrador> Todos (int? pagina);
       Administrador? BuscaPorId(int id);
    }   

