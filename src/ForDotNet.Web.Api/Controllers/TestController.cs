using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ForDotNet.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        //[Authorize]
        [HttpGet]
        public IActionResult Get() 
        {
            return new JsonResult(new { Code = 200 ,Msg = "请求成功!" });
        }
    }
}
