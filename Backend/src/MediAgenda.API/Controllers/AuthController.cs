using MediAgenda.Application.DTOs;
using MediAgenda.Application.DTOs.API;
using MediAgenda.Application.Interfaces;
using MediAgenda.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.ObjectModelRemoting;
using Swashbuckle.AspNetCore.Annotations;

namespace MediAgenda.API.Controllers
{
    [Route("api/Auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }

        // POST: api/Auth/Login
        [SwaggerOperation(Summary = "Inicia sesion en el sistema.", Description = "Este endpoint permite a los usuarios iniciar sesion.")]
        [SwaggerResponse(200, "Te devuelve el token en una cookie y la informacion del usuario.", typeof(APIJWTResponse))]
        [HttpPost("Login")]
        public async Task<ActionResult<APIJWTResponse>> Login(LoginDTO dto)
        {
            var token = await _service.Login(dto);
            SetTokenCookie(token.Token, token.ExpirationToken);
            return new APIJWTResponse(token.User, token.Roles);
        }

        // POST: api/Auth/Register
        [SwaggerOperation(Summary = "Registra un nuevo usuario en el sistema.", Description = "Este endpoint permite registrar un nuevo usuario.")]
        [SwaggerResponse(200, "Te devuelve el token en una cookie y la informacion del usuario.", typeof(APIJWTResponse))]
        [HttpPost("Register")]
        public async Task<ActionResult<APIJWTResponse>> Register(RegisterDTO dto)
        {
            var token = await _service.Register(dto);
            SetTokenCookie(token.Token, token.ExpirationToken);
            return new APIJWTResponse(token.User,token.Roles);
        }

        // PUT: api/Auth/Logout
        [SwaggerOperation(Summary = "Cierra sesion en el sistema.", Description = "Este endpoint permite a los usuarios cerrar sesion.")]
        [SwaggerResponse(204, "Sesion cerrada.")]
        [SwaggerResponse(401, "No estas registrado.")]
        [HttpPut("Logout")]
        [Authorize]
        public async Task<ActionResult> Logout()
        {
            Response.Cookies.Delete("jwt");
            return NoContent();
        }

        private void SetTokenCookie(string token, DateTime expiration)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,  // Protección XSS
                Secure = true,    // Si permite http o https
                SameSite = SameSiteMode.None, // Para que se mantenga aun luego de cambiar de paginas
                Expires = expiration,
                Path = "/"
            };

            //jwt es el nombre, token el valor y las opciones las opciones
            Response.Cookies.Append("jwt", token, cookieOptions);
        }
    }
}
