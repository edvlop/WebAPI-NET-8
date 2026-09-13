using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI_NET_8.Custom;
using WebAPI_NET_8.Models;
using WebAPI_NET_8.Models.DTOs;
using Microsoft.AspNetCore.Authorization;



namespace WebAPI_NET_8.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class AccesoController : ControllerBase
    {
        private readonly DbpruebaContext _dbPruebaContext;
        private readonly Utils _utils;

        public AccesoController(DbpruebaContext dbPruebaContext, Utils utils)
        {
            _dbPruebaContext = dbPruebaContext;
            _utils = utils;
        }
        [HttpPost]
        [Route("SignUp")]
        public async Task<IActionResult> SignUp(UsuarioDTO objDto)
        {
            var userModel = new Usuario
            {
                Nombre = objDto.Nombre,
                Correo = objDto.Correo,
                Clave = _utils.encryptPassword(objDto.Clave)
            };
            try
            {
                await _dbPruebaContext.AddAsync(userModel);
                await _dbPruebaContext.SaveChangesAsync();

                if (userModel.IdUsuario != 0)
                {
                    return StatusCode(StatusCodes.Status200OK, new { isSuccess = true });

                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, new { isSuccess = false });
                }

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }


        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult>Login(LoginDTO objLoginDto)
        {
            var userFound = await _dbPruebaContext.Usuarios
                                  .Where(u=>u.Correo==objLoginDto.Correo &&
                                         u.Clave==_utils.encryptPassword(objLoginDto.Clave)
                                  ).FirstOrDefaultAsync();
            if (userFound == null)
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = false, token = "" });
            else
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, token = _utils.generateJWT(userFound) });
        }
    }
}
