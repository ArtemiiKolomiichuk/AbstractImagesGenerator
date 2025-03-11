namespace AbstractImagesGenerator.Misc
{
    public class Layer
    {
        public string title;
        public string type;
        public List<LayerSetting> settings = [];

        public Layer Copy
        {
            get
            {
                Layer layer = new()
                {
                    title = $"{title} *",
                    type = type,
                    settings = [.. settings.Select(x => x.Copy)]
                };
                return layer;
            }
        }
    }

    public class LayerSetting
    {
        public string title;
        public LayerSettingType type;
        public object? value;

        public LayerSetting Copy
        {
            get
            {
                LayerSetting setting = new()
                {
                    title = title,
                    type = type,
                    value = value
                };
                return setting;
            }
        }
    }

    public enum LayerSettingType
    {
        PositiveNumber,
        Number,
        Integer,

        Scale = PositiveNumber,
        Angle = 3,

        Color
    }
}
