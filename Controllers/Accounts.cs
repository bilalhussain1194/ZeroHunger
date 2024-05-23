using Kill_hunger.Data;
using Kill_hunger.Models;
using MaxMind.GeoIP2;
using MaxMind.GeoIP2.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.Eventing.Reader;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Kill_hunger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  
    public class Accounts : ControllerBase
    {

        private IConfiguration _config;
        private ApplicationDataContext _dbcontext;

        public Accounts(IConfiguration config,ApplicationDataContext dbcontext)
        {
            _config = config;
            _dbcontext = dbcontext;
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public APIResponse Login([FromBody] UserModel login)
        {
            var ipAddress = HttpContext.GetServerVariable("HTTP_X_FORWARDED_FOR") ?? HttpContext.Connection.RemoteIpAddress?.ToString();
            var ipAddressWithoutPort = ipAddress?.Split(':')[0];
            APIResponse aPIResponse = new APIResponse();
             ipAddressWithoutPort = ipAddressWithoutPort==""?"::1":ipAddressWithoutPort;
            List<string> GeaLocationData = GetIpLocation(ipAddressWithoutPort);
            try
            {
                IActionResult response = Unauthorized();
                var user = AuthenticateUser(login);

                if (user != null)
                {
                    var tokenString = GenerateJSONWebToken();
                    login.usertoken = tokenString;
                    login.city = GeaLocationData?[0]??"";
                    login.country = GeaLocationData?[1]??"";

                }
                else
                {
                  
                    aPIResponse.Status = "Error";
                    aPIResponse.Message = "Login Failed";
                    return aPIResponse;
                }


                aPIResponse.Data = login;
                aPIResponse.Status = "Success";
                aPIResponse.Message = "Login Successful";

            }
            catch (Exception e)
            {
           
                aPIResponse.Status = "Error";
                aPIResponse.Message = "Login Failed";
            }
            return aPIResponse;
        }

        [NonAction]
        private string GenerateJSONWebToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              null,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [NonAction]
        private User AuthenticateUser(UserModel login)
        {
            User user = null;

            user = _dbcontext.Users.Where(x=>x.Password == login.Password && x.Email == login.Email).FirstOrDefault();
            return user;
        }
    

        [NonAction]
        public static List<String> GetIpLocation(String ip)
        {
            List<String> GealocationData = new List<string>();
            IPAddress address = IPAddress.Parse(ip);

            if (IPAddress.IsLoopback(address))
            {
                return null;
            }

            using (var reader = new DatabaseReader(Directory.GetCurrentDirectory() + "/GeoLite2-City.mmdb"))
            {
                var response = reader.City(ip);
                var responsecountry = reader.Country(ip);

                GealocationData.Add(response.City.Name??"");
          
                GealocationData.Add(responsecountry.Country.Name??"");
            }

            return GealocationData;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<APIResponse> Register([FromForm] UserRegisterModel user)
        {
            APIResponse aPIResponse = new APIResponse();
            if(user != null)
            {
                try
                {
                    var fileresponse = new Dictionary<string, string>();
                    List<FileDetails> dbfilelist = new List<FileDetails>();
                 
                    if(user.FileDetails != null||user.FileDetails.Count>0)
                    {
                        foreach (var file in user.FileDetails)
                        {

                            var folderName = Path.Combine("Resources", "Files");
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                            if (!Directory.Exists(filePath))
                            {
                                Directory.CreateDirectory(filePath);
                            }
                            var fileName = file.FileName;
                            var fullPath = Path.Combine(filePath, fileName);
                            var dbPath = Path.Combine(folderName, fileName);

                            if (!System.IO.File.Exists(fullPath))
                            {
                                using var memoryStream = new MemoryStream();
                                await file.CopyToAsync(memoryStream);
                                await System.IO.File.WriteAllBytesAsync(fullPath, memoryStream.ToArray());
                                
                                
                                dbfilelist.Add(new FileDetails { FileName = fileName, FileType = user.Filetype,FilePath=dbPath });


                                fileresponse.Add(fileName, "File uploaded successfully.");

                            }
                            else
                            {
                                fileresponse.Add(fileName, "File already exists");
                            }

                        }
                       
                    }
                    else
                    {

                        aPIResponse.Status = "Error";
                        aPIResponse.Message = "Please Upload Files";
                        return aPIResponse;
                    }

                  User dbuser= new User
                    {
                        Email = user.Email,
                        Password = user.Password,
                        IsIndivitual=user.IsIndivitual,
                        Name=user.Name,
                        City=user.City,
                        Country=user.Country,
                        FileDetails= dbfilelist,
                        IsProvider=user.IsProvider,
                        Street=user.Street,
                        PostalCode  =user.PostalCode



                    };


                    _dbcontext.Users.Add(dbuser);
                    _dbcontext.SaveChanges();

                    aPIResponse.Status = "Success";
                    aPIResponse.Message = "Registration Successful";


                }
                catch (Exception)
                {
                    aPIResponse.Status = "Error";
                    aPIResponse.Message = "Registration Failed";
                }

            }
            return aPIResponse;
        }
    }



}
