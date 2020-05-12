using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyAzure;
using WebImage.API.Context;
using WebImage.API.Services;
using WebImage.Model;

namespace WebImage.API.Controllers
{
    [Authorize]
    [Route("api/gallery")]
    [ApiController]
    public class GalleryController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IIjpRepository _ijpRepository;

        public GalleryController(IMapper mapper, IIjpRepository ijpRepository)
        {
            _mapper = mapper;
            _ijpRepository = ijpRepository;
        }


        [HttpPost()]
        public IActionResult CreateGallery([FromBody] Gallery gallery)
        {
            var galleryEntity = _mapper.Map<Context.IjpGallery>(gallery);

            var userId = User.Claims.FirstOrDefault(c => c.Type == "sub").Value;
            galleryEntity.UserId = userId;
            _ijpRepository.AddGallery(galleryEntity);
            _ijpRepository.Save();

            var savedGallery = _mapper.Map<Gallery>(galleryEntity);
            return Ok(savedGallery);
        }


        [HttpDelete("{galleryId}")]
        public IActionResult RemoveGallery(int galleryId)
        {
            using TransactionScope scope = new TransactionScope();

            _ijpRepository.RemoveImageAttributes(galleryId);
            _ijpRepository.Save();

            _ijpRepository.RemoveGallery(galleryId);
            _ijpRepository.Save();

            scope.Complete();
            return NoContent();
        }


        [HttpPut()]
        public IActionResult UpdateGallery([FromBody] GalleryUpdate gallery)
        {
            using TransactionScope scope = new TransactionScope();

            foreach (var itemimage in gallery.ImageAttributes)
            {
                var imageAttribute = _ijpRepository.GetImageAttribute(itemimage.ImageId);
                imageAttribute.Description = itemimage.Description;

                _ijpRepository.UpdateImageAttribute(imageAttribute);
            }
            _ijpRepository.Save();

            IjpGallery galleryupdate = new IjpGallery()
            {
                Active = gallery.Active,
                Columns = gallery.Columns,
                Description = gallery.Description,
                GalleryId = gallery.GalleryId,
                Images = gallery.Images,
                Name = gallery.Name
            };

            var galleryEntity = _mapper.Map<Context.IjpGallery>(galleryupdate);

            _ijpRepository.UpdateGallery(galleryEntity);
            _ijpRepository.Save();

            scope.Complete();

            return NoContent();
        }


        [HttpGet()]
        public async Task<IActionResult> GetGalleries()
        {
            List<GalleryWithImages> myGallery = new List<GalleryWithImages>();
            var ownerId = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var myGalleriesRepository = _ijpRepository.GetGalleries(ownerId);
            var galleries = _mapper.Map<IEnumerable<Gallery>>(myGalleriesRepository);

            foreach (Gallery itemGallery in galleries)
            {
                List<Image> mylistimages = new List<Image>();
                var imageDescriptions = _ijpRepository.GetImageAttributes(itemGallery.GalleryId);

                foreach (IjpGalleryFile descr in imageDescriptions)
                {
                    var image = _ijpRepository.GetImage(descr.FileId);
                    var fullimage = _mapper.Map<Model.Image>(image);
                    fullimage.Content = await AzureStorage.DownloadFileFromBlob(fullimage.Name, ownerId);
                    fullimage.Description = descr.Description;
                    fullimage.Url = await AzureStorage.BlobUrlAsync(ownerId, fullimage.Name);
                    fullimage.UrlResized = await AzureStorage.BlobUrlAsync(ownerId, fullimage.ResizedFileName);
                    fullimage.UrlThumb = await AzureStorage.BlobUrlAsync(ownerId, fullimage.ThumbFileName);

                    // check which attributes will be exposed
                    fullimage=fullimage.HideAttributes(Helper.Decode(itemGallery.Columns));

                    mylistimages.Add(fullimage);
                }

                myGallery.Add(new GalleryWithImages()
                {
                    Gallery = itemGallery,
                    Images = mylistimages,
                });
            }
            return Ok(myGallery);
        }

        [HttpGet("galleryId")]
        [Authorize(Policy = "CanOrderFrame")]
        public async Task<IActionResult> GetGallery(int galleryId)
        {
            List<GalleryWithImages> myGallery = new List<GalleryWithImages>();
            var ownerId = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var myGalleryRepository = _ijpRepository.GetGalleries(ownerId).SingleOrDefault(x => x.GalleryId == galleryId);
            var itemGallery = _mapper.Map<Gallery>(myGalleryRepository);

            List<Image> mylistimages = new List<Image>();
            var imageDescriptions = _ijpRepository.GetImageAttributes(itemGallery.GalleryId);

            foreach (IjpGalleryFile descr in imageDescriptions)
            {
                var image = _ijpRepository.GetImage(descr.FileId);
                var fullimage = _mapper.Map<Model.Image>(image);
                fullimage.Content = await AzureStorage.DownloadFileFromBlob(fullimage.ThumbFileName, ownerId);
                fullimage.Description = descr.Description;
                fullimage.Url = await AzureStorage.BlobUrlAsync(ownerId, fullimage.Name);
                fullimage.UrlResized = await AzureStorage.BlobUrlAsync(ownerId, fullimage.ResizedFileName);
                fullimage.UrlThumb = await AzureStorage.BlobUrlAsync(ownerId, fullimage.ThumbFileName);

                // check which attributes will be exposed
                fullimage = fullimage.HideAttributes(Helper.Decode(itemGallery.Columns));

                mylistimages.Add(fullimage);
            }

            myGallery.Add(new GalleryWithImages()
            {
                Gallery = itemGallery,
                Images = mylistimages,
            });

            return Ok(myGallery);
        }


    }
}