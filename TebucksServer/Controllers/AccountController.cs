﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq.Expressions;
using TEbucksServer.DAO;
using TEbucksServer.Models;
using TEBucksServer.Models;

namespace TEbucksServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountDAO accountDao;

        [HttpGet]
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
            catch (System.Exception)
            {
                return StatusCode(500);
            }
        }
        [HttpGet]
        public ActionResult<List<Transfer>> GetAccountTrans()
        {
            try
            {
                //TODO Dan this should work once you create Transfer
                Account output = accountDao.GetAccountTransfer(User.Identity.Name);
                if (output.user_Id > 0)
                {
                    return Ok(output);
                }
                return NotFound(new { messgae = "User Not found!" });
            }
            catch (System.Exception)
            {
            return StatusCode(500);
            }
        }
    }
}