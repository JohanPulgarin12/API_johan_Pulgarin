using Entities.Core;
using Entities.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Models.Dto;
using Models.Services;
using Models.Services.Interface;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ParametrizacionesApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly ConfigurationSectionWebApi _config;
        private readonly IUserService _userService;

        public UserController(IOptions<ConfigurationSectionWebApi> config)
        {
            _config = config.Value;
            _userService = new UserService(_config);
        }

        [HttpGet]
        [Route("GetUser")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<List<User>> GetUser()
        {
            var response = _userService.GetUser();
            return response.Result;
        }

        [HttpGet]
        [Route("GetUserData")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<List<UserDTO>> GetUserData()
        {
            var response = _userService.GetUserData();
            return response.Result;
        }

        [HttpPost]
        [Route("PostUser")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<bool> PostUser([FromBody] UserDTO userDTO)
        {
            var response = _userService.PostUser(userDTO);
            return response.Result;
        }

        [HttpPost]
        [Route("PutUser")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<bool> PutUser([FromBody] User user)
        {
            var response = _userService.PutUser(user);
            return response.Result;
        }

        [HttpPost]
        [Route("DeletetUser")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<bool> DeleteUser([FromBody] UserDeleteDTO userDeleteDTO)
        {
            var response = _userService.DeleteUser(userDeleteDTO);
            return response.Result;
        }

        [HttpPost]
        [Route("Login")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<bool> Login([FromBody] UserLoginDTO userLoginDTO)
        {
            var response = _userService.Login(userLoginDTO);
            return response;
        }
    }
}
