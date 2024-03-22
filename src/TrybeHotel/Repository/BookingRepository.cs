using TrybeHotel.Models;
using TrybeHotel.Dto;
using Microsoft.EntityFrameworkCore;

namespace TrybeHotel.Repository
{
    public class BookingRepository : IBookingRepository
    {
        protected readonly ITrybeHotelContext _context;
        public BookingRepository(ITrybeHotelContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // 9. Refatore o endpoint POST /booking
        public BookingResponse Add(BookingDtoInsert booking, string email)
         {
            var room = GetRoomById(booking.RoomId);

            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            var bookingEntity = new Booking
            {
                CheckIn = booking.CheckIn,
                CheckOut = booking.CheckOut,
                GuestQuant = booking.GuestQuant,
                UserId = user!.UserId,
                RoomId = room.RoomId,
            };

            _context.Bookings.Add(bookingEntity);
            _context.SaveChanges();

            var hotel = _context.Hotels.FirstOrDefault(h => h.HotelId == room.HotelId) ?? throw new Exception("Hotel not found!");
            var city = _context.Cities.FirstOrDefault(c => c.CityId == hotel.CityId);

            return new BookingResponse
            {
                BookingId = bookingEntity.BookingId,
                CheckIn = bookingEntity.CheckIn,
                CheckOut = bookingEntity.CheckOut,
                GuestQuant = bookingEntity.GuestQuant,
                Room = new RoomDto
                {
                    RoomId = room.RoomId,
                    Name = room.Name,
                    Capacity = room.Capacity,
                    Image = room.Image,
                    Hotel = new HotelDto
                    {
                        HotelId = room.Hotel!.HotelId,
                        Name = room.Hotel.Name,
                        Address = room.Hotel.Address,
                        CityId = room.Hotel.CityId,
                        CityName = city!.Name,
                        State = city.State
                    }
                }
            };
        }

        // 10. Refatore o endpoint GET /booking
        public BookingResponse GetBooking(int bookingId, string email)
        {
            var booking = _context.Bookings.FirstOrDefault(b => b.BookingId == bookingId) ?? throw new Exception("Booking not found!");
            var room = GetRoomById(booking.RoomId);

            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (booking.UserId != user.UserId) throw new UnauthorizedAccessException("User not allowed to access this booking");

            var hotel = _context.Hotels.FirstOrDefault(h => h.HotelId == room.HotelId) ?? throw new Exception("Hotel not found!");
            var city = _context.Cities.FirstOrDefault(c => c.CityId == hotel.CityId);

            return new BookingResponse
            {
                BookingId = booking.BookingId,
                CheckIn = booking.CheckIn,
                CheckOut = booking.CheckOut,
                GuestQuant = booking.GuestQuant,
                Room = new RoomDto
                {
                    RoomId = room.RoomId,
                    Name = room.Name,
                    Capacity = room.Capacity,
                    Image = room.Image,
                    Hotel = new HotelDto
                    {
                        HotelId = room.Hotel!.HotelId,
                        Name = room.Hotel.Name,
                        Address = room.Hotel.Address,
                        CityId = room.Hotel.CityId,
                        CityName = city!.Name,
                        State = city.State
                    }
                }
            };
        }

        public Room GetRoomById(int RoomId)
        {
             return _context.Rooms.FirstOrDefault(r => r.RoomId == RoomId);
        }

    }

}