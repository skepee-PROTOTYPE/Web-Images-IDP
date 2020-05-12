using System;
using System.Collections.Generic;
using System.Linq;
using WebImage.API.Context;
using WebImage.Model;

namespace WebImage.API.Services
{
    public class IjpRepository : IIjpRepository
    {
        private readonly IjpContext _ijpContext;

        public IjpRepository(IjpContext ijpContext)
        {
            _ijpContext = ijpContext;
        }

        public void AddGallery(IjpGallery gallery)
        {
            _ijpContext.Gallery.Add(gallery);
        }

        public IjpGallery GetGallery(int galleryId, string ownerId)
        {
            var res = _ijpContext.Gallery.FirstOrDefault(x => x.UserId == ownerId & x.GalleryId == galleryId);
            return res;
        }

        public List<IjpGallery> GetGalleries(string ownerId)
        {
            var res = _ijpContext.Gallery.Where(x => x.UserId == ownerId).ToList();
            return res;
        }

        public List<IjpFile> GetImages(bool isprivate, string ownerId)
        {
            var res = _ijpContext.File.Where(x => x.UserId == ownerId & x.IsPrivate == isprivate).ToList();
            return res;
        }

        public bool Save()
        {
            return (_ijpContext.SaveChanges() >= 0);
        }

        public List<IjpGalleryFile> GetImageAttributes(int GalleryId)
        {
            var res = _ijpContext.GalleryFile.Where(x => x.GalleryId == GalleryId).ToList();
            return res;
        }

        public IjpFile GetImage(int imageId)
        {
            var res = _ijpContext.File.FirstOrDefault(x => x.FileId == imageId);
            return res;
        }

        public void RemoveGallery(int galleryId)
        {
            var res = _ijpContext.Gallery.FirstOrDefault(x => x.GalleryId == galleryId);
            _ijpContext.Gallery.Remove(res);
        }

        public void UpdateGallery(IjpGallery gallery)
        {
            var res = _ijpContext.Gallery.FirstOrDefault(x => x.GalleryId == gallery.GalleryId);

            res.Active = gallery.Active;
            res.Columns = gallery.Columns;
            res.DateUpdate = DateTime.Now;
            res.Description = gallery.Description;
            res.Images = gallery.Columns;
            res.Name = gallery.Name;

            _ijpContext.Gallery.Update(res);
        }


        public void RemoveImageAttributes(int galleryId)
        {
            var res = _ijpContext.GalleryFile.Where(x => x.GalleryId == galleryId);
            _ijpContext.GalleryFile.RemoveRange(res);
        }

        public void UpdateImageAttribute(IjpGalleryFile imageDescription)
        {
            var res = _ijpContext.GalleryFile.FirstOrDefault(x => x.FileId == imageDescription.FileId);

            res.Description = imageDescription.Description;

            _ijpContext.GalleryFile.Update(res);

        }

        public IjpGalleryFile GetImageAttribute(int imageId)
        {
            var res = _ijpContext.GalleryFile.FirstOrDefault(x => x.FileId == imageId);
            return res;
        }
    }
}
