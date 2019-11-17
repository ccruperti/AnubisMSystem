using System.Collections.Generic;

namespace AnubisDBMS.Data.Localization.Entities
{
    public class Idioma
    {
        public int IdIdioma { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string WebLang { get; set; }
        public string AspCulture { get; set; }
        public string CodigoISO { get; set; }
        public bool Defecto { get; set; }

        public bool Disponible { get; set; }

        // Virtual
        public virtual ICollection<Pais> Paises { get; set; }
    }
}