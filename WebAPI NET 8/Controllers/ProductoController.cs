using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI_NET_8.Models;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI_NET_8.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly DbpruebaContext _dbPruebaContext;
        public ProductoController(DbpruebaContext dbPruebaContext)
        {
            _dbPruebaContext = dbPruebaContext;
        }
        [HttpGet]
        [Route("ProductList")]
        public async Task<IActionResult> ProductList()
        {
            var productList = await _dbPruebaContext.Productos.ToListAsync();
            return StatusCode(StatusCodes.Status200OK, new { value = productList });
        }

    }
}
