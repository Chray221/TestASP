using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SwaggerTest.Controllers;
using Swashbuckle.AspNetCore.Annotations;
using TestASP.Data;
using TestASP.Data.Enums;
using TestASP.Domain.Contexts;
using TestASP.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestASP.API.Controllers
{
    [SwaggerTag("Test Controller")]
    [ApiVersion("1")]
    [ApiController]
    [Route("v{version:apiVersion}/Test")]
    public class TestController : ControllerBase
    {
        private readonly ILogger _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [SwaggerOperation(Summary = "Test Phonenumber Mapper", Description = "Test PhoneNumber using swagger Type Mapper")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(PhoneNumber), Description = "Success")]
        [HttpPost("PhoneNumber")]
        public PhoneNumber ShowPhonenumber([FromBody] PhoneNumber phoneNumber)
        {
            return phoneNumber;
        }

        [Authorize]
        [SwaggerOperation(Summary = "With Authorize Attribute", Description = "With Authorize Attribute")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(GenderEnum), Description = "Success")]
        [HttpPost("Authorize")]
        public IActionResult GetAuthorizeSample([FromQuery, SwaggerParameter("Gender Param")] GenderEnum gender)
        {
            return Ok(gender);
        }

        [SwaggerOperation(Summary = "Test Enum", Description = "Test Gender Enum")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(GenderEnum), Description = "Success")]
        [HttpPost("Gender")]
        public GenderEnum GetGender([FromQuery] GenderEnum gender)
        {
            return gender;
        }

        #region TestRequest

        [AllowAnonymous]
        [SwaggerOperation(Summary = "List From Form Attribute", Description = "List from FromFormAttribute/[FromForm] params and with the use of FromFOrmWithListSwaggerFilter to show form params format for sending")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(List<Item>), Description = "Success")]
        [HttpPost("/FromForm/List/Item")]
        public IActionResult TestItemRequest(
            [FromForm] Item requests)
        //[FromForm] List<Item> requests)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(requests);
        }

        [AllowAnonymous]
        [SwaggerOperation(Summary = "Nested List From Form Attribute", Description = "Nested List from FromFormAttribute/[FromForm] params and with the use of FromFOrmWithListSwaggerFilter to show form params format for sending")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(List<Item>), Description = "Success")]
        [HttpPost("/FromForm/List")]
        public IActionResult TestRequest(
            //[FromForm] Item requests)
            [FromForm] List<Item> requests)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(requests);
        }

        [AllowAnonymous]
        [SwaggerOperation(Description = "Save DataType")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(DataTypeTable), Description = "Success")]
        [HttpPost("DataType/Save")]
        public async Task<IActionResult> TestRequest(
            [FromBody] DataTypeTable dataType,
            TestDbContext testDb)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            testDb.DataTypeTables.Add(dataType);
            if (await testDb.SaveChangesAsync() < 1)
            {
                return BadRequest("Something went wrong in saving DataTypeTable");
            }

            return Ok(dataType);
        }

        #endregion
    }
}

