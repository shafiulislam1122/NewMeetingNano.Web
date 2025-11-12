using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [Authorize(Roles = "Admin")]
        [HttpGet("Admin")]
        public IActionResult AdminPanel()
        {
            return Ok("Admin Access Granted");
        }

        [Authorize(Roles = "Employee")]
        [HttpGet("Employee")]
        public IActionResult EmployeePanel()
        {
            return Ok("Employee Access Granted");
        }
    }
}
