using Microsoft.Extensions.Primitives;
using Microsoft.OpenApi.Models;
using Microsoft.VisualBasic;
using Npgsql;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace mengtylulu.Infrastructure
{
    public class Repository<T> : IRepository<T> where T : class, new()
    {
        private readonly NpgsqlDataSource _dataSource;
        private readonly string _tableName;
        public Repository(NpgsqlDataSource dataSource)
        {
            _dataSource = dataSource;
            _tableName = typeof(T).Name.ToLower();
        }

        public Task<T> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            using var cmd = _dataSource.CreateCommand($"SELECT * FROM {_tableName} WHERE id= @ID");
            cmd.Parameters.AddWithValue("@ID", id);
            using var reader = cmd.ExecuteReader();

            // 如果没有查询到数据，返回null
            if (!await reader.ReadAsync())
                return null;

            //1.获取T类型的元数据
            Type entityType = typeof(T);

            //创建T的实例(要求T有默认构造函数 后面会加约束)
            T entity = Activator.CreateInstance<T>();

            //2.遍历DataReader 中所有列
            for (int i = 0; i < reader.FieldCount; i++)
            {
                //获取数据库列名
                string columnName = reader.GetName(i);
                //将列明转换为实体属性名名(如"user_name" -> "UserName")
                string propertyName = ConvertColumnNameToPropertyName(columnName);

                //3.查找实体中对应的属性
                PropertyInfo? property = entityType.GetProperty(
                    propertyName,
                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase
                    );

                //如果属性不存在,跳过当前列
                if (property == null)
                    continue;

                //4.处理空值(数据库的DBNull 转换为C#的null)
                if (reader.IsDBNull(i))
                {
                    //如果属性是可空类型(如int?),可以赋值为null
                    if (property.PropertyType.IsReferenceOrNullableType())
                    {
                        property.SetValue(entity, null);
                    }
                    continue;
                }

                object? value = reader.GetValue(i);
                object? convertedValue = ConvertValueToPropertyType(value, property.PropertyType);

                //6.给属性赋值
                property.SetValue(entity, convertedValue);
            }
            return entity;
        }

        public async Task<bool> InsertAsync(T Entity)
        {
            if (Entity == null)
                throw new ArgumentNullException(nameof(Entity), "参数不能为null");

            Type entityType = typeof(T);
            var tableName = SqlHelp.ConvertStringToPgsqlName(entityType.Name);
            //using var cmd = _dataSource.CreateCommand($"INSERT INTO {ConvertPropertyNameToColumnName(entityType.Name)}");

            //获取实体属性
            var properties = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && p.CanWrite)
                .ToList();

            if (!properties.Any())
                throw new InvalidOperationException("实体没有可读写的公共属性");

            var columnNames = new List<string>();
            var parameterNames = new List<string>();
            var parameters = new List<NpgsqlParameter>();

            foreach (var property in properties)
            {
                //获取实体类型
                Type propertyType = property.PropertyType;

                //获取属性值
                object? value = property.GetValue(Entity);

                //列明转义(处理特殊字符)
                string columnName = SqlHelp.ConvertStringToPgsqlName(property.Name);
                columnNames.Add(columnName);

                //参数名
                string paramName = $"@{property.Name}";
                parameterNames.Add(paramName);

                //创建参数
                var param = new NpgsqlParameter(paramName, value ?? DBNull.Value)
                {
                    NpgsqlDbType = SqlHelp.GetNpgsqlDbType(propertyType)
                };
                parameters.Add(param);
            }

            //构建完整sql语句
            string sql = $@"INSERT INTO {tableName} ({string.Join(",", columnNames)}) VALUES ({string.Join(",", parameterNames)})";

            using var cmd = _dataSource.CreateCommand(sql);
            //添加所有参数(防止sql注入)
            cmd.Parameters.AddRange(parameters.ToArray());

            //执行
            int affectedRows = await cmd.ExecuteNonQueryAsync();

            return affectedRows > 0;
        }

        public Task<T> UpdateAsync(T Entity)
        {
            throw new NotImplementedException();
        }


        //辅助方法:将数据库列名(下划线命名)转换为属性名(帕斯卡命名)
        //例 "user_name" -> "UserName","create_time"->"CreateTime"
        private string ConvertColumnNameToPropertyName(string columnName)
        {
            return string.Join("", columnName.Split('_')
                .Select(part => part.Length > 0
                ? char.ToUpper(part[0]) + part.Substring(1).ToLower()
                : part));
        }



        //辅助方法:将数据库值转换为属性所需的类型
        private object? ConvertValueToPropertyType(object value, Type targetType)
        {
            //如果目标类型是可空类型(如int?) 获取其基础类型(如 int)
            Type underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;

            //处理枚举类型(数据库存储的是数值,转换为枚举)
            if (underlyingType.IsEnum)
            {
                return Enum.ToObject(underlyingType, value);
            }

            //处理DateTime (可能需要转换 时区,根据需求调整)
            if (underlyingType == typeof(DateTime) && value is DateTimeOffset dateTimeOffset)
            {
                return dateTimeOffset.UtcDateTime;
            }

            //通用类型转换 (long->int string->Guid)
            return Convert.ChangeType(value, underlyingType);
        }
    }
}
