using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TEbucksServer.DAO;
using TEbucksServer.Models;

namespace TEbucksServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransfersController : ControllerBase
    {
        private readonly IAccountDAO accountDao;
        private readonly ITransferDao transferDao;

        public TransfersController(ITransferDao _transferDao, IAccountDAO _accountDao)
        {
            transferDao = _transferDao;
            accountDao = _accountDao;
        }


        [HttpGet("{id}")]
        public ActionResult<Transfer> Get(int id)
        {
            Transfer transfer = transferDao.GetTransferByID(id);
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
        public ActionResult<Transfer> CreateTransfer(NewTransfer transfer)
        {
            Transfer added = transferDao.CreateNewTransfer(transfer);
            return Created($"/auctions/{added.TransferID}", added);
        }
        [HttpPut ("{id}/status")]
        public ActionResult<Transfer> UpdateTransfer(int id, string newStatus)
        {
            if (transferDao.GetTransferByID(id) == null)
            {
                return NotFound();
            }
            Transfer updatestatus = transferDao.UpdateTransferStatus(id, newStatus);
            return Ok(transferDao.GetTransferByID(id));
        }

    }
}
