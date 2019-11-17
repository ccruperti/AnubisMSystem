using System;
using System.ComponentModel.DataAnnotations;

namespace AnubisDBMS.Infraestructure.Validators
{
    /// <summary>
    ///     Clase estatica para funciones de verificacion de numeros de documentos nacionales y codigos. Aplicable a Ecuador
    /// </summary>
    public static class DocumentosNacionalesValidator
    {
        /// <summary>
        ///     Valida si el numero de cedula ingresado es valido
        /// </summary>
        /// <param name="cedula">Numero de Cedula</param>
        /// <returns></returns>
        public static bool ValidarCedula(string cedula)
        {
            int isNumeric;
            var total = 0;
            const int tamanoLongitudCedula = 10;
            int[] coeficientes = {2, 1, 2, 1, 2, 1, 2, 1, 2};
            const int numeroProvincias = 24;
            const int tercerDigito = 6;

            if (int.TryParse(cedula, out isNumeric) && cedula.Length == tamanoLongitudCedula)
            {
                var provincia = Convert.ToInt32(string.Concat(cedula[0], cedula[1], string.Empty));
                var digitoTres = Convert.ToInt32(cedula[2] + string.Empty);
                if (provincia > 0 && provincia <= numeroProvincias && digitoTres < tercerDigito)
                {
                    var digitoVerificadorRecibido = Convert.ToInt32(cedula[9] + string.Empty);
                    var contador = 0;
                    for (var k = 0; k < coeficientes.Length; k++)
                    {
                        contador++;
                        var valor = Convert.ToInt32(coeficientes[k] + string.Empty) *
                                    Convert.ToInt32(cedula[k] + string.Empty);
                        total = valor >= 10 ? total + (valor - 9) : total + valor;
                    }
                    var digitoVerificadorObtenido = total % 10 != 0 ? 10 - total % 10 : 0;
                    return digitoVerificadorObtenido == digitoVerificadorRecibido;
                }
                return false;
            }
            return false;
        }
    }

    public class CedulaAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value.GetType() == Type.GetType("string"))
                return DocumentosNacionalesValidator.ValidarCedula((string) value);
            return false;
        }
    }
}