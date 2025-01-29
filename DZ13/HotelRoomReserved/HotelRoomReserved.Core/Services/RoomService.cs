using AutoMapper;
using Microsoft.EntityFrameworkCore;
using HotelRoomReserved.Core.DTOs;
using HotelRoomReserved.Core.Interfaces;
using HotelRoomReserved.Entities.Models;
using HotelRoomReserved.Storage;

namespace HotelRoomReserved.Core.Services;

public class RoomService : IRoomService
{
    private readonly HotelContext _context;
    private readonly IMapper _mapper;

    public RoomService(HotelContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RoomDTO>> GetRoomsAsync(CancellationToken cancellationToken = default)
    {
        var rooms = await _context.Rooms.ToListAsync(cancellationToken);
        return _mapper.Map<IEnumerable<RoomDTO>>(rooms);
    }

    public async Task<RoomDTO> AddRoomAsync(AddRoomDTO roomDto, CancellationToken cancellationToken = default)
    {
        ValidateRoom(roomDto);

        var room = _mapper.Map<Room>(roomDto);
        room.IsAvailable = true;

        _context.Rooms.Add(room);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<RoomDTO>(room);
    }

    private void ValidateRoom(AddRoomDTO roomDto)
    {
        if (string.IsNullOrEmpty(roomDto.RoomNumber))
        {
            throw new ArgumentException("Room number must have a value", nameof(roomDto.RoomNumber));
        }

        if (string.IsNullOrEmpty(roomDto.Type))
        {
            throw new ArgumentException("Room type must have a value", nameof(roomDto.Type));
        }

        if (roomDto.Price <= 0)
        {
            throw new ArgumentException("Price must be greater than 0", nameof(roomDto.Price));
        }
    }
}
