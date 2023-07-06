using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TEbucksServer.DAO;
using TEbucksServer.Models;

namespace TEbucksServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransferController : ControllerBase
    {
        private readonly IAccountDAO accountDao;
        private readonly ITransferDao transferDao;

        [HttpGet("{id}")]
        public ActionResult<Transfer> Get(int id)
        {
            Transfer transfer = transferDao.GetTransferById(id);
            if (transfer != null)
            {
                return Ok(transfer);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPost]
        public ActionResult<Transfer> CreateTransfer(Transfer transfer)
        {
            Transfer added = transferDao.CreateNewTransfer(transfer);
            return Created($"/auctions/{added.TransferID}", added);

        }


    }
}
