using AutoMapper;
using Microsoft.EntityFrameworkCore;
using HotelRoomReserved.Core.DTOs;
using HotelRoomReserved.Core.Interfaces;
using HotelRoomReserved.Entities.Models;
using HotelRoomReserved.Storage;

namespace HotelRoomReserved.Core.Services;

public class ReservationService : IReservationService
{
    private readonly HotelContext _context;
    private readonly IMapper _mapper;

    public ReservationService(HotelContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ReservationDTO>> GetUserReservationsAsync(int userId, CancellationToken cancellationToken = default)
    {
        var reservations = await _context.Reservations
            .Include(r => r.Room)
            .Include(r => r.User)
            .Where(r => r.UserId == userId)
            .ToListAsync(cancellationToken);

        return _mapper.Map<IEnumerable<ReservationDTO>>(reservations);
    }

    public async Task<ReservationDTO> CreateReservationAsync(CreateReservationDTO reservationDto, CancellationToken cancellationToken = default)
    {
        ValidateReservation(reservationDto);

        var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == reservationDto.RoomId, cancellationToken);
        if (room == null || !room.IsAvailable)
        {
            throw new ArgumentException("Room is not available for reservation.");
        }

        var reservation = _mapper.Map<Reservation>(reservationDto);
        reservation.Status = ReservationStatus.Confirmed;

        _context.Reservations.Add(reservation);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ReservationDTO>(reservation);
    }

    public async Task CancelReservationAsync(int reservationId, int userId, CancellationToken cancellationToken = default)
    {
        var reservation = await _context.Reservations
            .FirstOrDefaultAsync(r => r.Id == reservationId && r.UserId == userId, cancellationToken);

        if (reservation == null)
        {
            throw new KeyNotFoundException($"Reservation with ID {reservationId} not found or not owned by user.");
        }

        reservation.Status = ReservationStatus.Cancelled;
        await _context.SaveChangesAsync(cancellationToken);
    }

    private void ValidateReservation(CreateReservationDTO reservationDto)
    {
        if (reservationDto.RoomId <= 0)
        {
            throw new ArgumentException("Invalid Room ID", nameof(reservationDto.RoomId));
        }

        if (reservationDto.CheckInDate >= reservationDto.CheckOutDate)
        {
            throw new ArgumentException("Check-in date must be before check-out date.");
        }
    }
}
