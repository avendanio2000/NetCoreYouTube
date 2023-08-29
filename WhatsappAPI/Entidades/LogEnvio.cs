namespace WhatsappAPI.Entidades
{
    public class LogEnvio
    {
        public int Id { get; set; }                

        public int IdUsuarioAPI { get; set; }     //especifica la aplicacion que invoco a la WEB API

        public DateTime FechaEnvio { get; set; }

        public string NroCelular { get; set; }

        public string? NombreDestinatario { get; set; }

        public string Mensaje { get; set; }

        public int CodigoRespueta { get; set; } //codigo de respuesta html de la operacion de envio a la api de Meta (Whatsapp app)

        public string DescripcionRespuesta { get; set; }
    }
}
