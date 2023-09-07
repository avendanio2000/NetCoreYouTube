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
using Newtonsoft.Json.Linq;
using System.Security.Principal;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace WhatsappAPI.Controllers
{
    [ApiController]
    [Route("usuario")]
    public class UsuarioController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ILog _logger;
        private readonly UserManager<IdentityUser> _userManager;

        public UsuarioController(IConfiguration configuration, ILog logger, UserManager<IdentityUser> userManager)
        {
            _configuration = configuration;
            _logger = logger;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("login")]
        public async Task<dynamic> IniciarSesion([FromBody] Object optData)
        {
            var data = JsonConvert.DeserializeObject<dynamic>(optData.ToString());

            string user = data.usuario.ToString();
            string password = data.password.ToString();

            //error al buscar por id en lista en memoria
            //Usuario usuario = Usuario.DB().Where(x => x.usuario == user && x.password == password).FirstOrDefault();


            //busca y obtiene el usuario que solicita ingreso para construir y entregar su token adjunto

            var usuario_t = await _userManager.FindByNameAsync(user);
            //var usuario_t = await _userManager.FindByLoginAsync(user, password);

            if (usuario_t != null)
            {
                Usuario usuario = new Usuario();

                usuario.idUsuario = usuario_t.Id;
                usuario.usuario = usuario_t.UserName;

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
            else
            {
                _logger.Info("Error en inicio de sesion, credenciales incorrectas");

                return new
                {
                    success = false,
                    message = "Credenciales incorrectas",
                    result = ""
                };
            }
        }

        //METODO PARA REGISTRAR UN NUEVO USUARIO DE LA WEB API

        [HttpPost]
        [Route("registrar")]
        //[Authorize]
        public async Task<dynamic> RegistrarAsync(CredencialesUsuario credencialesUsuario)
        {
            //crea y persiste el nuevo usuario en las tablas de identity
            var usuarioIdentity = new IdentityUser { UserName = credencialesUsuario.UserName };
            var resultado = await _userManager.CreateAsync(usuarioIdentity, credencialesUsuario.Password);

            if (resultado.Succeeded)
            {
                //Usuario usuario = Usuario.DB().Where(x => x.usuario == credencialesUsuario.UserName && x.password == credencialesUsuario.Password).FirstOrDefault();

                var usuario_t = _userManager.FindByNameAsync(credencialesUsuario.UserName);

                //busca y obtiene el usuario recien creado para construir su token adjunto
                Usuario usuario = new Usuario();

                usuario.idUsuario = usuario_t.Result.Id;
                usuario.usuario = usuario_t.Result.UserName;

                var token = ConstruirToken(usuario);

                _logger.Info("Inicio de sesion correcto");

                return new
                {
                    success = true,
                    message = "exito",
                    //claimsV = claims,
                    result = new JwtSecurityTokenHandler().WriteToken(token)
                };
            }
            else
            {
                return new
                {
                    success = false,
                    message = "Error al registrar usuario " + resultado.ToString(),
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
