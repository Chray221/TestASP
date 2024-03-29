﻿using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TestASP.Model;

namespace TestASP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {

        [SwaggerOperation(Summary = "Cannot be tested in swagger", Description = "This request can only be tested successfully in postman")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(LoginRequest), Description = "Success")]
        [HttpGet]
        public async Task<IActionResult> GetTokenAsync([FromBody] LoginRequest body )
        {
            return Ok(body);
        }
    }
}
