using AutoMapper;
using Microsoft.EntityFrameworkCore;
using HotelRoomReserved.Core.Interfaces;
using HotelRoomReserved.Core.DTOs;
using HotelRoomReserved.Entities.Models;
using HotelRoomReserved.Storage.Database;

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
        return await _context.Rooms
            .Select(r => _mapper.Map<RoomDTO>(r))
            .ToArrayAsync(cancellationToken);
    }

    public async Task<RoomDTO> GetRoomByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var room = await _context.Rooms.FindAsync(new object[] { id }, cancellationToken);
        if (room == null)
        {
            throw new KeyNotFoundException($"Room with ID {id} not found");
        }
        return _mapper.Map<RoomDTO>(room);
    }

    public async Task<RoomDTO> AddRoomAsync(AddRoomDTO roomDto, CancellationToken cancellationToken = default)
    {
        ValidateRoom(roomDto);

        var room = _mapper.Map<Room>(roomDto);
        _context.Rooms.Add(room);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<RoomDTO>(room);
    }

    public async Task<RoomDTO> UpdateRoomAsync(int roomId, AddRoomDTO roomDto, CancellationToken cancellationToken = default)
    {
        var room = await _context.Rooms.FindAsync(new object[] { roomId }, cancellationToken);
        if (room == null)
        {
            throw new ArgumentException($"Room with ID {roomId} not found", nameof(roomId));
        }

        ValidateRoom(roomDto);

        room.RoomNumber = roomDto.RoomNumber;
        room.Type = roomDto.Type;
        room.Price = roomDto.Price;
        room.IsAvailable = roomDto.IsAvailable;

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<RoomDTO>(room);
    }

    public async Task DeleteRoomAsync(int roomId, CancellationToken cancellationToken = default)
    {
        var room = await _context.Rooms.FindAsync(new object[] { roomId }, cancellationToken);
        if (room == null)
        {
            throw new ArgumentException($"Room with ID {roomId} not found", nameof(roomId));
        }

        _context.Rooms.Remove(room);
        await _context.SaveChangesAsync(cancellationToken);
    }

    private void ValidateRoom(AddRoomDTO roomDto)
    {
        if (string.IsNullOrWhiteSpace(roomDto.RoomNumber))
        {
            throw new ArgumentException("RoomNumber must not be empty", nameof(roomDto.RoomNumber));
        }

        if (roomDto.Price <= 0)
        {
            throw new ArgumentException("Price must be greater than 0", nameof(roomDto.Price));
        }
    }
}
