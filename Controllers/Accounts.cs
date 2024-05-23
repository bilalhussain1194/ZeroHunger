using Kill_hunger.Data;
using Kill_hunger.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
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
        public IActionResult Login([FromBody] UserModel login)
        {
            IActionResult response = Unauthorized();
            var user = AuthenticateUser(login);

            if (user != null)
            {
                var tokenString = GenerateJSONWebToken(user);
                response = Ok(new { token = tokenString });
            }

            return response;
        }

        private string GenerateJSONWebToken(UserModel userInfo)
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

        private UserModel AuthenticateUser(UserModel login)
        {
            UserModel user = null;

            //Validate the User Credentials
            //Demo Purpose, I have Passed HardCoded User Information
            if (login.UserName == "Jignesh")
            {
                user = new UserModel { UserName = "Jignesh Trivedi", Password = "test.btest@gmail.com" };
            }
            return user;
        }


        public IActionResult Register([FromBody] UserRegisterModel user)
        {



            if(user != null)
            {
                try
                {
                    foreach (var file in user.FileDetails)
                    {
                    
                        var folderName = Path.Combine("Resources", "Files");
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                        if (!Directory.Exists(filePath))
                        {
                            Directory.CreateDirectory(filePath);
                        }
                        var fileName=file.FileName;
                        var fullPath=Path.Combine(filePath, fileName);
                        var dbPath=Path.Combine(folderName, fileName);
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        };
                  


                    }

                }
                catch (Exception)
                {
                    throw;
                }

            }
            return BadRequest();
        }
    }



}
