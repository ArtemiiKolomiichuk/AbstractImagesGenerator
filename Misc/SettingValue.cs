using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AbstractImagesGenerator.Misc
{
    [JsonConverter(typeof(ValueRecordConverter))]
    public abstract record SettingValue
    {
        public static implicit operator SettingValue(List<string> values) => new StringListValue(values);
        public static implicit operator SettingValue((int, int) values) => new IntTupleValue(values);
        public static implicit operator SettingValue((double, double) values) => new FloatTupleValue(values);
        public static implicit operator SettingValue(int value) => new IntRecord(value);
        public static implicit operator SettingValue(double value) => new FloatRecord(value);
        public static implicit operator SettingValue(bool value) => new BoolValue(value);

        public static SettingValue FromObject(object o)
        {
            return o switch
            {
                int i => i,
                double d => d,
                bool b => b,
                string s => new StringListValue([s]),
                List<string> l => l,
                int[] i => (i[0], i[1]),
                double[] d => (d[0], d[1]),
                _ => throw new ArgumentException("Unknown object type"),
            };
        }

        public static object ToObject(SettingValue value)
        {
            return value switch
            {
                IntRecord i => i.Value,
                FloatRecord f => f.Value,
                BoolValue b => b.Value,
                StringListValue s => s.Values,
                IntTupleValue i => new int[] { i.Values.Item1, i.Values.Item2 },
                FloatTupleValue f => new double[] { f.Values.Item1, f.Values.Item2 },
                _ => throw new ArgumentException("Unknown SettingValue type"),
            };
        }
    }

    public record StringListValue(List<string> Values) : SettingValue
    {
        public static implicit operator List<string>(StringListValue value) => value.Values;
    }

    public record IntTupleValue((int, int) Values) : SettingValue
    {
        public static implicit operator (int, int)(IntTupleValue value) => value.Values;
    }

    public record FloatTupleValue((double, double) Values) : SettingValue
    {
        public static implicit operator (double, double)(FloatTupleValue value) => value.Values;
    }

    public record IntRecord(int Value) : SettingValue
    {
        public static implicit operator int(IntRecord value) => value.Value;
    }

    public record FloatRecord(double Value) : SettingValue
    {
        public static implicit operator double(FloatRecord value) => value.Value;
    }

    public record BoolValue(bool Value) : SettingValue
    {
        public static implicit operator bool(BoolValue value) => value.Value;
    }

    public class ValueRecordConverter : JsonConverter<SettingValue>
    {
        public override SettingValue ReadJson(JsonReader reader, Type objectType, SettingValue existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jToken = JToken.Load(reader);

            if (jToken.Type == JTokenType.Boolean)
                return new BoolValue(jToken.ToObject<bool>());

            if(jToken.Type == JTokenType.Integer)
                return new IntRecord(jToken.ToObject<int>());

            if (jToken.Type == JTokenType.Float)
                return new FloatRecord(jToken.ToObject<double>());

            if (jToken.Type == JTokenType.Array)
            {
                var array = jToken.ToObject<JArray>();
                if (array.Count == 2)
                {
                    if (array[0].Type == JTokenType.Integer)
                        return new IntTupleValue((array[0].ToObject<int>(), array[1].ToObject<int>()));
                    if (array[0].Type == JTokenType.Float)
                        return new FloatTupleValue((array[0].ToObject<double>(), array[1].ToObject<double>()));
                }
                return new StringListValue(array.ToObject<List<string>>());
            }

            throw new JsonSerializationException("Unknown ValueRecord format");
        }

        public override void WriteJson(JsonWriter writer, SettingValue value, JsonSerializer serializer)
        {
            switch (value)
            {
                case BoolValue boolValue:
                    serializer.Serialize(writer, boolValue.Value);
                    break;
                case IntTupleValue intTupleValue:
                    serializer.Serialize(writer, new int[] { intTupleValue.Values.Item1, intTupleValue.Values.Item2 });
                    break;
                case FloatTupleValue floatTupleValue:
                    serializer.Serialize(writer, new double[] { floatTupleValue.Values.Item1, floatTupleValue.Values.Item2 });
                    break;
                case StringListValue stringListValue:
                    serializer.Serialize(writer, stringListValue.Values);
                    break;
                case IntRecord intRecord:
                    serializer.Serialize(writer, intRecord.Value);
                    break;
                case FloatRecord floatRecord:
                    serializer.Serialize(writer, floatRecord.Value);
                    break;
                default:
                    throw new JsonSerializationException("Unknown ValueRecord type");
            }
        }
    }

}
