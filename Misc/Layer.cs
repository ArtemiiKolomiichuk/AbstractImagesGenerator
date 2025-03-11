#pragma warning disable CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
using System.Collections.Generic;

namespace AbstractImagesGenerator.Misc
{
    public abstract class Layer
    {
        public string title;
        public string type;
        public List<LayerSetting> settings = [];
        public List<LayerSetting> inheritedSettings = [];

        public virtual Layer Copy { get; }

        public void UpdateInheritedSettings(List<LayerSetting> newSettings)
        {
            if (inheritedSettings.Count != newSettings.Count || inheritedSettings.Select((x, i) => x != newSettings[i]).Any())
            {
                inheritedSettings = [..newSettings.Select(x => x.Copy)];
            }
        }
    }

    public class Drawing : Layer
    {        
        public override Drawing Copy
        {
            get => new()
            {
                title = title,
                type = type,
                settings = [.. settings.Select(x => x.Copy)],
                inheritedSettings = [.. inheritedSettings.Select(x => x.Copy)]
            };
        }
    }

    public class Blending : Layer
    {
        public string Id { get; private init; } = Guid.NewGuid().ToString();
        public List<LayerSetting> hereditarySettings = [];
        public List<Layer> subLayers = [];

        public override Blending Copy
        {
            get => new()
            {
                title = title,
                type = type,
                settings = [.. settings.Select(x => x.Copy)],
                hereditarySettings = [.. hereditarySettings.Select(x => x.Copy)],
                subLayers = [.. subLayers.Select(x => x.Copy)],
                inheritedSettings = [.. inheritedSettings.Select(x => x.Copy)]
            };
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

        public static List<LayerSetting> RandomSettings()
        {
            Random random = new();
            List<LayerSetting> list = [];
            for (int i = 0; i < random.Next() * 4; i++)
            {
                list.Add(new LayerSetting
                {
                    title = random.Next().ToString(),
                    type = (LayerSettingType)random.Next(4),
                });
            }
            return list;
        }

        public static bool operator ==(LayerSetting a, LayerSetting b) => a.title == b.title && a.type == b.type;
        public static bool operator !=(LayerSetting a, LayerSetting b) => a.title != b.title || a.type != b.type;
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
