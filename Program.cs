using AbstractImagesGenerator.Misc;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace AbstractImagesGenerator
{
    public class Program
    {
        public static string BaseApiUrl(NavigationManager nav) => nav.BaseUri[..nav.BaseUri.LastIndexOf(':')] + ":8000/api";
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddSingleton<LikesService>();

            await builder.Build().RunAsync();
        }
    }
}
