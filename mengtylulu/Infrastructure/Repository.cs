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

        public async Task<T> InsertAsync(T Entity)
        {
            if (Entity == null)
                throw new ArgumentNullException(nameof(Entity), "参数不能为null");

            using var cmd = _dataSource.CreateCommand("INSERT INTO @table_name");
            Type entityType = typeof(T);

            cmd.Parameters.AddWithValue("@table_name", ConvertPropertyNameToColumnName(entityType.Name));
            var properties = entityType.GetProperties();

            IDictionary<string, object?> dic = new Dictionary<string, object?>();
            foreach (var property in properties)
            {
                Type type = property.GetType();
                //值类型
                if (type.IsValueType)
                {
                    object? value = property.GetValue(Entity);
                    dic.Add(property.Name, value);
                }
                //引用类型
                if (type.IsClass)
                {
                    object? value = property.GetValue(Entity);
                    dic.Add(property.Name, value);
                }
            }

            var str = dic.SelectMany((k,v) => k.ToString());
            var str2 = dic.Select((k,v) => k.ToString());
            StringBuilder insertColumnName= new StringBuilder();
            var test = cmd.CommandText;

            await Task.CompletedTask;
            return null;
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

        //辅助方法:将数据库列名(下划线命名)转换为属性名(帕斯卡命名)
        //例 "UserName" -> "user_name","CreateTime"->"create_time"
        private string ConvertPropertyNameToColumnName(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                return propertyName;
            var result = new StringBuilder();
            result.Append(char.ToLower(propertyName[0]));
            for (int i = 1; i < propertyName.Length; i++)
            {
                if (char.IsUpper(propertyName[i]))
                {
                    result.Append("_");
                    result.Append(char.ToLower(propertyName[i]));
                }
                else
                    result.Append(propertyName[i]);
            }
            return result.ToString();
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
