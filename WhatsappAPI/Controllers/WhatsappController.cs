using log4net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using WhatsappAPI.DTOs;
using WhatsappAPI.Models;
using System.Security.Claims;

//using Newtonsoft.Json.Converters;
//using Newtonsoft.Json.Serialization;

namespace WhatsappAPI.DTOs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class WhatsappController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private string _externalApiBaseUrl;
        private string _authorization;
        private readonly ILog _logger;
        private readonly UserManager<IdentityUser> _userManager;

        UserManager<IdentityUser> userManager;

        public WhatsappController(IConfiguration configuration, ILog logger, UserManager<IdentityUser> userManager)
        {
            _configuration = configuration;
            _externalApiBaseUrl = _configuration.GetValue<string>("WhatsappApiSettings:ExternalApiBaseUrl");
            _authorization = _configuration.GetValue<string>("WhatsappApiSettings:Authorization");
            _logger = logger;
            _userManager = userManager;
        }


        /// <summary>
        /// este metodo de envio de mensaje es llamado desde la aplicacon web de vivivendas con la finalidad de notificar la generacion de BUIS a los benificiarios
        /// se realiza una solicitud Post a la plataforma Meta para el envio de Whatsapp
        /// </summary>
        /// <returns></returns>
        /// 

        //public async Task<ActionResult<IList<Todo>>> Create(

        [HttpPost]
        [Route("Send")]
        //[Authorize]
        public async Task<IActionResult> SendMessage([FromBody] WhatsappDataDto whatsappDataDto)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            var rToken = Jwt.validarTokenAsync(identity, _userManager);

            if (!rToken.IsCompletedSuccessfully)
            {
                _logger.Info(rToken.Result.message);

                return (IActionResult)rToken;
            }

            Usuario usuario = rToken.Result;

            if (usuario.rol != "administrador")
            {
                string msg = "No tienes permisos para eliminar clientes";

                _logger.Info(msg);

                return StatusCode(400, msg);
            }

            string token = Request.Headers.Where(x => x.Key == "Authorization").FirstOrDefault().Value;
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                using (var httpClient = new HttpClient())
                {
                    using (var request = new HttpRequestMessage(new HttpMethod("POST"), _externalApiBaseUrl))
                    {
                        request.Headers.Add("Authorization", _authorization);

                        string ContentSerialize = JsonConvert.SerializeObject(whatsappDataDto);
                        request.Content = new StringContent(ContentSerialize);
                        request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                        // Realizar la llamada a la API externa
                        HttpResponseMessage response = await httpClient.SendAsync(request);

                        // Verificar el código de estado de la respuesta
                        if (response.IsSuccessStatusCode)
                        {
                            // Si la respuesta de la API externa es exitosa, no necesitas retornar datos específicos
                            // Puedes simplemente devolver un resultado de acción vacío
                            _logger.Info("Mensaje enviado a Meta correctamente");

                            return NoContent();
                        }
                        else
                        {
                            // Si ocurre un error en la respuesta de la API externa, puedes retornar un resultado de error
                            _logger.Error($"Error al enviar el mensaje a Meta: StatusCode: {(int)response.StatusCode} - {response.ReasonPhrase}");
                            
                            return StatusCode((int)response.StatusCode);
                        }

                        //string responseBody = await response.Content.ReadAsStringAsync();  //TLS versions older than v1.2 are not supported

                        //// serialize the object to JSON using JSON.Net
                        //string JSONText = JsonConvert.SerializeObject(null);
                        //return JSONText;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Se produjo un error en SendMessage", ex);

                return StatusCode(500, $"Internal Server Error - {ex.Message}");
            }
        }

        //[Authorize]
        [HttpPost]
        [Route("Status")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Status()
        {
            _logger.Debug("mensaje de DEBUG");
            _logger.Error("mensaje de ERROR");

            return Ok("API autorizada");
        }

        //[HttpPost]
        //[Route("Status2")]
        //[Authorize]
        //public IActionResult Status2()
        //{
        //    return Ok("API autorizada. Status 2");
        //}

        #region EJEMPLOS 

        // En este ejemplo, se utiliza Task<IActionResult> como tipo de retorno, ya que la API externa se invoca 
        // asincrónicamente y no se requiere un tipo de contenido específico para la respuesta del controlador.
        // Si la respuesta de la API externa es exitosa, se retorna NoContent() para indicar que no hay datos 
        // adicionales que devolver. Si la respuesta de la API externa es un error, se utiliza StatusCode() para 
        // retornar un resultado de error con el mismo código de estado que se recibió de la API externa.
        // Recuerda que esto es solo un ejemplo y puedes adaptarlo según tus necesidades y el manejo específico que 
        // desees realizar al invocar a la API externa desde tu controlador.

        //public async Task<IActionResult> InvokeExternalApi()
        //{
        //    // Lógica para invocar a la API externa

        //    // Realizar la llamada a la API externa
        //    var response = await httpClient.GetAsync("https://api.externa.com/resource");

        //    // Verificar el código de estado de la respuesta
        //    if (response.IsSuccessStatusCode)
        //    {
        //        // Si la respuesta de la API externa es exitosa, no necesitas retornar datos específicos
        //        // Puedes simplemente devolver un resultado de acción vacío
        //        return NoContent();
        //    }
        //    else
        //    {
        //        // Si ocurre un error en la respuesta de la API externa, puedes retornar un resultado de error
        //        return StatusCode((int)response.StatusCode);
        //    }
        //}


        #endregion


    }
}
