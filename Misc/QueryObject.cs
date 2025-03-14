using Newtonsoft.Json;

namespace AbstractImagesGenerator.Misc
{
    public class QueryObject
    {
        public QueryObject(int width, int height, Blending blending)
        {
            Width = width;
            Height = height;
            FinalBlending = new BlendingQuery(blending);
        }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("layer_query")]
        public LayerQuery FinalBlending { get; set; }
    }

    public abstract class LayerQuery
    {
        [JsonProperty("name")]
        public string Title { get; set; }

        [JsonProperty("generator_type")]
        public string Type { get; set; }
    }

    public class DrawingQuery : LayerQuery
    {
        [JsonProperty("values")]
        public Dictionary<string, object> Values { get; set; }

        [JsonProperty("blending_values")]
        public Dictionary<string, object> BlendingValues { get; set; }

        public DrawingQuery(Drawing drawing)
        {
            Title = drawing.Type;
            Type = "algorithm";
            Values = [];
            foreach (var setting in drawing.Settings)
            {
                Values[setting.Type] = setting.Value switch
                {
                    IntRecord intRecord => intRecord.Value,
                    FloatRecord floatRecord => floatRecord.Value,
                    BoolValue boolValue => boolValue.Value,
                    IntTupleValue intTupleValue => new int[] { intTupleValue.Values.Item1, intTupleValue.Values.Item2 },
                    FloatTupleValue floatTupleValue => new double[] { floatTupleValue.Values.Item1, floatTupleValue.Values.Item2 },
                    StringListValue stringListValue => stringListValue.Values,
                    _ => null
                };
            }
            BlendingValues = [];
            foreach (var setting in drawing.InheritedSettings)
            {
                BlendingValues[setting.Type] = setting.Value switch
                {
                    IntRecord intRecord => intRecord.Value,
                    FloatRecord floatRecord => floatRecord.Value,
                    BoolValue boolValue => boolValue.Value,
                    IntTupleValue intTupleValue => new int[] { intTupleValue.Values.Item1, intTupleValue.Values.Item2 },
                    FloatTupleValue floatTupleValue => new double[] { floatTupleValue.Values.Item1, floatTupleValue.Values.Item2 },
                    StringListValue stringListValue => stringListValue.Values,
                    _ => null
                };
            }
        }
    }

    public class BlendingQuery : LayerQuery
    {
        [JsonProperty("layers")]
        public List<LayerQuery> SubLayers { get; set; }

        public BlendingQuery(Blending blending)
        {
            Title = blending.Type;
            Type = "blending";
            SubLayers = [];
            foreach (var layer in blending.SubLayers)
            {
                SubLayers.Add(layer switch
                {
                    Drawing drawing => new DrawingQuery(drawing),
                    Blending _blending => new BlendingQuery(_blending)
                });
            }
        }
    }
}