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
using Microsoft.AspNetCore.Identity;

namespace WhatsappAPI.Controllers
{
    [ApiController]
    [Route("usuario")]
    public class UsuarioController
    {
        private IConfiguration _configuration;
        private readonly ILog _logger;
        private readonly Microsoft.AspNetCore.Identity.UserManager<IdentityUser> userManager;

        public UsuarioController(IConfiguration configuration, ILog logger, UserManager<IdentityUser> userManager)
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
                //claims,
                //expires: DateTime.Now.AddHours(6),
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

        //METODO PARA REGISTRAR UN NUEVO USUARIO DE LA WEB API

        [HttpPost]
        [Route("registrar")]
        public async Task<dynamic> RegistrarAsync(CredencialesUsuario credencialesUsuario)
        {
            //crea y persiste el nuevo usuario en las tablas de identity
            var usuarioIdentity = new IdentityUser { UserName = credencialesUsuario.UserName };
            var resultado = await userManager.CreateAsync(usuarioIdentity, credencialesUsuario.Password);

            //busca y obtiene el usuario creado para construir su token
            Usuario usuario = Usuario.DB().Where(x => x.usuario == credencialesUsuario.UserName && x.password == credencialesUsuario.Password).FirstOrDefault();

            if (resultado.Succeeded)
            {
                return ConstruirToken(usuario);
            }
            else
            {
                return new
                {
                    success = false,
                    message = "Error al registrar usuario",
                    result = ""
                };
            }
        }

        private JwtSecurityToken ConstruirToken(Usuario usuario)
        {
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

            return token;
        }
    }
}
