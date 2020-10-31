using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tgs.Common.Util.General;

namespace Tgs.Web.Authentication.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _logger.LogDebug(1, "NLog injected into HomeController");
        }

        [HttpGet]
        public ActionResult<Result> Get()
        {
            _logger.LogInformation("Api funcionando correctamente");

            Result response = new Result
            {
                IsSuccess = true,
                Message = "Servicio esta funcionando correctamente"
            };

            return response;
        }
    }
}
