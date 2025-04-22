using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System.Text;

namespace AbstractImagesGenerator.Misc
{
    public class LikesService(IJSRuntime js, NavigationManager navManager)
    {
        private readonly IJSRuntime JS = js;
        private readonly NavigationManager NavManager = navManager;

        public async Task<bool> IsLiked(string id)
        {
            var likes = await JS.InvokeAsync<string>("localStorage.getItem", "likes");
            return JsonConvert.DeserializeObject<List<string>>(likes ?? "[]")?.Contains(id) ?? false;
        }

        public async Task<bool> Like(string id)
        {
            var likes = await MyLikes();
            if (!likes.Contains(id))
            {
                likes.Add(id);
                var response = await new HttpClient().SendAsync(new HttpRequestMessage(HttpMethod.Put, $"{Program.BaseApiUrl(NavManager)}/like/query-id/{id}"));
                if (response.IsSuccessStatusCode)
                {
                    await JS.InvokeVoidAsync("localStorage.setItem", "likes", JsonConvert.SerializeObject(likes));
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> LikeRandom(string query)
        {
           var response = await new HttpClient().PutAsync( 
                $"{Program.BaseApiUrl(NavManager)}/like/query-full?full_metadata=false",
                new StringContent(query, Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                var id = JsonConvert.DeserializeObject<Dictionary<string, string>>(await response.Content.ReadAsStringAsync())?["query_id"];
                var likes = await MyLikes();
                likes.Add(id);
                await JS.InvokeVoidAsync("localStorage.setItem", "likes", JsonConvert.SerializeObject(likes));
                return true;
            }
            return false;
        }

        public async Task<bool> UnLikeRandom(string query)
        {
            var response = await new HttpClient().PutAsync(
                $"{Program.BaseApiUrl(NavManager)}/dislike/query-full?full_metadata=false",
                new StringContent(query, Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                var id = JsonConvert.DeserializeObject<Dictionary<string, string>>(await response.Content.ReadAsStringAsync())?["query_id"];
                var likes = await MyLikes();
                likes.Remove(id);
                await JS.InvokeVoidAsync("localStorage.setItem", "likes", JsonConvert.SerializeObject(likes));
                return true;
            }
            return false;
        }

        public async Task<bool> UnLike(string id)
        {
            var likes = await MyLikes();
            if (likes.Remove(id))
            {
                var response = await new HttpClient().SendAsync(new HttpRequestMessage(HttpMethod.Put, $"{Program.BaseApiUrl(NavManager)}/dislike/query-id/{id}"));
                await SetLikes(likes);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<List<string>> MyLikes()
        {
            return JsonConvert.DeserializeObject<List<string>>(await JS.InvokeAsync<string>("localStorage.getItem", "likes") ?? "[]") ?? [];
        }

        public async Task<List<(string image, int likes, string qId, string query)>> GetPopular(int page)
        {
            var response = await new HttpClient().SendAsync(new HttpRequestMessage(HttpMethod.Get, $"{Program.BaseApiUrl(NavManager)}/popular/images?page={page}&page_size=20&include_image=true&include_metadata=true&full_metadata=true"));
            if (response.IsSuccessStatusCode)
            {
                var parser = HttpMultipartParser.MultipartFormDataParser.Parse(await response.Content.ReadAsStreamAsync());
                List<(string image, int likes, string qId, string query)> result = [];
                for (int i = 0; i < parser.Files.Count; i += 2)
                {
                    Stream imageStream = parser.Files[i].Data;
                    Stream jsonStream = parser.Files[i + 1].Data;

                    var json = new StreamReader(jsonStream).ReadToEnd();
                    QueryObject queryObject = QueryObject.Deserialize(json);
                    
                    result.Add(($"data:image/jpg;base64,{Convert.ToBase64String(imageStream.ReadFully())}", 
                        queryObject.Likes,
                        queryObject.Id,
                        json));
                }
                return result;
            }
            else
            {
                return [];
            }
        }

        private async Task SetLikes(List<string> likes)
        {
            await JS.InvokeVoidAsync("localStorage.setItem", "likes", JsonConvert.SerializeObject(likes));
        }
    }
}
