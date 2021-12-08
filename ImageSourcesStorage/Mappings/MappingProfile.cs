namespace ImageSourcesStorage.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using ImageSourcesStorage.DataAccessLayer.Models;
    using ImageSourcesStorage.Models;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
           this.CreateMap<Pin, PinsModel>().ReverseMap();
           this.CreateMap<BoardModelDetails, BoardModel>().ReverseMap();
        }
    }
}
