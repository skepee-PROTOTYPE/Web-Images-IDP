using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebImage.API.Profiles
{
    public class GalleryProfile: Profile
    {

        public GalleryProfile()
        {
            CreateMap<Context.IjpGallery, Model.Gallery>();
        
        
        
        
        }



    }
}
