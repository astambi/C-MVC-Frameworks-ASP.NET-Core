﻿namespace Prestissimo.Services.Models
{
    using AutoMapper;
    using Common.Mapping;
    using Data.Models;

    public class CartItemWithDetailsServiceModel : CartItem, IMapFrom<RecordingFormat>, IHaveCustomMapping
    {
        public string RecordingTitle { get; set; }

        public string FormatName { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            mapper
                .CreateMap<RecordingFormat, CartItemWithDetailsServiceModel>()
                .ForMember(rf => rf.RecordingTitle, cfg => cfg.MapFrom(rf => rf.Recording.Title))
                .ForMember(rf => rf.FormatName, cfg => cfg.MapFrom(rf => rf.Format.Name));
        }
    }
}
