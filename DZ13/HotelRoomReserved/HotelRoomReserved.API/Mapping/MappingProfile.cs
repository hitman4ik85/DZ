using AutoMapper;
using HotelRoomReserved.Core.DTOs;
using HotelRoomReserved.Entities.Models;

namespace HotelRoomReserved.API.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RegisterUserDTO, User>();

        CreateMap<AddRoomDTO, Room>();

        CreateMap<Room, RoomDTO>();

        CreateMap<CreateReservationDTO, Reservation>();

        CreateMap<Reservation, ReservationDTO>()
            .ForMember(dest => dest.RoomNumber, opt => opt.MapFrom(src => src.Room.RoomNumber))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name));
    }
}