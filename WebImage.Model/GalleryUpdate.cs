using System;
using System.Collections.Generic;

namespace WebImage.Model
{
    public class GalleryUpdate
    {
        public int GalleryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Columns { get; set; }
        public string Images { get; set; }
        public bool Active { get; set; }
        public DateTime DateInsert { get; set; }
        public DateTime DateUpdate { get; set; }
        //public string UserId { get; set; }
        public List<ImageInsertUpdate> ImageAttributes { get; set; }

        public GalleryUpdate()
        {
            ImageAttributes = new List<ImageInsertUpdate>();
        }



    }
}
