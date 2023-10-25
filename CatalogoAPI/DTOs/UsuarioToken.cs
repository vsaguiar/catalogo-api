namespace CatalogoAPI.DTOs;

public class UsuarioToken
{
    public bool Authenticated { get; set; } // se o usuário está autenticado
    public DateTime Expiration { get; set; } // a data de expiração do token
    public string Token { get; set; } 
    public string Message { get; set; }
}
