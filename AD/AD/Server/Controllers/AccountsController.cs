using AD.Shared.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AD.Server.Services.Interfaces;
using AD.Server.Repositories;
using AD.Server.Models;
using AD.Shared.DTOs.Auth;
using AD.Shared.Entities;
using AD.Shared.Entities.Base.Enums;
using Microsoft.EntityFrameworkCore;
using AD.Shared.Entities.Base;
using System.Net;

namespace AD.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IUserServices _userServices;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IRepository<UserC> _userCRepository;
        private readonly IRepository<UserA_B> _userA_BRepository;
        private readonly IUserCServices _userCServices;

        public AccountsController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration, IUserServices userServices, IRepository<UserC> userRepository, IRepository<UserA_B> userA_BRepository, IUserCServices userCServices)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _userServices = userServices;
            _userCRepository = userRepository;
            _userA_BRepository = userA_BRepository;
            _userCServices = userCServices;
        }

        [HttpPost("Create")]
        public async Task<ActionResult<ApiResponse>> CreateUser([FromBody] RegisterDTO model)
        {
            try
            {
                var user = new IdentityUser { UserName = model.UserName, Email = model.UserName, PhoneNumber = model.PhoneNumber };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // Add all new users to the User role
                    await _userManager.AddToRoleAsync(user, "User");

                    UserC userC = new()
                    {
                        PhoneNumber = model.PhoneNumber,
                        UserName = model.UserName,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        BirthYear = model.BirthYear,
                        CityId = model.CityId,
                        CurrencyId = model.CurrencyId
                    };
                    long max = 0;
                    var query = await _userCRepository.TableNoTracking.FirstOrDefaultAsync();
                    if (query != null)
                        max = await _userCRepository.TableNoTracking.MaxAsync(x => x.Id);
                    userC.Code = $"UserC_{max + 1}";
                    await _userCRepository.InsertAsync(userC);

                    var myResult = await BuildToken(new BuildTokenDTO { UserName = model.UserName, Password = model.Password });
                    return ApiResponse.Create(HttpStatusCode.OK, myResult);

                }
                else
                {
                    return ApiResponse.Create(HttpStatusCode.OK, "UserName or Password Invalid");
                }
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult<ApiResponse>> Login([FromBody] LoginDTO userInfo)
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync(userInfo.UserName,
                    userInfo.Password, isPersistent: false, lockoutOnFailure: false);

                var userC = _userCRepository.TableNoTracking.SingleOrDefault(x => x.UserName == userInfo.UserName);
                BuildTokenDTO buildTokenDTO = new();
                buildTokenDTO.UserName = userInfo.UserName;
                if (userC != null)
                {
                    buildTokenDTO.CurrencyId = userC.CurrencyId;
                    if (result.Succeeded && userC.Status != Status.Offline)
                        return ApiResponse.Create(HttpStatusCode.OK, await BuildToken(buildTokenDTO));

                    else if (userC.Status == Status.Offline)
                        return ApiResponse.Create(HttpStatusCode.OK, "Sorry You are Freezed");

                    else
                        return ApiResponse.Create(HttpStatusCode.OK, "Invalid Login Attempt");
                }
                else if (result.Succeeded)
                    return ApiResponse.Create(HttpStatusCode.OK, await BuildToken(buildTokenDTO));
                else
                    return ApiResponse.Create(HttpStatusCode.OK, "Invalid Login Attempt");
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }

            //else
            //{
            //    var userA_B = _userA_BRepository.TableNoTracking.SingleOrDefault(x => x.UserName == userInfo.UserName);
            //    if (userA_B != null)
            //        buildTokenDTO.PhoneNumber = userA_B.PhoneNumber;

            //    else
            //        return BadRequest("Invalid Login Attempt!");
            //}
        }

        [HttpGet("RenewToken")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<ApiResponse>> Renew()
        {
            try
            {
                var buildTokenDTO = new BuildTokenDTO()
                {
                    UserName = User.Identity.Name
                };

                return ApiResponse.Create(HttpStatusCode.OK, await BuildToken(buildTokenDTO));
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }
        }

        private async Task<UserToken> BuildToken(BuildTokenDTO userinfo)
        {
            List<Claim> claims = new();

            claims.Add(new Claim(ClaimTypes.Name, userinfo.UserName));
            if (!String.IsNullOrEmpty(userinfo.CurrencyId.ToString()))
                claims.Add(new Claim("CurrencyId", userinfo.CurrencyId.ToString()));

            var identityUser = await _userManager.FindByNameAsync(userinfo.UserName);
            var claimsDB = await _userManager.GetClaimsAsync(identityUser);

            var user = await _signInManager.UserManager.FindByNameAsync(userinfo.UserName);
            var rolesDB = await _signInManager.UserManager.GetRolesAsync(user);

            claims.AddRange(claimsDB);
            foreach (var role in rolesDB)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddDays(1);

            JwtSecurityToken token = new JwtSecurityToken(
               issuer: "AD",
               audience: "AD",
               claims: claims,
               expires: expiration,
               signingCredentials: creds);

            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }
    }
}
