using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TEbucksServer.DAO;
using TEbucksServer.Models;
using TEBucksServer.DAO;
using TEBucksServer.Models;

namespace TEbucksServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserDao userDao;
        public UsersController(IUserDao _userDao)
        {
            userDao = _userDao;
    
        }
        [HttpGet]
        public ActionResult<List<User>> GetUsers()
        {
            return Ok(userDao.GetUsers(User.Identity.Name));
        }

    }
}
