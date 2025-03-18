#pragma warning disable CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace AbstractImagesGenerator.Misc
{
    public abstract class Layer
    {
        [JsonProperty("visible_name")]
        public string Title { get; set; }

        [JsonProperty("name")]
        public string Type { get; set; }

        [JsonProperty("parameters")]
        public List<LayerSetting> Settings { get; set; } = [];
        public List<LayerSetting> InheritedSettings { get; set; } = [];

        [JsonIgnore]
        public virtual Layer Copy { get; }

        public void UpdateInheritedSettings(List<LayerSetting> newSettings)
        {
            if (InheritedSettings.Count != newSettings.Count || InheritedSettings.Select((x, i) => x != newSettings[i]).Any())
            {
                InheritedSettings = [..newSettings.Select(x => x.Copy)];
            }
        }
    }

    public class Drawing : Layer
    {        
        public override Drawing Copy
        {
            get => new()
            {
                Title = Title,
                Type = Type,
                Settings = [.. Settings.Select(x => x.Copy)],
                InheritedSettings = [.. InheritedSettings.Select(x => x.Copy)]
            };
        }

        internal static Drawing[] LayerOptions { get; set; }

        public static async Task<Drawing[]> GetLayerOptions()
        {
            if (LayerOptions == null)
            {
                string json = await new HttpClient().GetStringAsync($"{Program.BaseApiUrl}/image-generator/model");
                var layers = JsonConvert.DeserializeObject<LayersModel>(json);
                LayerOptions = layers.Drawings;
                Blending.LayerOptions = layers.Blendings;
            }
            return [..LayerOptions.Select(x => x.Copy)];
        }
    }

    public class Blending : Layer
    {
        public string Id { get; private init; } = Guid.NewGuid().ToString();
        [JsonProperty("blending_parameters")]
        public List<LayerSetting> HereditarySettings { get; set; } = [];
        public List<Layer> SubLayers { get; set; } = [];

        public override Blending Copy
        {
            get => new()
            {
                Id = Guid.NewGuid().ToString(),
                Title = Title,
                Type = Type,
                Settings = [.. Settings.Select(x => x.Copy)],
                InheritedSettings = [.. InheritedSettings.Select(x => x.Copy)],
                HereditarySettings = [.. HereditarySettings.Select(x => x.Copy)],
                SubLayers = [.. SubLayers.Select(x => x.Copy)]
            };
        }

        internal static Blending[] LayerOptions { get; set; }

        public static async Task<Blending[]> GetLayerOptions()
        {
            if(LayerOptions == null)
            {
                string json = await new HttpClient().GetStringAsync($"{Program.BaseApiUrl}/image-generator/model");
                var layers = JsonConvert.DeserializeObject<LayersModel>(json);
                LayerOptions = layers.Blendings;
                Drawing.LayerOptions = layers.Drawings;
            }
            return [..LayerOptions.Select(x => x.Copy)];
        }
    }

    [JsonObject]
    public class LayerSetting
    {
        [JsonProperty("visible_name")]
        public string Title { get; set; }
        [JsonProperty("name")]
        public string Type { get; set; }
        [JsonProperty("data_type")]
        public PropertyValueType DataType { get; set; }
        [JsonProperty("visible_type")]
        public PropertyDisplayType VisibleType { get; set; }
        [JsonProperty("default")]
        public SettingValue? Value { get; set; }
        [JsonProperty("min_value")]
        public object? MinValue { get; set; }
        [JsonProperty("max_value")]
        public object? MaxValue { get; set; }
        [JsonProperty("possible_values")]
        public Dictionary<string, SettingValue>? PossibleValues { get; set; }
        [JsonProperty("min_length")]
        public int? MinCount { get; set; }
        [JsonProperty("max_count")]
        public int? MaxCount { get; set; }

        [JsonIgnore]
        public LayerSetting Copy
        {
            get
            {
                LayerSetting setting = new()
                {
                    Title = Title,
                    Type = Type,
                    DataType = DataType,
                    VisibleType = VisibleType,
                    Value = Value switch
                    {
                        IntValue intRecord => new IntValue(intRecord.Value),
                        FloatValue floatRecord => new FloatValue(floatRecord.Value),
                        BoolValue boolValue => new BoolValue(boolValue.Value),
                        IntTupleValue intTupleValue => new IntTupleValue((intTupleValue.Values.Item1, intTupleValue.Values.Item2)),
                        FloatTupleValue floatTupleValue => new FloatTupleValue((floatTupleValue.Values.Item1, floatTupleValue.Values.Item2)),
                        StringListValue stringListValue => new StringListValue([.. stringListValue.Values]),
                        StringValue stringValue => new StringValue(stringValue.Value),
                        _ => throw new NotImplementedException()
                    },
                    MinValue = MinValue,
                    MaxValue = MaxValue,
                    PossibleValues = PossibleValues,
                    MinCount = MinCount,
                    MaxCount = MaxCount
                };
                return setting;
            }
        }

        public static bool operator ==(LayerSetting a, LayerSetting b) => a.Type == b.Type && a.DataType == b.DataType && a.VisibleType == b.VisibleType && a.MinValue == b.MinValue && a.MaxValue == b.MaxValue && a.MinCount == b.MinCount && a.MaxCount == b.MaxCount;
        public static bool operator !=(LayerSetting a, LayerSetting b) => !(a == b);
    }


    [JsonConverter(typeof(StringEnumConverter))]
    public enum PropertyValueType
    {
        [EnumMember(Value = "integer_tuple")]
        IntTuple,
        [EnumMember(Value = "float_tuple")]
        FloatTuple,
        [EnumMember(Value = "integer")]
        Int,
        [EnumMember(Value = "float")]
        Float,
        [EnumMember(Value = "colors")]
        Colors,
        [EnumMember(Value = "enum_list")]
        EnumList,
        [EnumMember(Value = "bool")]
        Bool
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum PropertyDisplayType
    {
        [EnumMember(Value = "field")]
        Field,
        [EnumMember(Value = "slider")]
        Slider,
        [EnumMember(Value = "range_slider")]
        RangeSlider,
        [EnumMember(Value = "checkbox")]
        Checkbox,
        [EnumMember(Value = "colors")]
        Colors,
        [EnumMember(Value = "selector")]
        Dropdown
    }

    internal class LayersModel
    {
        [JsonProperty("algorithms")]
        public Drawing[] Drawings { get; set; }
        [JsonProperty("blendings")]
        public Blending[] Blendings { get; set; }
    }
}
