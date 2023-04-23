using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private readonly IConfiguration configuration;

        public UsersController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        private PPW.Models.ProgramacionWebContext _context;

        [Route("GetValidationUser")]
        [HttpPost]
        public async Task<bool> GetValidationUser([FromBody] string UserName)
        {
            _context = new PPW.Models.ProgramacionWebContext();
            var user = await _context.Users.Select(s =>
            new PPW.Models.User
            {
                Id = s.Id,
                Username = s.Username,
                Password = s.Password,
                StatusId = s.StatusId,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Phone = s.Phone,
                Birthdate = s.Birthdate,
                Email = s.Email,
                Genero = s.Genero,
                FecTransac = s.FecTransac
            }
            ).FirstOrDefaultAsync(s => s.Username == UserName && s.StatusId == 1);
            return (user != null) ? true : false;
        }

        [Route("GetUser")]
        [HttpPost]
        public async Task<PPW.Models.User> GetUser([FromBody] string userName)
        {
            _context = new PPW.Models.ProgramacionWebContext();
            var user = await _context.Users.Select(s =>
            new PPW.Models.User
            {
                Id = s.Id,
                Username = s.Username,
                Password = s.Password,
                StatusId = s.StatusId,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Phone = s.Phone,
                Birthdate = s.Birthdate,
                Email = s.Email,
                Genero = s.Genero,
                FecTransac = s.FecTransac
            }
            ).FirstOrDefaultAsync(s => s.Username == userName && s.StatusId == 1);
            return user;
        }

        [Route("GetUserbyID")]
        [HttpPost]
        public async Task<PPW.Models.User> GetUserbyID([FromBody] int ID)
        {
            _context = new PPW.Models.ProgramacionWebContext();
            var user = await _context.Users.Select(s =>
            new PPW.Models.User
            {
                Id = s.Id,
                Username = s.Username,
                Password = s.Password,
                StatusId = s.StatusId,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Phone = s.Phone,
                Birthdate = s.Birthdate,
                Email = s.Email,
                Genero = s.Genero,
                FecTransac = s.FecTransac
            }
            ).FirstOrDefaultAsync(s => s.Id == ID && s.StatusId == 1);
            return user;
        }

        [Route("GetAllUsers")]
        [HttpPost]
        public async Task<IEnumerable<PPW.Models.User>> GetAllUsers([FromBody] int ID)
        {
            _context = new PPW.Models.ProgramacionWebContext();
            var activeUsers = new List<PPW.Models.User>();
            try
            {
                activeUsers = (from m in _context.Users
                                where m.StatusId == 1 && m.Id != ID
                                select new PPW.Models.User
                                {
                                    Id = m.Id,
                                    Username = m.Username,
                                    Password = m.Password,
                                    StatusId = m.StatusId,
                                    FirstName = m.FirstName,
                                    LastName = m.LastName,
                                    Phone = m.Phone,
                                    Birthdate = m.Birthdate,
                                    Email = m.Email,
                                    Genero = m.Genero,
                                    FecTransac = m.FecTransac
                                }).ToList();
            }
            catch (Exception ex)
            {
                activeUsers = null;
            }
            return activeUsers;
        }

        [Route("CreateUser")]
        [HttpPost]
        public async Task<bool> CreateUser([FromBody] PPW.Models.User user)
        {
            _context = new PPW.Models.ProgramacionWebContext();
            bool result;
            try
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                result = true;
            }
            catch (Exception ex)
            {
                var ErrorMessage = ex.Message;
                result = false;
            }
            return result;
        }

        [Route("UpdateUser")]
        [HttpPut]
        public async Task<bool> UpdateUser([FromBody] PPW.Models.User user)
        {
            _context = new PPW.Models.ProgramacionWebContext();
            bool result;
            try
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        [Route("DisableUser")]
        [HttpPut]
        public async Task<bool> DisableUser([FromBody] PPW.Models.User user)
        {
            _context = new PPW.Models.ProgramacionWebContext();
            bool result;
            try
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        private string CustomTokenJWT(string ApplicationName, DateTime token_expiration)
        {
            var _symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));
            var _signingCredentials = new SigningCredentials(_symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var _Header = new JwtHeader(_signingCredentials);
            var _Claims = new[] {
                //acá es lo que debemos de modificar las los parametros que vamos a utilizar.
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.NameId, ApplicationName)
            };
            var _Payload = new JwtPayload(
                    issuer: configuration["JWT:Issuer"],
                    audience: configuration["JWT:Audience"],
                    claims: _Claims,
                    notBefore: DateTime.Now, //cuanto va a durar el token
                    expires: token_expiration    // cuando va a expirar el token.
                );
            var _Token = new JwtSecurityToken(
                    _Header,
                    _Payload
                );
            return new JwtSecurityTokenHandler().WriteToken(_Token);
        }


    }
}
