using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
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


        [Route("GetAllUsers")]
        [HttpGet]
        public async Task<IEnumerable<PPW.Models.User>> GetAllUsers()
        {
            _context = new PPW.Models.ProgramacionWebContext();
            var users = await _context.Users.Select(s =>
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
            ).ToListAsync();
            return users;
        }

        [Route("SetUser")]
        [HttpPost]
        public async Task<PPW.Models.generalResult> CreateUser([FromBody] PPW.Models.User user)
        {
            _context = new PPW.Models.ProgramacionWebContext();
            var generalResult = new PPW.Models.generalResult
            {
                Result = false
            };
            try
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                generalResult.Result = true;
            }
            catch (Exception ex)
            {
                generalResult.Result = false;
                generalResult.ErrorMessage = ex.Message;
            }
            return generalResult;
        }

        [Route("UpdateUser")]
        [HttpPost]
        public async Task<PPW.Models.generalResult> ModifyUser([FromBody] PPW.Models.User user)
        {
            _context = new PPW.Models.ProgramacionWebContext();
            var generalResult = new PPW.Models.generalResult
            {
                Result = false
            };
            try
            {
                /*var usuario = await _context.Users.Select(s =>
                new User
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
                var newUser = new User
                {
                    //lo que se necesita para realizar el update es la llave primaria, en este caso el ID.
                    Id = usuario.Id,
                    Username = usuario.Username,
                    Password = user.Password,
                    StatusId = user.StatusId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Phone = user.Phone,
                    Birthdate = user.Birthdate,
                    Email = user.Email,
                    Genero = user.Genero,
                    FecTransac = usuario.FecTransac
                };*/
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                generalResult.Result = false;
                generalResult.ErrorMessage = ex.Message;
            }
            return generalResult;
        }

        [Route("DeleteUser")]
        [HttpPost]
        public async Task<PPW.Models.generalResult> DeleteUser(string userName)
        {
            _context = new PPW.Models.ProgramacionWebContext();
            var generalResult = new PPW.Models.generalResult
            {
                Result = false
            };
            try
            {
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
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                generalResult.Result = false;
                generalResult.ErrorMessage = ex.Message;
            }
            return generalResult;
        }
    }
}
