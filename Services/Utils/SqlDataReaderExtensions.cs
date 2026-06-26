using Microsoft.Data.SqlClient;

namespace Services.Utils
{
    public static class SqlDataReaderExtensions
    {
        public static bool HasColumn(this SqlDataReader reader, string columnName)
        {
            try
            {
                reader.GetOrdinal(columnName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static T GetValue<T>(this SqlDataReader sqlReader, string column, Func<SqlDataReader, string, T> converter = null)
        {
            if (converter != null)
            {
                return converter(sqlReader, column);
            }

            if (sqlReader.HasColumn(column) && sqlReader[column] != DBNull.Value)
            {
                return (T)Convert.ChangeType(sqlReader[column], Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T));
            }
            return default;
        }
    }
}
