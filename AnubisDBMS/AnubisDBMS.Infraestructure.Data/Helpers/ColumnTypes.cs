namespace AnubisDBMS.Infraestructure.Data.Helpers
{
    /// <summary>
    ///     Tipos de Columna de base de datos para SQL Server
    /// </summary>
    public static class ColumnTypes
    {
        /// <summary>
        ///     Tipo de Columna VARCHAR
        /// </summary>
        public const string Varchar = "varchar";

        /// <summary>
        ///     Tipo de Columna DATE (Unicamente fecha)
        /// </summary>
        public const string Date = "date";

        public const string Decimal = "decimal";
        public const string Int = "int";
        public const string Bool = "bool";
    }
}