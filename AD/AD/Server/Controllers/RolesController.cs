using Microsoft.AspNetCore.Mvc;
using AD.Server.Services.Interfaces;
using AD.Shared.Entities.Base.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleServices _entityServices;
        public RolesController(IRoleServices entityServices)
        {
            _entityServices = entityServices;
        }

        [HttpGet("{storeId}")]
        public async Task<ActionResult> GetRolesForStore(long storeId)
        {
            #region Start the watch   
            var watch = new Stopwatch();
            watch.Start();
            #endregion

            var result = await _entityServices.GetRolesForStore(storeId, User.Identity.Name);
            #region End the watch  
            watch.Stop();
            result.Meta.TotalProcessingTime = watch.ElapsedMilliseconds;
            #endregion

            return Ok(result);
        }
    }
}
