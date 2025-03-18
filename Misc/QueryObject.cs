using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AbstractImagesGenerator.Misc
{
    public class QueryObject
    {
        public QueryObject() { }

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
        public BlendingQuery FinalBlending { get; set; } 

        public static async Task<(Blending, int, int)?> Deconstruct(string query)
        {
            var queryObject = JsonConvert.DeserializeObject<QueryObject>(query);
            if (queryObject == null || queryObject.FinalBlending == null) return null;
            var defaulltDrawings = await Drawing.GetLayerOptions();
            var defaultBlendings = await Blending.GetLayerOptions();

            Drawing DeconstructDrawing(Drawing[] defaultDrawings, Blending parentBlending, DrawingQuery drawingQuery)
            {
                var drawing = defaultDrawings.First(x => x.Type == drawingQuery.Type).Copy;
                foreach (var setting in drawingQuery.Values)
                {
                    drawing.Settings.First(x => x.Type == setting.Key).Value = SettingValue.FromObject(setting.Value);
                }
                drawing.InheritedSettings = [.. parentBlending.HereditarySettings.Select(x => x.Copy)];
                foreach (var setting in drawingQuery.BlendingValues)
                {
                    drawing.InheritedSettings.First(x => x.Type == setting.Key).Value = SettingValue.FromObject(setting.Value);
                }
                return drawing;
            }

            Blending DeconstructBlending(Blending[] defaultBlendings, Drawing[] defaultDrawings, Blending? parentBlending, BlendingQuery blendingQuery)
            {
                var blending = defaultBlendings.First(x => x.Type == blendingQuery.Type).Copy;
                foreach (var setting in blendingQuery.Values)
                {
                    blending.Settings.First(x => x.Type == setting.Key).Value = SettingValue.FromObject(setting.Value);
                }
                if (parentBlending != null)
                {
                    blending.InheritedSettings = [.. parentBlending.HereditarySettings.Select(x => x.Copy)];
                    foreach (var setting in blendingQuery.BlendingValues)
                    {
                        blending.InheritedSettings.First(x => x.Type == setting.Key).Value = SettingValue.FromObject(setting.Value);
                    }
                }
                blending.SubLayers = [];
                foreach (var layer in blendingQuery.SubLayers)
                {
                    blending.SubLayers.Add(layer switch
                    {
                        BlendingQuery _blendingQuery => DeconstructBlending(defaultBlendings, defaultDrawings, blending, _blendingQuery),
                        DrawingQuery drawingQuery => DeconstructDrawing(defaultDrawings, blending, drawingQuery),
                        _ => throw new NotImplementedException()
                    });
                }
                return blending;
            }

            return (DeconstructBlending(defaultBlendings, defaulltDrawings, null, queryObject.FinalBlending), queryObject.Width, queryObject.Height);
        }
    }

    [JsonConverter(typeof(LayerQueryConverter))]
    public abstract class LayerQuery
    {
        [JsonProperty("name")]
        public string Type { get; set; }

        [JsonProperty("generator_type")]
        public string LayerType { get; set; }

        [JsonProperty("values")]
        public Dictionary<string, object> Values { get; set; } = [];

        [JsonProperty("blending_values")]
        public Dictionary<string, object> BlendingValues { get; set; } = [];
    }

    public class DrawingQuery : LayerQuery
    {
        public DrawingQuery() { }
        public DrawingQuery(Drawing drawing)
        {
            Type = drawing.Type;
            LayerType = "algorithm";
            Values = [];
            foreach (var setting in drawing.Settings)
            {
                Values[setting.Type] = SettingValue.ToObject(setting.Value);
            }
            BlendingValues = [];
            foreach (var setting in drawing.InheritedSettings)
            {
                BlendingValues[setting.Type] = SettingValue.ToObject(setting.Value);
            }
        }
    }

    public class BlendingQuery : LayerQuery
    {
        [JsonProperty("layers")]
        public List<LayerQuery> SubLayers { get; set; } = [];

        public BlendingQuery() { }

        public BlendingQuery(Blending blending)
        {
            Type = blending.Type;
            LayerType = "blending";
            SubLayers = [];
            foreach (var layer in blending.SubLayers ?? [])
            {
                SubLayers.Add(layer switch
                {
                    Drawing drawing => new DrawingQuery(drawing),
                    Blending _blending => new BlendingQuery(_blending)
                });
            }
            foreach (var setting in blending.Settings ?? [])
            {
                Values[setting.Type] = SettingValue.ToObject(setting.Value);
            }
            BlendingValues = [];
            foreach (var setting in blending.InheritedSettings ?? [])
            {
                BlendingValues[setting.Type] = SettingValue.ToObject(setting.Value);
            }
        }
    }


    public class LayerQueryConverter : JsonConverter<LayerQuery>
    {
        public override LayerQuery? ReadJson(JsonReader reader, Type objectType, LayerQuery? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);
            var layerType = jObject["generator_type"].Value<string>();

            LayerQuery layerQuery;
            if (layerType == "algorithm")
            {
                layerQuery = new DrawingQuery();
            }
            else
            {
                layerQuery = new BlendingQuery();
            }
            layerQuery.Type = jObject["name"].Value<string>();
            layerQuery.LayerType = layerType;
            layerQuery.Values = jObject["values"]?.ToObject<Dictionary<string, object>>() ?? [];
            layerQuery.BlendingValues = jObject["blending_values"]?.ToObject<Dictionary<string, object>>() ?? [];

            if (layerQuery is BlendingQuery blendingQuery)
            {
                blendingQuery.SubLayers = jObject["layers"]?.ToObject<List<LayerQuery>>(serializer) ?? [];
            }
            return layerQuery;
        }

        public override void WriteJson(JsonWriter writer, LayerQuery? value, JsonSerializer serializer)
        {
            var jObject = new JObject
            {
                { "name", value.Type },
                { "generator_type", value.LayerType },
            };

            if(value.Values.Count != 0)
            {
                jObject.Add("values", JToken.FromObject(value.Values, serializer));
            }
            if(value.BlendingValues.Count != 0)
            {
                jObject.Add("blending_values", JToken.FromObject(value.BlendingValues, serializer));
            }


            if (value is BlendingQuery blendingQuery)
            {
                jObject.Add("layers", JToken.FromObject(blendingQuery.SubLayers, serializer));
            }

            jObject.WriteTo(writer);
        }
    }
}