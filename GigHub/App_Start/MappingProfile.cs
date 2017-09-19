using AutoMapper;
using GigHub.Dtos;
using GigHub.Models;

namespace GigHub
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<ApplicationUser, UserDto>();
            CreateMap<Gig, GigDto>();
            CreateMap<Notification, NotificationDto>();

        }
    }
}