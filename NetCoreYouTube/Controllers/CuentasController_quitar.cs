using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WhatsappAPI.DTOs;
using WhatsappAPI.Models;

namespace WhatsappAPI.Controllers
{
    //[ApiController]
    //[Route("api/[controller]")]
    //public class CuentasController : ControllerBase
    //{
    //    //private IAuthService authService;
    //    //private HelperToken helper;
    //    private readonly UserManager<IdentityUser> userManager;

    //    public CuentasController(IConfiguration configuration, UserManager<IdentityUser> userManager)
    //    {
    //        //this.helper = new HelperToken(configuration);
    //        this.userManager = userManager;
    //        //this.authService = authService;
    //    }

    //    //METODO PARA REGISTRAR UN NUEVO USUARIO DE LA WEB API

    //    [HttpPost("regitrar")]
    //    public async Task<ActionResult<RespuestaAutenticacionDto>> Registrar(CredencialesUsuario credencialesUsuario)
    //    {
    //        var usuario = new IdentityUser { UserName = credencialesUsuario.UserName };

    //        var resultado = await userManager.CreateAsync(usuario, credencialesUsuario.Password);

    //        if (resultado.Succeeded)
    //        {
    //            return ConstruirToken(credencialesUsuario);
    //        }
    //        else
    //        {
    //            return BadRequest(resultado.Errors);
    //        }
    //    }

    //    //RECIBIMOS CON LA CLASE CREDENCIALES USUARIO EL USUARIO Y EL PASSWORD Y DEVOLVEMOS EL TOKEN O UNATHORIZED DEPENDIENDO DE SI LAS CREDENCIALES SON O NO CORRECTAS
    //    [HttpPost]
    //    [Route("Login")]
    //    public IActionResult Login(CredencialesUsuario credencialesUsuario)
    //    {

    //        string userName = "Juan";
    //        string password = "abc123";


    //        if (credencialesUsuario != null &&
    //            credencialesUsuario.UserName == userName && credencialesUsuario.Password == password)
    //        {
    //            var t = ConstruirToken(credencialesUsuario);

    //            return Ok
    //            (
    //                ConstruirToken(credencialesUsuario)

    //            );
    //        }
    //        else
    //        {
    //            return Unauthorized();
    //        }
    //    }

    //    private RespuestaAutenticacionDto ConstruirToken(CredencialesUsuario credencialesUsuario)
    //    {
    //        //var claims = new Claim[]
    //        //{
    //        //    new Claim("name", credencialesUsuario.UserName)
    //        //};

    //        var claims = new Claim[]
    //        {
    //                new Claim(ClaimTypes.Role.ToString(), "admin")
    //        };

    //        var expiracion = DateTime.UtcNow.AddHours(6);

    //        JwtSecurityToken token = new JwtSecurityToken
    //        (
    //            issuer: helper.Issuer,
    //            audience: helper.Audience,
    //            //claims: claims,
    //            //expires: expiracion,
    //            //notBefore: DateTime.UtcNow,
    //            signingCredentials: new SigningCredentials(this.helper.GetKeyToken(), SecurityAlgorithms.HmacSha256)
    //        );

    //        return new RespuestaAutenticacionDto()
    //        {
    //            Token = new JwtSecurityTokenHandler().WriteToken(token)
    //        };
    //    }
    //}
}