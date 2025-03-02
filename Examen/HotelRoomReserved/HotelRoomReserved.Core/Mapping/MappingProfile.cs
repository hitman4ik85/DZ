using AutoMapper;
using HotelRoomReserved.Core.DTOs;
using HotelRoomReserved.Entities.Models;

namespace HotelRoomReserved.Core.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User мапінг
        CreateMap<RegisterUserDTO, User>();
        CreateMap<User, UserDTO>();

        // Room мапінг
        CreateMap<AddRoomDTO, Room>();
        CreateMap<Room, RoomDTO>();

        // Reservation мапінг
        CreateMap<CreateReservationDTO, Reservation>();

        CreateMap<Reservation, ReservationDTO>()
            .ForMember(dest => dest.RoomNumber, opt => opt.MapFrom(src => src.Room.RoomNumber))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
    }
}
