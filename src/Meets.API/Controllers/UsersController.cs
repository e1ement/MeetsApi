using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.Dto.Auth;
using Entities.Dto.User;
using Entities.Extensions;
using Entities.Models;
using Meets.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Meets.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;

        public UsersController(ILoggerManager logger, IRepositoryWrapper repository)
        {
            _logger = logger;
            _repository = repository;
        }

        /// <summary>
        /// Returns all users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _repository.User.GetAllUsersAsync();
            var mapperConfiguration = new MapperConfiguration(cfg => cfg.CreateMap<UserEntity, UserForListDto>());
            var mapper = mapperConfiguration.CreateMapper();
            var usersToReturn = mapper.Map<IEnumerable<UserEntity>, IEnumerable<UserForListDto>>(users);

            _logger.LogInfo("Returned all Users from database.");
            return Ok(usersToReturn);
        }

        /// <summary>
        /// Returns all active users
        /// </summary>
        /// <returns></returns>
        [HttpGet("active")]
        public async Task<IActionResult> GetActiveUsers()
        {
            var users = await _repository.User.GetAllActiveUsersAsync();
            var mapperConfiguration = new MapperConfiguration(cfg => cfg.CreateMap<UserEntity, UserForListDto>());
            var mapper = mapperConfiguration.CreateMapper();
            var usersToReturn = mapper.Map<IEnumerable<UserEntity>, IEnumerable<UserForListDto>>(users);

            _logger.LogInfo("Returned all Active Users from database.");
            return Ok(usersToReturn);
        }

        /// <summary>
        /// Returns all inactive users
        /// </summary>
        /// <returns></returns>
        [HttpGet("inactive")]
        public async Task<IActionResult> GetInactiveUsers()
        {
            var users = await _repository.User.GetAllInactiveUsersAsync();
            var mapperConfiguration = new MapperConfiguration(cfg => cfg.CreateMap<UserEntity, UserForListDto>());
            var mapper = mapperConfiguration.CreateMapper();
            var usersToReturn = mapper.Map<IEnumerable<UserEntity>, IEnumerable<UserForListDto>>(users);

            _logger.LogInfo("Returned all Inactive Users from database.");
            return Ok(usersToReturn);
        }

        /// <summary>
        /// Returns a specific user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "UserById")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var user = await _repository.User.GetUserByIdAsync(id);
            if (user.IsEmptyObject())
            {
                _logger.LogError($"User with id: {id}, hasn't been found in db.");
                return NotFound();
            }

            var mapperConfiguration = new MapperConfiguration(cfg => cfg.CreateMap<UserEntity, UserForDetailedDto>());
            var mapper = mapperConfiguration.CreateMapper();
            var userToReturn = mapper.Map<UserEntity, UserForDetailedDto>(user);

            _logger.LogInfo($"Returned User with id: {id}");
            return Ok(userToReturn);
        }

        /// <summary>
        /// Creates a specific user
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /users
        ///     {
        ///        "username": "username",
        ///        "name": "name",
        ///        "password": "password"
        ///     }
        ///
        /// </remarks>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody]UserForRegisterDto user)
        {

            if (user == null)
            {
                _logger.LogError("User object sent from client is null.");
                return BadRequest("User object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid User object sent from client.");
                return BadRequest("Invalid model object");
            }

            user.Username = user.Username.ToLower();
            var mapperConfiguration = new MapperConfiguration(cfg => cfg.CreateMap<UserForRegisterDto, UserEntity>());
            var mapper = mapperConfiguration.CreateMapper();
            var userToAdd = mapper.Map<UserForRegisterDto, UserEntity>(user);

            await _repository.User.CreateUserAsync(userToAdd, user.Password);

            _logger.LogInfo($"Created User with username: {user.Username}");
            return NoContent();
        }

        /// <summary>
        /// Updates a specific user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody]UserForUpdateDto user)
        {
            //This verification disable edit another user, update to add Admin role
            if (id != Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            if (user == null)
            {
                _logger.LogError("User object sent from client is null.");
                return BadRequest("User object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid User object sent from client.");
                return BadRequest("Invalid model object");
            }

            var dbUser = await _repository.User.GetUserByIdAsync(id);
            if (dbUser.IsEmptyObject())
            {
                _logger.LogError($"User with id: {id}, hasn't been found in db.");
                return NotFound();
            }

            var mapperConfiguration = new MapperConfiguration(cfg => cfg.CreateMap<UserForUpdateDto, UserEntity>());
            var mapper = mapperConfiguration.CreateMapper();
            var userToUpdate = mapper.Map<UserForUpdateDto, UserEntity>(user);

            await _repository.User.UpdateUserAsync(dbUser, userToUpdate);

            _logger.LogInfo($"Updated User with id: {id}");
            return NoContent();
        }

        /// <summary>
        /// Updates password a specific user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut("{id}/password")]
        public async Task<IActionResult> UpdateUserPassword(Guid id, [FromBody]UserForPasswordUpdateDto user)
        {
            //This verification disable edit another user, update to add Admin role
            if (id != Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            if (user == null)
            {
                _logger.LogError("User object sent from client is null.");
                return BadRequest("User object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid User object sent from client.");
                return BadRequest("Invalid model object");
            }

            var dbUser = await _repository.User.GetUserByIdAsync(id);
            if (dbUser.IsEmptyObject())
            {
                _logger.LogError($"User with id: {id}, hasn't been found in db.");
                return NotFound();
            }

            await _repository.User.UpdateUserPasswordAsync(dbUser, user.Password);

            _logger.LogInfo($"Updated User password with id: {id}");
            return NoContent();
        }

        /// <summary>
        /// Deactivates a specific user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}/deactivate")]
        public async Task<IActionResult> DeactivateUser(Guid id)
        {
            var dbUser = await _repository.User.GetUserByIdAsync(id);
            if (dbUser.IsEmptyObject())
            {
                _logger.LogError($"User with id: {id}, hasn't been found in db.");
                return NotFound();
            }

            await _repository.User.DeactivateUserAsync(dbUser);

            _logger.LogInfo($"Deactivated User with id: {id}");
            return NoContent();
        }

        /// <summary>
        /// Activates a specific user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}/activate")]
        public async Task<IActionResult> ActivateUser(Guid id)
        {
            var dbUser = await _repository.User.GetUserByIdAsync(id);
            if (dbUser.IsEmptyObject())
            {
                _logger.LogError($"User with id: {id}, hasn't been found in db.");
                return NotFound();
            }

            await _repository.User.ActivateUserAsync(dbUser);

            _logger.LogInfo($"Activated User with id: {id}");
            return NoContent();
        }

        /// <summary>
        /// Deletes a specific user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _repository.User.GetUserByIdAsync(id);
            if (user.IsEmptyObject())
            {
                _logger.LogError($"User with id: {id}, hasn't been found in db.");
                return NotFound();
            }

            await _repository.User.DeleteUserAsync(user);

            _logger.LogInfo($"Deleted User with id: {id}");
            return NoContent();
        }
    }
}