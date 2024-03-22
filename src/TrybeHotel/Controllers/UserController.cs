using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using TrybeHotel.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("user")]

    public class UserController : Controller
    {
        private readonly IUserRepository _repository;
        public UserController(IUserRepository repository)
        {
            _repository = repository;
        }
        
        [HttpGet]
        public IActionResult GetUsers(){
            var users = _repository.GetUsers();
            return Ok(users);
        }

        [HttpPost]
        public IActionResult Add([FromBody] UserDtoInsert user)
        {
            try
            {
             var verifyEmail = _repository.GetUserByEmail(user.Email!);

            if (verifyEmail != null ) throw new Exception("User email already exists");
            
                var newUser = _repository.Add(user);
                return Created("", newUser);
            }
            catch (Exception e){
                return StatusCode(409, new {
                    message = e.Message
                });
            }
        }
    }
}