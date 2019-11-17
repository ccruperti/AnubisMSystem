using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnubisDBMS.Infraestructure.Extensions
{
    public static class NumericTypesExtensions
    {
        /// <summary>
        /// Redondea Int al valor mas cercano a la Integral indicada.
        /// </summary>
        /// <param name="integer"></param>
        /// <param name="integralValue">Valor Integral</param>
        /// <returns></returns>
        public static int RoundToInt(this int integer, double integralValue)
        {
            var round = (Math.Round(integer / integralValue)) * integralValue;
            return Convert.ToInt32(round);
        }

        public static int RoundUpToInt(this int integer, double integralValue)
        {
            var round = (Math.Ceiling(integer / integralValue)) * integralValue;
            return Convert.ToInt32(round);
        }

        public static int RoundDownToInt(this int integer, double integralValue)
        {
            var round = (Math.Floor(integer / integralValue)) * integralValue;
            return Convert.ToInt32(round);
        }
    }
}
