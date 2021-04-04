using Microsoft.AspNetCore.Mvc;
using AD.Server.Services.Interfaces;
using AD.Shared.Entities.MTM;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace AD.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FavouritesController : ControllerBase
    {
        private readonly IUserCServices _userCServices;
        private readonly IFavouritesServices _favouritesServices;

        public FavouritesController(IFavouritesServices favouritesServices, IUserCServices userCServices)
        {
            _favouritesServices = favouritesServices;
            _userCServices = userCServices;
        }

        [HttpPost("Follow_UnFollow_Store")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        public async Task<ActionResult> Follow_UnFollow_Store(StoreFollowers entity)
        {
            #region Start the watch   
            var watch = new Stopwatch();
            watch.Start();
            #endregion

            entity.UserCId = (await _userCServices.GetCurrent(User.Identity.Name)).Id;
            var result = await _favouritesServices.Follow_UnFollow_Store(entity);
           
            #region End the watch  
            watch.Stop();
            result.Meta.TotalProcessingTime = watch.ElapsedMilliseconds;
            #endregion

            return Ok(result);
        }

        [HttpPost("Follow_UnFollow_Category")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        public async Task<ActionResult> Follow_UnFollow_Category(CategoryFollowers entity)
        {
            #region Start the watch   
            var watch = new Stopwatch();
            watch.Start();
            #endregion

            entity.UserCId = (await _userCServices.GetCurrent(User.Identity.Name)).Id;
            var result = await _favouritesServices.Follow_UnFollow_Category(entity);

            #region End the watch  
            watch.Stop();
            result.Meta.TotalProcessingTime = watch.ElapsedMilliseconds;
            #endregion

            return Ok(result);
        }
    }
}
