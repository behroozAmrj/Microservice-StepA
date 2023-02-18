using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Terminal.ApiProducer.Models;
using Terminal.ApiProducer.Services;

namespace Terminal.ApiProducer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly ILogger<BookingController> _logger;
        private readonly IMessageProducer _messageProducer;

        public static readonly List<Booking> _booking = new();
        public BookingController(ILogger<BookingController> logger , IMessageProducer messageProducer)
        {
            this._logger = logger;
            this._messageProducer = messageProducer;
        }

        [HttpPost]
        public IActionResult CreateBooking(Booking booking)
        {
            if (booking is null)
            {
                throw new ArgumentNullException(nameof(booking));
            }

            if (!ModelState.IsValid)
                return BadRequest();

            _booking.Add(booking);
            _messageProducer.SendingMessage<Booking>(booking);

            return Ok("Successfuly added to the message broker");
        }
    }
}
