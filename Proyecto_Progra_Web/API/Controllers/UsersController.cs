using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Progra_Web.Models;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private Proyecto_Progra_Web.Models.ProgramacionWebContext _context;

        [Route("GetValidationUser")]
        [HttpPost]
        public async Task<bool> GetValidationUser([FromBody] string UserName)
        {
            _context = new Proyecto_Progra_Web.Models.ProgramacionWebContext();
            var user = await _context.Users.Select(s =>
            new Proyecto_Progra_Web.Models.User
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
        public async Task<Proyecto_Progra_Web.Models.User> GetUser([FromBody] string userName)
        {
            _context = new Proyecto_Progra_Web.Models.ProgramacionWebContext();
            var user = await _context.Users.Select(s =>
            new Proyecto_Progra_Web.Models.User
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
        public async Task<Proyecto_Progra_Web.Models.User> GetUserbyID([FromBody] int ID)
        {
            _context = new Proyecto_Progra_Web.Models.ProgramacionWebContext();
            var user = await _context.Users.Select(s =>
            new Proyecto_Progra_Web.Models.User
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
        [HttpGet]
        public async Task<IEnumerable<Proyecto_Progra_Web.Models.User>> GetAllUsers()
        {
            _context = new Proyecto_Progra_Web.Models.ProgramacionWebContext();
            var user = await _context.Users.Select(s =>
            new Proyecto_Progra_Web.Models.User
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
            return user;
        }

        [Route("CreateUser")]
        [HttpPost]
        public async Task<bool> CreateUser([FromBody] Proyecto_Progra_Web.Models.User user)
        {
            _context = new Proyecto_Progra_Web.Models.ProgramacionWebContext();
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
        public async Task<bool> UpdateUser([FromBody] Proyecto_Progra_Web.Models.User user)
        {
            _context = new Proyecto_Progra_Web.Models.ProgramacionWebContext();
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
        public async Task<bool> DisableUser([FromBody] Proyecto_Progra_Web.Models.User user)
        {
            _context = new Proyecto_Progra_Web.Models.ProgramacionWebContext();
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
