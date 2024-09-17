using minimal_api.Dominio.DTOS;
using minimal_api.Dominio.Entidades;

namespace minimal_api.Dominio.Interfaces;
    public interface  IAdministradorServico{
        Administrador? Login(LoginDTO loginDTO);
    }

