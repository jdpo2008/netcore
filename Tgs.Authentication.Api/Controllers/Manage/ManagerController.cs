using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tgs.Common.Util.General;
using Tgs.DataAccess.Core.Context;
using Tgs.DataContract.Identity;
using Tgs.Entities.Seguridad;

namespace Tgs.Web.Authentication.Api.Controllers.Manage
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ManagerController> _logger;

        public ManagerController(ApplicationDbContext Context, ILogger<ManagerController> logger)
        {
            _context = Context;
            _logger = logger;
            _logger.LogDebug(1, "NLog injected into UserController");
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("Users")]
        public Result<List<UserModelDto>> Users()
        {
            Result<List<UserModelDto>> response = new Result<List<UserModelDto>>();
            List<UserModelDto> Users;

            try
            {
                Users = _context.Users.Select(user => new UserModelDto
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    EmailConfirmed = user.EmailConfirmed
                    //Roles = _userManager.GetRolesAsync(user)
                }).ToList();

                response.IsSuccess = true;
                response.Data = Users;
                _logger.LogInformation(1, "Se envia la lista de los usuarios");
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Data = null;
                response.Message = "Error desde el Servidor " + ex.Message;
                _logger.LogError("Error inesperado en el servidor" + ex.Message);
            }

            return response;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("User")]
        public Result<ApplicationUser> UserByEmail([FromBody] UserModelDto input)
        {
            Result<ApplicationUser> response = new Result<ApplicationUser>();
            ApplicationUser user;
            try
            {
                user = _context.Users.Select(user => new ApplicationUser
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    EmailConfirmed = user.EmailConfirmed,
                    CreatedAt = user.CreatedAt
                }).Where(u => u.Email == input.Email).FirstOrDefault();

                if (user != null)
                {
                    response.IsSuccess = true;
                    response.Data = user;
                    _logger.LogInformation(1, "Se envia la informacion del usuario " + input.Email);
                } else
                {
                    response.IsSuccess = false;
                    response.Message = "Usuario no encontrado en la base de datos";
                    response.Data = null;
                    _logger.LogInformation("EUsuario no encontrado" + input.Email);
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Error en el Servidor " + ex.Message;
                response.Data = null;
                _logger.LogError("Error inesperado en el servidor" + ex.Message);
            }

            return response;

        }
    }
}
