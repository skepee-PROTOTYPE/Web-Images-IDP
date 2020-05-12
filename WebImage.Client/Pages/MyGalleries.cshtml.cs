using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebImage.Model;

namespace WebImage.Client.Pages
{
    [Authorize]
    public class MyGalleriesModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public int GalleryIdSelected {get;set;}
        public List<GalleryWithImages> MyGalleries { get; set; }

        public MyGalleriesModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public string AttributeChecked(string columns, string columnName)
        {
            string columnsList = Helper.Decode(columns);
            var isChecked = columnsList.Contains(columnName) ? "" : "checked";
            return isChecked;
        }

        public async Task OnGetAsync(int galleryId)
        {
            var httpClient = _httpClientFactory.CreateClient("APIClient");
            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/gallery");
            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            using (var responseStream = await response.Content.ReadAsStreamAsync())
            {
                MyGalleries = await JsonSerializer.DeserializeAsync<List<GalleryWithImages>>(responseStream, options);
            }
            GalleryIdSelected = galleryId;
        }

        public async Task<RedirectToPageResult> OnPostSaveGallery()
        {
            GalleryUpdate galleryUpdate = new GalleryUpdate();

            galleryUpdate.Columns = Helper.Encode(GetRequestParam("attrs"));
            galleryUpdate.Description = GetRequestParam("inputgallerydescription");
            galleryUpdate.Name = GetRequestParam("inputgalleryname");
            galleryUpdate.GalleryId = Convert.ToInt32(GetRequestParam("galleryid"));
            galleryUpdate.Active = GetRequestParam("toggleActive") == "on" ? true : false;
            galleryUpdate.Images = Helper.Encode(GetRequestParam("images"));

            string description_ids = GetRequestParam("descriptions");

            foreach (var item in description_ids.Split(","))
            {
                galleryUpdate.ImageAttributes.Add(new ImageInsertUpdate()
                {
                    ImageId = Convert.ToInt32(item.Split('|')[1]),
                    Description= item.Split('|')[0]
                });
            }

            var serializedGalleryForUpdate = JsonSerializer.Serialize(galleryUpdate);

            var httpClient = _httpClientFactory.CreateClient("APIClient");
            var request = new HttpRequestMessage(HttpMethod.Put, $"/api/gallery");
            
            request.Content = new StringContent(serializedGalleryForUpdate, System.Text.Encoding.Unicode,"application/json");

            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();
            return new RedirectToPageResult("MyGalleries");
        }


        public async Task<RedirectToPageResult> OnPostRemoveGallery(int galleryId)
        {
            var httpClient = _httpClientFactory.CreateClient("APIClient");
            var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/gallery/{galleryId}");
            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();
            return new RedirectToPageResult("MyGalleries");
        }

        private string GetRequestParam(string param)
        {
            if (Request.Form[param].Count == 1)
                return Request.Form[param][0];
            else
                return string.Empty;
        }

    }
}