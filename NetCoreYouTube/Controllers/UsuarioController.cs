using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WhatsappAPI.Models;
using Microsoft.AspNetCore.Mvc;
using WhatsappAPI.Models;
using Newtonsoft.Json;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WhatsappAPI.Controllers
{
    [ApiController]
    [Route("usuario")]
    public class UsuarioController
    {
        private IConfiguration _configuration;
        private readonly ILog _logger;

        public UsuarioController(IConfiguration configuration, ILog logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost]
        [Route("login")]
        public dynamic IniciarSesion([FromBody] Object optData)
        {
            var data = JsonConvert.DeserializeObject<dynamic>(optData.ToString());

            string user = data.usuario.ToString();
            string password = data.password.ToString();

            Usuario usuario = Usuario.DB().Where(x => x.usuario == user && x.password == password).FirstOrDefault();

            if (usuario == null)
            {
                _logger.Info("Error en inicio de sesion, credenciales incorrectas");

                return new
                {
                    success = false,
                    message = "Credenciales incorrectas",
                    result = ""
                };
            }

            var jwt = _configuration.GetSection("Jwt").Get<Jwt>();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString()),
                new Claim("Id", usuario.idUsuario),
                new Claim("usuario", usuario.usuario)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                jwt.Issuer,
                jwt.Audience,
                claims,
                expires: DateTime.Now.AddHours(6),
                signingCredentials: signIn
                );

            _logger.Info("Inicio de sesion correcto");

            return new
            {
                success = true,
                message = "exito",
                //claimsV = claims,
                result = new JwtSecurityTokenHandler().WriteToken(token)
            };
        }
    }
}
