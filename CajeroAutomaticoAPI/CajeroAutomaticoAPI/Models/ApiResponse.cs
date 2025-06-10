using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CajeroAutomaticoAPI.Models
{
    public class ApiResponse
    {
        public int Codigo { get; set; }
        public string Mensaje { get; set; }
        public object Data { get; set; }
    }
}