using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration.Configuration;

namespace AnubisDBMS.Infraestructure.Data.Extensions
{
    /// <summary>
    ///     Extensiones del Mapeador de Configuracion de Fluent API para Entity Framework
    /// </summary>
    public static class MappingExtensions
    {
        /// <summary>
        ///     Configura la propiedad para que sea Indice Unico
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static PrimitivePropertyConfiguration IsUnique(this PrimitivePropertyConfiguration configuration)
        {
            return configuration.HasColumnAnnotation("Index",
                new IndexAnnotation(new IndexAttribute {IsUnique = true}));
        }
    }
}