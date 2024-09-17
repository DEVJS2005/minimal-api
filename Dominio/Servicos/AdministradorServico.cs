using minimal_api.Dominio.DTOS;
using minimal_api.Dominio.Entidades;
using minimal_api.Dominio.Interfaces;
using minimal_api.Infraestrutura.Db;

namespace minimal_api.Dominio.Servico;
    public class AdministradorServico : IAdministradorServico{
        private readonly DbContexto _contexto;
        public AdministradorServico(DbContexto contexto){
            _contexto = contexto;
        }

        public Administrador? Login(LoginDTO loginDTO){
            var adm = _contexto.Administradores.Where(a => a.Email == loginDTO.Email && a.Senha == loginDTO.Senha).FirstOrDefault();
           return adm;
  
        }
    }


