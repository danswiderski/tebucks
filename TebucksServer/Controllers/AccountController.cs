using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq.Expressions;
using TEbucksServer.DAO;
using TEbucksServer.Models;
using TEBucksServer.DAO;
using TEBucksServer.Security;

namespace TEbucksServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IAccountDAO accountDao;
        private readonly ITransferDao transferDao;

        public AccountController(ITransferDao _transferDao, IAccountDAO _accountDao)
        {
            transferDao = _transferDao;
            accountDao = _accountDao;
        }
        [HttpGet("balance")]
        public ActionResult<Account> GetAccountByName()
        {
            try
            {
                Account output = accountDao.GetAccountByUserName(User.Identity.Name);
                if (output.user_Id > 0)
                {
                    return Ok(output);
                }
                return NotFound(new { message = "User Not found!" });
            }
            catch (System.Exception e)
            {
                return StatusCode(500);
            }
        }
        [HttpGet ("transfers")]
        public ActionResult<List<Transfer>> GetAccountTrans()
        {
            try
            {
                //TODO Dan this should work once you create Transfer
                List<Transfer> output = transferDao.GetAccountTransfer(User.Identity.Name);
                if (output.Count > 0)
                {
                    return Ok(output);
                }
                return NotFound(new { message = "User Not found!" });
            }
            catch (System.Exception)
            {
            return StatusCode(500);
            }
        }
    }
}