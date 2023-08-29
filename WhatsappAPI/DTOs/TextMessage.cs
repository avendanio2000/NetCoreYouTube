using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhatsappAPI.DTOs
{
    public class TextMessage
    {
        public bool preview_url { get; set; }
        public string body { get; set; }
    }
}
