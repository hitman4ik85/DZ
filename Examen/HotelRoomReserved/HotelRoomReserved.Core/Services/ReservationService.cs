using AutoMapper;
using Microsoft.EntityFrameworkCore;
using HotelRoomReserved.Core.Interfaces;
using HotelRoomReserved.Core.DTOs;
using HotelRoomReserved.Entities.Models;
using HotelRoomReserved.Storage.Database;

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
        return await _context.Reservations
            .Where(r => r.UserId == userId)
            .Include(r => r.Room)
            .Select(r => _mapper.Map<ReservationDTO>(r))
            .ToArrayAsync(cancellationToken);
    }

    public async Task<IEnumerable<ReservationDTO>> GetAllReservationsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Reservations
            .Include(r => r.Room)
            .Include(r => r.User)
            .Select(r => _mapper.Map<ReservationDTO>(r))
            .ToArrayAsync(cancellationToken);
    }

    public async Task<ReservationDTO> CreateReservationAsync(CreateReservationDTO reservationDto, int userId, CancellationToken cancellationToken = default)
    {
        ValidateReservationDates(reservationDto.CheckInDate, reservationDto.CheckOutDate);

        var room = await _context.Rooms.FindAsync(new object[] { reservationDto.RoomId }, cancellationToken);
        if (room == null || !room.IsAvailable)
        {
            throw new ArgumentException("Room is not available or does not exist", nameof(reservationDto.RoomId));
        }

        var reservation = new Reservation
        {
            RoomId = reservationDto.RoomId,
            UserId = userId,
            CheckInDate = reservationDto.CheckInDate,
            CheckOutDate = reservationDto.CheckOutDate,
            Status = ReservationStatus.Confirmed
        };

        _context.Reservations.Add(reservation);
        room.IsAvailable = false; // Бронюємо кімнату
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ReservationDTO>(reservation);
    }

    public async Task CancelReservationAsync(int reservationId, int userId, CancellationToken cancellationToken = default)
    {
        var reservation = await _context.Reservations
            .Include(r => r.Room)
            .FirstOrDefaultAsync(r => r.Id == reservationId && r.UserId == userId, cancellationToken);

        if (reservation == null)
        {
            throw new ArgumentException("Reservation not found or does not belong to user", nameof(reservationId));
        }

        reservation.Status = ReservationStatus.Cancelled;
        reservation.Room.IsAvailable = true; // Робимо кімнату доступною знову
        await _context.SaveChangesAsync(cancellationToken);
    }

    private void ValidateReservationDates(DateTime checkIn, DateTime checkOut)
    {
        if (checkIn >= checkOut)
        {
            throw new ArgumentException("Check-in date must be before check-out date");
        }

        if (checkIn < DateTime.UtcNow)
        {
            throw new ArgumentException("Check-in date cannot be in the past");
        }
    }
}
