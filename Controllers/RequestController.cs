using Azure.Core;
using Kill_hunger.Data;
using Kill_hunger.Models;
using Kill_hunger.RequestDto;
using Kill_hunger.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Request = Kill_hunger.Models.Request;

namespace Kill_hunger.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public class RequestController : Controller
    {
        private readonly ApplicationDataContext _context;

        public RequestController(ApplicationDataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Return All Requests
        /// </summary>
        /// <response code="200">Return All Requests</response>
        /// <response code="400">The server was unable to process the request</response>
        /// <response code="401">Client is not authenticated</response>
        /// <response code="403">Client lack sufficient permission to perform the request</response>
        /// <response code="500">Unexpected Error encountered</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        [Route("[controller]/[action]")]
        public async Task<APIResponse> Get()
        {
            APIResponse aPIResponse = new();

            var request = await _context.Requests.ToListAsync();

            aPIResponse.Data = request;
            aPIResponse.Status = "Success";

            return aPIResponse;
        }

        /// <summary>
        /// Return All Requests
        /// </summary>
        /// <response code="200">Return All Requests</response>
        /// <response code="400">The server was unable to process the request</response>
        /// <response code="401">Client is not authenticated</response>
        /// <response code="403">Client lack sufficient permission to perform the request</response>
        /// <response code="500">Unexpected Error encountered</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        [Route("[controller]/[action]/{UserId}")]
        public async Task<APIResponse> GetRequestByUserId(int UserId)
        {
            APIResponse aPIResponse = new();

            var request = await _context.Requests.Where(x => x.UserId == UserId).ToListAsync();

            aPIResponse.Data = request;
            aPIResponse.Status = "Success";

            return aPIResponse;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        [Route("[controller]/[action]/{UserId}")]
        public async Task<APIResponse> GetNearestRequests(int UserId)
        {
            APIResponse aPIResponse = new();
            var ipAddress = HttpContext.GetServerVariable("HTTP_X_FORWARDED_FOR") ?? HttpContext.Connection.RemoteIpAddress?.ToString();
            var ipAddressWithoutPort = ipAddress?.Split(':')[0];

            ipAddressWithoutPort = ipAddressWithoutPort == "" ? "::1" : ipAddressWithoutPort;
          
            List<string> GeaLocationData = GeaoLocationService.GetIpLocation(ipAddressWithoutPort);
            if (GeaLocationData == null)
            {
                var usercity = _context.Users.Where(User => User.UserId == UserId).FirstOrDefault().City ?? "";
                if(usercity != null)
                {
                    var requestList = _context.Requests.Where(c => c.City == usercity).ToList();
                    aPIResponse.Status = "Success";
                    aPIResponse.Data = requestList;
                }
                else
                {
                    aPIResponse.Message = "gea location is not fount";
                    aPIResponse.Status = "Error";
                }
            }
           

         
            

            return aPIResponse;
        }

        /// <summary>
        /// Creat a Request
        /// </summary>
        /// <response code="201">Create a Request</response>
        /// <response code="400">The server was unable to process the request</response>
        /// <response code="401">Client is not authenticated</response>
        /// <response code="403">Client lack sufficient permission to perform the request</response>
        /// <response code="500">Unexpected Error encountered</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [HttpPost]
        [Route("[controller]")]
        public async Task<APIResponse> Post(CreateRequestParameters param)
        {
            APIResponse aPIResponse = new();

            if (ModelState.IsValid)
            {
                var user = _context.Users.Where(x => x.UserId == param.UserId).FirstOrDefault();

                if (user == null)
                {
                    aPIResponse.Status = "Error";
                    aPIResponse.Message = "Invalid userId";

                    return aPIResponse;
                }

                var request = new Request
                {
                    Discription = param.Discription,
                    RequestType = param.RequestType,
                    City = param.City,
                    Country = param.Country,
                    UserId = param.UserId,
                };

                _context.Requests.Add(request);
                await _context.SaveChangesAsync();

                aPIResponse.Status = "Success";
                aPIResponse.Message = "Request Created Successfully";

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

        /// <summary>
        /// Creat a Request
        /// </summary>
        /// <response code="201">Create a Request</response>
        /// <response code="400">The server was unable to process the request</response>
        /// <response code="401">Client is not authenticated</response>
        /// <response code="403">Client lack sufficient permission to perform the request</response>
        /// <response code="500">Unexpected Error encountered</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [HttpPut]
        [Route("[controller]")]
        public async Task<APIResponse> Edit(UpdateRequestParameters parameters)
        {
            APIResponse aPIResponse = new();

            if (ModelState.IsValid)
            {
                var request = _context.Requests.Where(x => x.Id == parameters.Id).FirstOrDefault();

                if (request == null)
                {
                    aPIResponse.Status = "Error";
                    aPIResponse.Message = "Unable to Update the Request of given Id";

                    return aPIResponse;
                }

                request.Id = parameters.Id;
                request.Discription = parameters.Discription;
                request.RequestType = parameters.RequestType;
                request.UserId = parameters.UserId;
                request.IsClaimed = parameters.IsClaimed;
                request.IsDelete = parameters.IsDelete;

                _context.Requests.Update(request);

                await _context.SaveChangesAsync();

                aPIResponse.Status = "Success";
                aPIResponse.Message = "Request Updated Successfully";

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

        /// <summary>
        /// Delete a Request
        /// </summary>
        /// <response code="204">Delete a Request</response>
        /// <response code="400">The server was unable to process the request</response>
        /// <response code="401">Client is not authenticated</response>
        /// <response code="403">Client lack sufficient permission to perform the request</response>
        /// <response code="500">Unexpected Error encountered</response>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [Route("[controller]")]
        [Route("Id")]
        public async Task<APIResponse> Delete(int Id)
        {
            APIResponse aPIResponse = new();

            var request = await _context.Requests.SingleOrDefaultAsync(x => x.Id == Id); ;

            if (request != null)
            {
                _context.Requests.Remove(request);
                _context.SaveChanges();

                aPIResponse.Status = "Success";
                aPIResponse.Message = "Request Deleted Successfully";

                return aPIResponse;
            }
            else
            {
                aPIResponse.Status = "Error";
                aPIResponse.Message = "Unable to delete the Request of given Id";

                return aPIResponse;
            }
        }

        //public async Task<APIResponse> GetRequest(int userid)
        //{
        //    var ipAddress = HttpContext.GetServerVariable("HTTP_X_FORWARDED_FOR") ?? HttpContext.Connection.RemoteIpAddress?.ToString();
        //    var ipAddressWithoutPort = ipAddress?.Split(':')[0];
        //    APIResponse aPIResponse = new APIResponse();
        //    ipAddressWithoutPort = ipAddressWithoutPort == "" ? "::1" : ipAddressWithoutPort;
        //    List<string> GeaLocationData = GetIpLocation(ipAddressWithoutPort);

        //}

    }
}
