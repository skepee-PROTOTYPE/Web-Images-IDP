using System.Collections.Generic;
using WebImage.API.Context;

namespace WebImage.API.Services
{
    public interface IIjpRepository
    {
        List<IjpFile> GetImages(bool isPrivate, string ownerId);
        void AddGallery(IjpGallery gallery);
        IjpGallery GetGallery(int galleryId, string ownerId);
        List<IjpGallery> GetGalleries(string ownerId);
        List<IjpGalleryFile> GetImageAttributes(int GalleryId);
        IjpFile GetImage(int imageId);
        void RemoveGallery(int galleryId);
        void RemoveImageAttributes(int galleryId);
        public void UpdateGallery(IjpGallery gallery);

        IjpGalleryFile GetImageAttribute(int imageId);

        public void UpdateImageAttribute(IjpGalleryFile imageDescription);
        bool Save();
    }
}
