using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog;

namespace ForDotNet.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private ILogger<TestController> _logger;
        public TestController(ILogger<TestController> logger) 
        {
            _logger = logger;
        }
        //[Authorize]
        [HttpGet]
        public IActionResult Get() 
        {
            if (!HttpContext.User.IsAuthenticated()) 
            {
                _logger.LogWarning("未授权请求!");
            }
            return new JsonResult(new { Code = 200 ,Msg = "请求成功!" });
        }
    }
}
