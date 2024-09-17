using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;


namespace minimal_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : Controller
    {
        [HttpPost("LoginDTO/{Email}/{Senha}")]
        public IActionResult Login(string Email, string Senha)
        {
            if (Email == "adm@teste.com" && Senha == "123456")
            {
                return Ok("Login com sucesso");
            }
            else {
                return Unauthorized();
            }
                
            
        }
    }
}
