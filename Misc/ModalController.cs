using AbstractImagesGenerator.Components;
using AbstractImagesGenerator.Pages;
using Microsoft.AspNetCore.Components;

namespace AbstractImagesGenerator.Misc
{
    public static class ModalController
    {
        private static LayersModal Modal => Generation.LayersModal;

        public static async Task<Layer?> ShowAsync(NavigationManager NavManager, bool onlyBlendings)
        {
            Modal.SetLayers(await Blending.GetLayerOptions(NavManager), (onlyBlendings ? [] : await Drawing.GetLayerOptions(NavManager)));
            return await Modal.ShowAsync();
        }
    }
}
