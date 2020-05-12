using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WebImage.Model;

namespace WebImage.Client.Pages
{
    [Authorize]
    public class MyImagesModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public List<Image> myImages { get; set; }

        public MyImagesModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            myImages = new List<Image>();
        }

        public async Task OnGetAsync(bool isPrivate)
        {
            var httpClient = _httpClientFactory.CreateClient("APIClient");

            string public_or_private = (isPrivate == true) ? "private" : "public";

            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/images/{public_or_private}");
            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            using (var responseStream = await response.Content.ReadAsStreamAsync())
            {
                myImages = await JsonSerializer.DeserializeAsync<List<Image>>(responseStream, options);
            }
        }
    }
}
