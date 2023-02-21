using System;
using System.Collections.Generic;
using System.Text;

namespace Finanzas.Models
{
    internal class MontoModel
    {
        //Variables a utilizar para los Ingresos/Egresos
        public string nombreAccion { get; set; }
        public Guid registroID { get; set; }
        public int cantidad { get; set; }
        public string descripcion { get; set; }
        public string fechaSubida { get; set; }
        public string fechaUpdate { get; set; }
    }
}
