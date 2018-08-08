using AutoMapper;
using TravelPlanner.DomainModel;
using TravelPlanner.Web.Models;

namespace TravelPlanner.Web.Infrastructure
{
    public static class AutoMapperConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(c =>
            {
                c.CreateMap<Trip, TripModel>().ReverseMap();
                c.CreateMap<TripRoute, TripRouteModel>()
                .ForMember(dest => dest.Time, opt => opt.Ignore());
                c.CreateMap<TripRouteModel, TripRoute>()
                    .ForMember(dest => dest.Trip, opt => opt.Ignore());
                c.CreateMap<TripWaypoint, TripWaypointModel>().ReverseMap();
                c.CreateMap<Trip, TripDetailModel>().ReverseMap();
                c.CreateMap<User, UserModel>().ReverseMap();
                c.CreateMap<Car, CarModel>().ReverseMap();

                c.CreateMap<Message, MessageModel>().ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.User.UserName));
                c.CreateMap<MessageModel, Message>()
                    .ForMember(dest => dest.User, opt => opt.Ignore())
                    .ForMember(dest => dest.Chat, opt => opt.Ignore());
            });

            //Mapper.AssertConfigurationIsValid();
        }
    }
}
