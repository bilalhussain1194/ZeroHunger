using Kill_hunger.Data;
using Kill_hunger.Models;
using Kill_hunger.RequestDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kill_hunger.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class RequestClaimController : Controller
    {
        private readonly ApplicationDataContext _context;

        public RequestClaimController(ApplicationDataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Return All RequestClaims
        /// </summary>
        /// <response code="200">Return All RequestClaims</response>
        /// <response code="400">The server was unable to process the request</response>
        /// <response code="401">Client is not authenticated</response>
        /// <response code="403">Client lack sufficient permission to perform the request</response>
        /// <response code="500">Unexpected Error encountered</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<RequestClaim>> Get()
        {
            var request = await _context.RequestClaims.ToListAsync();

            return Ok(request);
        }

        /// <summary>
        /// Creat a RequestClaim
        /// </summary>
        /// <response code="201">Create a RequestClaim</response>
        /// <response code="400">The server was unable to process the request</response>
        /// <response code="401">Client is not authenticated</response>
        /// <response code="403">Client lack sufficient permission to perform the request</response>
        /// <response code="500">Unexpected Error encountered</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [HttpPost]
        public async Task<ActionResult> Post(CreateRequestClaimParameters param)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.Where(x => x.UserId == param.RequestClaimBye).FirstOrDefault();

                if (user == null)
                {
                    return BadRequest("Invalid userId");
                }

                var request = _context.Requests.Where(x => x.Id == param.RequestId).FirstOrDefault();

                if (request == null)
                {
                    return BadRequest("Invalid RequestId");
                }

                var requestClaim = new RequestClaim
                {
                    Discription = param.Discription,
                    RequestClaimType = param.RequestClaimType,
                    RequestClaimBye = param.RequestClaimBye,
                    RequestId = param.RequestId,
                };

                _context.RequestClaims.Add(requestClaim);
                await _context.SaveChangesAsync();

                return Created();
            }
            else
            {
                var errors = ModelState.Values.SelectMany(c => c.Errors).Select(e => e.ErrorMessage);
                return BadRequest(new { Error = errors });
            }
        }
    }
}
