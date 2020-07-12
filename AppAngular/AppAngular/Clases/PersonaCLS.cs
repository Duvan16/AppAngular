using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppAngular.Clases
{
    public class PersonaCLS
    {
        public int iidpersona { get; set; }
        public string nombreCompleto { get; set; }
        public string telefono { get; set; }
        public string correo { get; set; }
        public DateTime fechaNacimiento { get; set; }
        public int bhabilitado { get; set; }
        public string nombre { get; set; }
        public string apPaterno { get; set; }
        public string apMaterno { get; set; }

        /// <summary>
        /// /año - mes - dia
        /// </summary>
        public string fechaCadena { get; set; }
    }
}
