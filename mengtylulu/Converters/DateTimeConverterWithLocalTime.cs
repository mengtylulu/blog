using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace mengtylulu.Converters
{
    public class DateTimeConverterWithLocalTime : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var timeStr = reader.GetString();
            
            //解析
            if (!DateTime.TryParse(reader.GetString(), CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out DateTime dateTime))
                throw new JsonException($"无法解析时间:{reader.GetString()}");


            //时间格式为设置为 yyyy-MM-dd HH:mm:ss (ISO 标准格式)
            DateTime time = new DateTime(
               dateTime.Year,
               dateTime.Month,
               dateTime.Day,
               dateTime.Hour,
               dateTime.Minute,
               dateTime.Second,
               dateTime.Kind
               );

            //若类型为Utc(Coordinated Universal Time) 或 Unspecified 全部转换为 本地时间
            return time.Kind switch
            {
                DateTimeKind.Utc => time.ToLocalTime(),
                DateTimeKind.Unspecified => DateTime.SpecifyKind(time, DateTimeKind.Local),
                _ => time
            };
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            //ToString("O"):用于将日期时间转换为ISO 8601 标准格式的字符串
            writer.WriteStringValue(value.ToLocalTime().ToString("o"));
        }
    }
}
