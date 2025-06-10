using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CajeroAutomaticoAPI.Models
{
    public class Transaccion
    {
        public int Fiidtrans { get; set; }
        public string Fczona { get; set; }
        public System.DateTime Fdfechaopera { get; set; }
        public int Fiidoperacion { get; set; }
        public string Fcusuarioalta { get; set; }
        public System.DateTime Fdfechaalta { get; set; }
        public TipoOperacion TipoOperacion { get; set; }
    }
}