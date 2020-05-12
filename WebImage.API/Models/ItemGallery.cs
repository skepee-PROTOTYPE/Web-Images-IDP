using System.Collections.Generic;
using WebImage.API.Context;

namespace WebImage.API.Models
{
    public class ItemGallery
    {
        public IjpGallery Gallery { get; set; }
        public List<IjpGalleryFile> GalleryFile { get; set; }

        public ItemGallery()
        {
            Gallery = new IjpGallery();
            GalleryFile = new List<IjpGalleryFile>();
        }

        public void NewGallery(string name, string description, string images, string attr)
        {
            Gallery.Name = name;
            Gallery.Description = description;
            Gallery.Images = images;
            Gallery.Columns = attr;
            Gallery.Active = false;
        }
    }
}
