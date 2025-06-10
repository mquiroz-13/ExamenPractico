using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CajeroAutomaticoAPI.Models
{
    public class TipoOperacion
    {
        public int Fiidoperacion { get; set; }
        public string Fcdescoper { get; set; }
        public string Fcusuarioalta { get; set; }
        public System.DateTime Fdfechaalta { get; set; }
    }
}