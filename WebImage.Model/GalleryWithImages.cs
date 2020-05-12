using System.Collections.Generic;

namespace WebImage.Model
{
    public class GalleryWithImages
    {
        public Gallery Gallery { get; set; }
        public List<Image> Images { get; set; }

        public GalleryWithImages()
        {
            Images = new List<Image>();
        }
    }
}
