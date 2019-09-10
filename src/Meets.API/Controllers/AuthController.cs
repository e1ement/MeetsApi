using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.Dto.Auth;
using Entities.Dto.User;
using Entities.Extensions;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Meets.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IConfiguration _configuration;

        public AuthController(ILoggerManager logger, IRepositoryWrapper repository, IConfiguration configuration)
        {
            _logger = logger;
            _repository = repository;
            _configuration = configuration;
        }

        /// <summary>
        /// Register a specific user
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /auth/register
        ///     {
        ///        "username": "Username",
        ///        "name": "name",
        ///        "password": "password"
        ///     }
        ///
        /// </remarks>
        /// <param name="userForRegisterDto"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

            if (await _repository.User.UserExists(userForRegisterDto.Username))
            {
                _logger.LogError($"User with username: {userForRegisterDto.Username}, has been found in db.");
                return BadRequest("Username already exists");
            }

            var mapperConfiguration = new MapperConfiguration(cfg => cfg.CreateMap<UserForRegisterDto, UserEntity>());
            var mapper = mapperConfiguration.CreateMapper();
            var userToCreate = mapper.Map<UserForRegisterDto, UserEntity>(userForRegisterDto);

            var createdUser = await _repository.Auth.Register(userToCreate, userForRegisterDto.Password);

            mapperConfiguration = new MapperConfiguration(cfg => cfg.CreateMap<UserEntity, UserForDetailedDto>());
            mapper = mapperConfiguration.CreateMapper();
            var user = mapper.Map<UserEntity, UserForDetailedDto>(createdUser);

            _logger.LogInfo($"Registered User with username: {userForRegisterDto.Username}");
            return CreatedAtRoute("UserById", new { controller = "Users", id = user.Id }, user);
        }

        /// <summary>
        /// Login a specific user
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /auth/login
        ///     {
        ///        "username": "Username",
        ///        "password": "password"
        ///     }
        ///
        /// </remarks>
        /// <param name="userForLoginDto"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var userFromRepo = await _repository.Auth.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);
            if (userFromRepo.IsObjectNull())
            {
                _logger.LogError($"User with username: {userForLoginDto.Username}, hasn't been found in db.");
                return Unauthorized();
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Username)
            };

            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var mapperConfiguration = new MapperConfiguration(cfg => cfg.CreateMap<UserEntity, UserForDetailedDto>());
            var mapper = mapperConfiguration.CreateMapper();
            var user = mapper.Map<UserEntity, UserForDetailedDto>(userFromRepo);

            _logger.LogInfo($"User with username: {userForLoginDto.Username} successfully login in application.");
            return Ok(new
            {
                token = tokenHandler.WriteToken(token),
                user
            });
        }
    }
}