using AdventureWorksWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace AdventureWorksWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StateProvincesController : ControllerBase
    {

        private readonly AdventureWorks2019Context _context;
        public StateProvincesController(AdventureWorks2019Context context)
        {
            this._context = context;
        }

        /// <summary>
        /// Returns list of all State Provinces
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Description("GetStateProvinces")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StateProvince))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetStateProvinces()
        {
            List<StateProvince> stateProvinces = await _context.StateProvinces.ToListAsync();
            return stateProvinces == null ? NotFound() : Ok(stateProvinces);
        } 

    }
}
