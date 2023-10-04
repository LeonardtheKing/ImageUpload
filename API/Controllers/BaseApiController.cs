using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    // [ServiceFilter(typeof(LogUserActivity))]
    [ApiController]
    [Route("api")]
    public class BaseApiController : ControllerBase
    {
        
    }
}