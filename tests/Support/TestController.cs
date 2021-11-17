using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FluentValidation.AspNet.AsyncValidationFilter.Tests.Support
{
    public class TestController : ControllerBase
    {
        [HttpPost("/test-validator")]
        public IActionResult Post([FromBody] TestPayload request) => Ok();

        [HttpPost("/test-validator-collection")]
        public IActionResult PostCollection([FromBody] IEnumerable<TestPayload> request) => Ok();
    }
}