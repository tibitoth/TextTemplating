using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace RuntimeTemplateSample.Controllers
{
    [Route("api/[controller]")]
    public class DevController : Controller
    {
        private readonly IApiDescriptionGroupCollectionProvider _apiExplorer;
        public DevController(IApiDescriptionGroupCollectionProvider apiExplorer)
        {
            _apiExplorer = apiExplorer;
        }

        [HttpGet]
        public IActionResult ApiClient([FromQuery] string lib)
        {

        }
    }
}
