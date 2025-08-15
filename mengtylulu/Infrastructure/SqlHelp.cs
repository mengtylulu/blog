using Npgsql;
using Npgsql.Internal;
using Npgsql.Schema;
using NpgsqlTypes;
using System.Text;

namespace mengtylulu.Infrastructure
{
    public static class SqlHelp
    {
        private static readonly NpgsqlCommandBuilder _commandBuilder = new NpgsqlCommandBuilder();

        /// <summary>
        /// 转义标识符(表名，列名),符合psql语法规则 user->"user"
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public static string QuoteIdentifier(string identifier)
        {
            return _commandBuilder.QuoteIdentifier(identifier);
        }

        /// <summary>
        /// 将数据库列名(下划线命名)转换为属性名(帕斯卡命名)
        /// "UserName" -> "user_name","CreateTime"->"create_time"
        /// </summary>
        /// <param name="str">要转换的字符转</param>
        /// <returns></returns>
        public static string ConvertStringToPgsqlName(string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;
            var result = new StringBuilder();
            result.Append(char.ToLower(str[0]));
            for (int i = 1; i < str.Length; i++)
            {
                if (char.IsUpper(str[i]))
                {
                    result.Append("_");
                    result.Append(char.ToLower(str[i]));
                }
                else
                    result.Append(str[i]);
            }
            return QuoteIdentifier(result.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static NpgsqlDbType GetNpgsqlDbType(Type type)
        {
            //类型映射官网
            //https://www.npgsql.org/doc/types/basic.html 
            if (type == null)
                throw new ArgumentNullException(nameof(type),"类型不能为nul");
            Type underlyingType = Nullable.GetUnderlyingType(type) ?? type;
            return underlyingType switch
            {
                Type t when t == typeof(int) => NpgsqlDbType.Integer,
                Type t when t == typeof(string) => NpgsqlDbType.Text,
                Type t when t == typeof(DateTime) => NpgsqlDbType.Timestamp,

                Type t when t == typeof(Guid) => NpgsqlDbType.Uuid,
                Type t when t == typeof(bool) => NpgsqlDbType.Boolean,
                Type t when t == typeof(decimal) => NpgsqlDbType.Numeric,
                _ => throw new NotSupportedException($"未找到类型映射: {underlyingType.FullName}"),
            };
        }
    }
}
