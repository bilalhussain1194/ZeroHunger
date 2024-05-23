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
        public async Task<APIResponse> Get()
        {
            APIResponse aPIResponse = new();

            var request = await _context.RequestClaims.ToListAsync();

            aPIResponse.Data = request;
            aPIResponse.Status = "Success";

            return aPIResponse;
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
        public async Task<APIResponse> Post(CreateRequestClaimParameters param)
        {
            APIResponse aPIResponse = new();

            if (ModelState.IsValid)
            {
                var user = _context.Users.Where(x => x.UserId == param.RequestClaimBye).FirstOrDefault();

                if (user == null)
                {
                    aPIResponse.Status = "Error";
                    aPIResponse.Message = "Invalid userId";

                    return aPIResponse;
                }

                var request = _context.Requests.Where(x => x.Id == param.RequestId).FirstOrDefault();

                if (request == null)
                {
                    aPIResponse.Status = "Error";
                    aPIResponse.Message = "Invalid RequestId";

                    return aPIResponse;
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

                aPIResponse.Status = "Success";
                aPIResponse.Message = "RequestClaims Created Successfully";

                return aPIResponse;
            }
            else
            {
                var errors = ModelState.Values.SelectMany(c => c.Errors).Select(e => e.ErrorMessage);

                if (errors.Any())
                {
                    aPIResponse.Message = errors.ToString()!;
                }
                aPIResponse.Status = "Error";

                return aPIResponse;
            }
        }
    }
}
