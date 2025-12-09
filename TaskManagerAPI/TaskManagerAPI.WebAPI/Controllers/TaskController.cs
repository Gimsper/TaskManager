using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models.Dto;
using Newtonsoft.Json;
using TaskManagerAPI.Domain.Models;
using TaskManagerAPI.Domain.Models.Task;

namespace TaskManagerAPI.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = false)]

    public class TaskController : BaseController
    {
        private readonly IConfiguration _configuration;

        public TaskController(IServiceProvider serviceProvider, IOptions<SectionConfiguration> configuration, IConfiguration configurationSettings)
            : base(serviceProvider, configuration)
        {
            _configuration = configurationSettings;
        }

        #region GET

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Domain.Entities.Task>))]
        public IActionResult GetAll([FromQuery] QueryFilter query, [FromHeader] string Authorization)
        {
            try
            {
                var tokenString = ValidateAuthorizationHeader(Authorization);
                var token = ValidateToken(tokenString, _configuration["Authentication:SecretKey"] ?? "");
                ValidateRole(token, "Administrador");

                var response = _taskService.GetAllInclude(query);
                if (!response.stateOperation)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, response);
                }

                if (!string.IsNullOrEmpty(response.MessageResult))
                {
                    return Ok(response.MessageResult);
                }

                var metadata = new
                {
                    response.Result?.TotalCount,
                    response.Result?.PageSize,
                    response.Result?.CurrentPage,
                    response.Result?.TotalPages,
                    response.Result?.HasNextPage,
                    response.Result?.HasPreviousPage
                };

                Response.Headers.Append("Pagination", JsonConvert.SerializeObject(metadata));

                return Ok(new { tasks = response.Result, metadata });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (SecurityTokenException ex)
            {
                return Unauthorized($"Token inválido: {ex.Message}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al procesar la solicitud: {ex.Message}");
            }

        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Domain.Entities.Task))]
        public IActionResult GetById(int id, [FromHeader] string Authorization)
        {
            try
            {
                var tokenString = ValidateAuthorizationHeader(Authorization);
                var token = ValidateToken(tokenString, _configuration["Authentication:SecretKey"] ?? "");
                ValidateRole(token, "Administrador");

                var response = _taskService.GetByIdInclude(id);
                if (!response.stateOperation)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, response);
                }

                if (!string.IsNullOrEmpty(response.MessageResult))
                {
                    return Ok(response.MessageResult);
                }

                return Ok(response.Result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (SecurityTokenException ex)
            {
                return Unauthorized($"Token inválido: {ex.Message}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al procesar la solicitud: {ex.Message}");
            }

        }

        #endregion

        #region POST

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        public IActionResult Add([FromBody] CreateTaskDTO task, [FromHeader] string Authorization)
        {
            try
            {
                var tokenString = ValidateAuthorizationHeader(Authorization);
                var token = ValidateToken(tokenString, _configuration["Authentication:SecretKey"] ?? "");
                ValidateRole(token, "Administrador");

                if (task == null)
                {
                    return BadRequest("The request body cannot be null.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var entity = new Domain.Entities.Task
                {
                    Title = task.Title,
                    Description = task.Description,
                    DueDate = task.DueDate,
                    StateId = task.StateId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                var response = _taskService.Add(entity);
                if (!response.stateOperation)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, response);
                }

                if (!string.IsNullOrEmpty(response.MessageResult))
                {
                    return Ok(response.MessageResult);
                }

                return Ok(response.stateOperation);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (SecurityTokenException ex)
            {
                return Unauthorized($"Token inválido: {ex.Message}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al procesar la solicitud: {ex.Message}");
            }

        }

        #endregion

        #region PUT

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        public IActionResult Update([FromBody] UpdateTaskDTO task, [FromHeader] string Authorization)
        {
            try
            {
                var tokenString = ValidateAuthorizationHeader(Authorization);
                var token = ValidateToken(tokenString, _configuration["Authentication:SecretKey"] ?? "");
                ValidateRole(token, "Administrador");

                if (task == null)
                {
                    return BadRequest("The request body cannot be null.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var entity = new Domain.Entities.Task
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    DueDate = task.DueDate,
                    StateId = task.StateId,
                    UpdatedAt = DateTime.UtcNow
                };
                var response = _taskService.Update(entity);
                if (!response.stateOperation)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, response);
                }

                if (!string.IsNullOrEmpty(response.MessageResult))
                {
                    return Ok(response.MessageResult);
                }

                return Ok(response.stateOperation);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (SecurityTokenException ex)
            {
                return Unauthorized($"Token inválido: {ex.Message}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al procesar la solicitud: {ex.Message}");
            }

        }

        #endregion

        #region DELETE

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        public IActionResult Delete(int id, [FromHeader] string Authorization)
        {
            try
            {
                var tokenString = ValidateAuthorizationHeader(Authorization);
                var token = ValidateToken(tokenString, _configuration["Authentication:SecretKey"] ?? "");
                ValidateRole(token, "Administrador");

                var response = _taskService.Delete(id);
                if (!response.stateOperation)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, response);
                }

                if (!string.IsNullOrEmpty(response.MessageResult))
                {
                    return Ok(response.MessageResult);
                }

                return Ok(response.stateOperation);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (SecurityTokenException ex)
            {
                return Unauthorized($"Token inválido: {ex.Message}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al procesar la solicitud: {ex.Message}");
            }

        }

        #endregion
    }
}
