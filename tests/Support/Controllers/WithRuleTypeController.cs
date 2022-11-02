using JSM.FluentValidation.AspNet.AsyncFilter.Tests.Support.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace JSM.FluentValidation.AspNet.AsyncFilter.Tests.Support.Controllers
{
    [ApiController, Route("[controller]")]
    public class WithRuleTypeController : ControllerBase
    {
        [HttpPost("test-validator")]
        public IActionResult Post([FromBody] TestPayloadWithRuleType request) => Ok();
    }
}