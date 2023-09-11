using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WhatsappAPI.Models
{
    public class CredencialesUsuario
    {
        [Required]
        public String UserName { get; set; }
        [Required]
        public String Password { get; set; }

        public String NewPassword { get; set; }
    }
}