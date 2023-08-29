using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhatsappAPI.DTOs
{
    //var obj = new
    //{
    //    // Datos requeridos
    //    messaging_product = "whatsapp", 
    //    to = "+54 11 6372-3442",    //
    //    //to = "+54 11 3846-6361",
    //    type = "text",
    //    text = new
    //    {
    //        preview_url = false,
    //        body = "*Vencimiento de BUIS*\nEstimado: Juan Perez\nsu boleta 1234567890,\ngenerada el 27/06/2023\nVence el 27/07/2023\nNúmero de contrato: 12345\nGracias!" //
    //    }
    //};


    //using System;
    //using System.Collections.Generic;

    //namespace Facturador.Common.Dto
    //{
    //    public class Funcion : BaseDto
    //    {
    //        public virtual Guid Id { get; set; }
    //        //descripcion en base
    //        public virtual string Observaciones { get; set; }
    //        public virtual bool IsSelectedProp { get; set; }
    //        public virtual IList<ResponsableInterlocutor> ResponsablesL { get; set; }
    //    }
    //}

    public class WhatsappDataDto
    {
        public string messaging_product { get; set; }
        public string to { get; set; }  // se recibe le numero de celular destinatario
        public string type { get; set; }
        public TextMessage text { get; set; }

        public WhatsappDataDto()
        {
            messaging_product = "whatsapp";
            type = "text";

            text = new TextMessage();
            text.preview_url = false;
            text.body = ""; // se recibe el contenido del mensaje personalizado
        }
    }
}
