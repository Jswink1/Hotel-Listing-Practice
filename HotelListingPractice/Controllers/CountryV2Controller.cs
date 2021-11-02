using HotelListingPractice.DataAccess.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// This Controller is used to test Versioning

namespace HotelListingPractice.Controllers
{
    // Specify the endpoint to be an updated CountryController
    [ApiVersion("2.0")]
    [Route("api/country")]

    // When a newer version of this controller is made, the older versions can be marked as deprecated
    //[ApiVersion("2.0", Deprecated = true)]

    // Api version can be specified in the route parameters like this
    //[Route("api/{v:apiversion}/country")]     

    [ApiController]
    public class CountryV2Controller : ControllerBase
    {
        private readonly DatabaseContext _context;

        public CountryV2Controller(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountries()
        {
            return Ok(_context.Countries);
        }
    }
}
