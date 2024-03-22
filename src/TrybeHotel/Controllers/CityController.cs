using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;

namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("city")]
    public class CityController : Controller
    {
        private readonly ICityRepository _repository;
        public CityController(ICityRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult GetCities()
        {
            return Ok(_repository.GetCities().ToList());
        }

        [HttpPost]
        public IActionResult PostCity([FromBody] City city)
        {
            return Created("", _repository.AddCity(city));
        }

        // 3. Desenvolva o endpoint PUT /city
        [HttpPut]
        public IActionResult PutCity([FromBody] City city)
        {
            var cityDto = _repository.UpdateCity(city);
            return Ok(cityDto);
        }
    }
}