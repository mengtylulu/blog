using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace mengtylulu.Converters
{
    public class DateTimeConverterWithLocalTime : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            //解析逻辑
            if (DateTime.TryParse(reader.GetString(), CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out DateTime dateTime))
            {
                if (dateTime.Kind == DateTimeKind.Unspecified)
                {
                    return DateTime.SpecifyKind(dateTime, DateTimeKind.Local);
                }
                if (dateTime.Kind == DateTimeKind.Utc)
                {
                    return dateTime.ToLocalTime();
                }
            }
            return reader.GetDateTime();
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            //ToString("O"):用于将日期时间转换为ISO 8601 标准格式的字符串
            writer.WriteStringValue(value.ToLocalTime().ToString("o"));
        }
    }
}
