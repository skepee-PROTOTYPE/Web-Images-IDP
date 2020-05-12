﻿using System.Linq;
using WebImage.API.DBContext;

namespace WebImage.API.Models
{
    public class MyContainer
    {
        private readonly IjpContext ijpContext;
        public MyGalleries myGalleries { get; set; }
        public MyImages myImages { get; set; }

        public MyContainer(IjpContext _ijpContext, string userId)
        {
            ijpContext = _ijpContext;
            myGalleries = new MyGalleries(ijpContext, userId);
            myImages = new MyImages(ijpContext, userId);
        }

        public MyGalleries GetMyGalleries(int imageId, string userId)
        {
            var myout = new MyGalleries();
            var res = new MyGalleries(ijpContext, userId);

            foreach (var gall in res.ItemGalleries)
            {
                var myItem = gall.GalleryFile.FirstOrDefault(x => x.FileId == imageId);

                if (myItem != null)
                {
                    myout.ItemGalleries.Add(res.ItemGalleries.FirstOrDefault(x => x.Gallery.GalleryId == myItem.GalleryId));
                }
            }

            return myout;
        }


        public JsonData GetFileInfoJson(int galleryId)
        {
            JsonData myData = new JsonData();

            var itemGallery = this.myGalleries.ItemGalleries.FirstOrDefault(x => x.Gallery.GalleryId == galleryId);

            string hidecolumn = Helper.Decode(itemGallery.Gallery.Columns);

            foreach (var imageGallery in itemGallery.GalleryFile)
            {
                var image = myImages.Images.FirstOrDefault(x => x.FileId == imageGallery.FileId);

                if (image != null)
                {
                    myData.MyJson.Add(new MyJson()
                    {
                        Url = hidecolumn.Contains("url") ? "" : image.Url,
                        UrlResized = hidecolumn.Contains("url") ? "" : image.UrlResized,
                        UrlThumb = hidecolumn.Contains("url") ? "" : image.UrlThumb,
                        Name = hidecolumn.Contains("name") ? "" : image.Name,
                        Title = hidecolumn.Contains("title") ? "" : image.Title,
                        Format = hidecolumn.Contains("format") ? "" : image.RawFormat,
                        Length = hidecolumn.Contains("length") ? 0 : (double)image.LengthKB,
                        IsLandscape = (hidecolumn.Contains("landscape") ? null : image.IsLandscape.ToString()),
                        PixelFormat = hidecolumn.Contains("pixel") ? "" : image.PixelFormat,
                        Size = hidecolumn.Contains("size") ? "" : image.Width.ToString() + " X " + image.Height.ToString(),
                        Resolution = hidecolumn.Contains("resolution") ? "" : image.HorizontalResolution + " X " + image.VerticalResolution,
                        Content = hidecolumn.Contains("content") ? null : image.Content
                    });
                }
            }
            return myData;
        }


    }
}
