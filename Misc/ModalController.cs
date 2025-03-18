using AbstractImagesGenerator.Components;
using AbstractImagesGenerator.Pages;

namespace AbstractImagesGenerator.Misc
{
    public static class ModalController
    {
        private static LayersModal Modal => Home.Modal;

        public static async Task<Layer?> ShowAsync(bool onlyBlendings)
        {
            Modal.SetLayers(await Blending.GetLayerOptions(), (onlyBlendings ? [] : await Drawing.GetLayerOptions()));
            return await Modal.ShowAsync();
        }
    }
}
