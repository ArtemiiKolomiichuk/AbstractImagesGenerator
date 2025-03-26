using AbstractImagesGenerator.Pages;
using System.Collections.Generic;

namespace AbstractImagesGenerator.Misc
{
    public static class LayerListsController
    {
        private static readonly Blending blending = Generation.blending;

        public static void Move((int oldIndex, int newIndex, string toId, string fromId) indices)
        {
            var (oldIndex, newIndex, toId, fromId) = indices;
            var from = FindBlending(blending, fromId);
            var to = FindBlending(blending, toId);

            if (from != null && to != null)
            {
                var layer = from.SubLayers[oldIndex];
                from.SubLayers.RemoveAt(oldIndex);
                if (newIndex < to.SubLayers.Count)
                {
                    to.SubLayers.Insert(newIndex, layer);
                }
                else
                {
                    to.SubLayers.Add(layer);
                }
                layer.UpdateInheritedSettings(to.HereditarySettings);
            }
        }

        public static Blending? FindBlending(Blending blending, string id)
        {
            if (blending.Id == id)
            {
                return blending;
            }
            foreach (var layer in blending.SubLayers)
            {
                if (layer is Blending _blending)
                {
                    var result = FindBlending(_blending, id);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            return null;
        }
    }
}
