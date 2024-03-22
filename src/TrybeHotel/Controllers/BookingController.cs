using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TrybeHotel.Dto;

namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("booking")]

    public class BookingController : Controller
    {
        private readonly IBookingRepository _repository;
        public BookingController(IBookingRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "Client")]
        public IActionResult Add([FromBody] BookingDtoInsert bookingInsert)
        {
            var email = User?.FindFirst(ClaimTypes.Email)?.Value;
            var bookingResponse = _repository.Add(bookingInsert, email!);

            if (bookingResponse == null)
            {
                return BadRequest(new { message = "Guest quantity over room capacity" });
            }

            return Created("", bookingResponse);
        }


        [HttpGet("{Bookingid}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "Client")]
        public IActionResult GetBooking(int Bookingid)
        {

            try
            {
                var email = User.FindFirst(ClaimTypes.Email).Value;
                var bookingResponse = _repository.GetBooking(Bookingid, email);
                return Ok(bookingResponse);
            }

            catch
            { return Unauthorized(new { message = "booking not found" }); }

        }
    }
}