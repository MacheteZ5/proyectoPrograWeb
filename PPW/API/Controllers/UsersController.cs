using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Models;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private ProgramacionWebContext _context;

        [Route("GetValidationUser")]
        [HttpPost]
        public async Task<bool> GetValidationUser([FromBody] string UserName)
        {
            _context = new ProgramacionWebContext();
            var user = await _context.Users.Select(s =>
            new Modelos.User
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
        public async Task<Modelos.User> GetUser([FromBody] string userName)
        {
            _context = new ProgramacionWebContext();
            var user = await _context.Users.Select(s =>
            new Modelos.User
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
        public async Task<Modelos.User> GetUserbyID([FromBody] int ID)
        {
            _context = new ProgramacionWebContext();
            var user = await _context.Users.Select(s =>
            new Modelos.User
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
        public async Task<IEnumerable<Modelos.User>> GetAllUsers([FromBody] int ID)
        {
            _context = new ProgramacionWebContext();
            var activeUsers = new List<Modelos.User>();
            try
            {
                activeUsers = (from m in _context.Users
                                where m.StatusId == 1 && m.Id != ID
                                select new Modelos.User
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
        public async Task<bool> CreateUser([FromBody] Modelos.User user)
        {
            _context = new ProgramacionWebContext();
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
        public async Task<bool> UpdateUser([FromBody] Modelos.User user)
        {
            _context = new ProgramacionWebContext();
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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("DisableUser")]
        [HttpPut]
        public async Task<bool> DisableUser([FromBody] Modelos.User user)
        {
            _context = new ProgramacionWebContext();
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
    }
}
