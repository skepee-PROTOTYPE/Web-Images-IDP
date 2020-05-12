using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebImage.API.Profiles
{
    public class ImageProfile : Profile
    {

        public ImageProfile()
        {
            CreateMap<Context.IjpFile, Model.Image>()
           .ForMember(
                dest => dest.ImageId,
                opt => opt.MapFrom(src => src.FileId)
            );//.ReverseMap();
        }



    }
}
