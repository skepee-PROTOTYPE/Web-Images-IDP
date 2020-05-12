using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebImage.API.Services;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using AutoMapper;

namespace WebImage.API.Controllers
{
    [Authorize]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IIjpRepository _ijpRepository;
        private readonly IMapper _mapper;

        public ImageController(IIjpRepository ijpRepository, IMapper mapper)
        {
            _ijpRepository = ijpRepository;
            _mapper = mapper;
        }


        [Route("/api/images/{public_or_private}")]
        [HttpGet()]
        public IActionResult GetImages(string public_or_private)
        {
            var ownerId = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var myImagesRepository = _ijpRepository.GetImages((public_or_private == "private") ? true : false, ownerId);
            var imagesToReturn = _mapper.Map<IEnumerable<Model.Image>>(myImagesRepository);
            return Ok(imagesToReturn);            
        }



        ////[EnableCors()]
        //[HttpGet("/api/gallery/{id}/user/{userId}")]
        //public async Task<JsonResult> GetListSelection(int id, string userId)
        //{
        //    //    if (id > 0)
        //    //    {
        //    //        DateTime StartDate = DateTime.Now;
        //    //        var mycontainer = new MyContainer(ijpContext, userId);
        //    //        var mygallery = mycontainer.myGalleries.ItemGalleries.Where(x => x.Gallery.GalleryId == id).ToList();

        //    //        if (mygallery.Count() == 1 && mygallery[0].Gallery.Active)
        //    //        {
        //    //            var mydata = mycontainer.GetFileInfoJson(id);

        //    //            return Json(new JsonGallery()
        //    //            {
        //    //                Data = mydata,

        //    //                Stat = new JsonStats()
        //    //                {
        //    //                    Count = mydata.MyJson.Count(),
        //    //                    TotalLengthKb = mydata.MyJson.Select(x => x.Length).Sum(),
        //    //                    ElapsedTime = (DateTime.Now - StartDate).TotalMilliseconds
        //    //                }
        //    //            });
        //    //        }
        //    //        else
        //    //            return Json(new JsonGallery());
        //    //    }
        //    //    else
        //    //        return Json(new JsonGallery());
        //    //}

        //    return null;
        //}

    }
}
