using System;
using System.Threading.Tasks;
using Contracts;
using Entities.Extensions;
using Entities.Models;
using Meets.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Meets.API.Controllers
{
    [Authorize]
    [Route("api/values")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;

        public ValuesController(ILoggerManager logger, IRepositoryWrapper repository)
        {
            _logger = logger;
            _repository = repository;
        }

        /// <summary>
        /// Returns all values
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetValues()
        {
            var values = await _repository.Value.GetAllValuesAsync();

            _logger.LogInfo("Returned all Values from database.");
            return Ok(values);
        }

        /// <summary>
        /// Returns a specific value
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ServiceFilter(typeof(LogUserActivity))]
        [HttpGet("{id}", Name = "ValueById")]
        public async Task<IActionResult> GetValue(Guid id)
        {
            var value = await _repository.Value.GetValueByIdAsync(id);
            if (value.IsEmptyObject())
            {
                _logger.LogError($"Value with id: {id}, hasn't been found in db.");
                return NotFound();
            }

            _logger.LogInfo($"Returned Value with id: {id}");
            return Ok(value);
        }

        /// <summary>
        /// Creates a specific value
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /values
        ///     {
        ///        "name": "Item1"
        ///     }
        ///
        /// </remarks>
        /// <param name="value"></param>
        /// <returns></returns>
        [ServiceFilter(typeof(LogUserActivity))]
        [HttpPost]
        public async Task<IActionResult> CreateValue([FromBody]ValueEntity value)
        {
            if (value.IsObjectNull())
            {
                _logger.LogError("Value object sent from client is null.");
                return BadRequest("Value object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid Value object sent from client.");
                return BadRequest("Invalid model object");
            }

            await _repository.Value.CreateValueAsync(value);

            return CreatedAtRoute("ValueById", new { id = value.Id }, value);
        }

        /// <summary>
        /// Updates a specific value
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [ServiceFilter(typeof(LogUserActivity))]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateValue(Guid id, [FromBody]ValueEntity value)
        {
            if (value.IsObjectNull())
            {
                _logger.LogError("Value object sent from client is null.");
                return BadRequest("Value object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid Value object sent from client.");
                return BadRequest("Invalid model object");
            }

            var dbValue = await _repository.Value.GetValueByIdAsync(id);
            if (dbValue.IsEmptyObject())
            {
                _logger.LogError($"Value with id: {id}, hasn't been found in db.");
                return NotFound();
            }

            await _repository.Value.UpdateValueAsync(dbValue, value);

            return NoContent();
        }

        /// <summary>
        /// Deletes a specific value
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ServiceFilter(typeof(LogUserActivity))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteValue(Guid id)
        {
            var value = await _repository.Value.GetValueByIdAsync(id);
            if (value.IsEmptyObject())
            {
                _logger.LogError($"Value with id: {id}, hasn't been found in db.");
                return NotFound();
            }

            await _repository.Value.DeleteValueAsync(value);

            return NoContent();
        }
    }
}
