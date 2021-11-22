using FluentValidation.AspNet.AsyncValidationFilter.Tests.Support.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FluentValidation.AspNet.AsyncValidationFilter.Tests.Support.Controllers
{
    [Route("[controller]")]
    public class WithoutApiAttributeController : ControllerBase
    {
        [HttpPost("test-validator")]
        public IActionResult Post([FromBody] TestPayload request) => Ok();

        [HttpPost("test-validator-collection")]
        public IActionResult PostCollection([FromBody] IEnumerable<TestPayload> request) => Ok();

        [HttpPost("without-validation")]
        public IActionResult Post([FromBody] TestPayloadWithoutValidation request) => Ok();
    }
}