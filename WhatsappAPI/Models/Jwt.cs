using Microsoft.AspNetCore.Identity;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace WhatsappAPI.Models
{
    public class Jwt
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Subject { get; set; }

        public static async Task<dynamic> validarTokenAsync (ClaimsIdentity identity, UserManager<IdentityUser> userManager)
        {
            try
            {
                if (identity.Claims.Count() == 0)
                {
                    return new
                    {
                        success = false,
                        message = "ERROR: Verifica si estas enviando un token valido",
                        result = ""
                    };
                }

                //busca y obtiene el usuario creado para construir su token

                var id = identity.Claims.FirstOrDefault(x => x.Type == "Id").Value;
                var usuario_t = await userManager.FindByIdAsync(id);
                //var usuario_t = userManager.FindByNameAsync(identity.Name);

                Usuario usuario = new Usuario();

                //probar await para no usar result

                usuario.idUsuario = usuario_t.Id;
                usuario.usuario = usuario_t.UserName;
                usuario.rol = "";
                
                //if (usuario_t != null)
                //{
                //    var roles = await userManager.GetRolesAsync(usuario_t); //buscar rol en la base manualmente
                //}                

                //busca usuario desde lista temporal en memoria, mal

                //var id = identity.Claims.FirstOrDefault(x => x.Type == "Id").Value;
                //Usuario usuario = Usuario.DB().FirstOrDefault(x => x.idUsuario == id);

                return new
                {
                    success = true,
                    message = "exito",
                    result = usuario
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    success = false,
                    message = "Catch: " + ex.Message,
                    result = ""
                };
            }
        }       

    }
}
