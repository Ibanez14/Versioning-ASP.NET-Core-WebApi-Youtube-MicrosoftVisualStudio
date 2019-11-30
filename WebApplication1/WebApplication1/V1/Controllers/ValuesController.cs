using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.V1.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("1.5")]
    [Route("api/[controller]")]
    // If this route specified this will override configured
    // QueryApiVersionReader and HeaderApiVersionReader in Startup
    // and will handle request like 
    //~/api/1.0/values
    //~/api/1.5/values
    //~/api/2.0/values
    // BUT X-Version:1.0 in HTTP Request won't work and ?api-version=1.0 Won't work as well
    //[Route("api/{version:apiVersion}/[controller]")]
    public class ValuesController : Controller
    {
        // https://localhost:5001/api/values?api-version=1.0
        // If ApiVersionReader = QueryStringApiVersionReader() which is by default

        [HttpGet]
        [MapToApiVersion("1.0")]
        public IActionResult Get1_0()
            => Ok("From version 1");


        // https://localhost:5001/api/values?api-version=1.5
        // If ApiVersionReader = QueryStringApiVersionReader() which is by default
        [HttpGet]
        [MapToApiVersion("1.5")]
        public IActionResult Get1_5()
            => Ok("From version 1.5");
    }
}
