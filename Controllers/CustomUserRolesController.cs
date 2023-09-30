using ecommerceApi.Data;
using ecommerceApi.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ecommerceApi.Controllers
{
    public class CustomUserRolesController:BaseApiController
    {
        private readonly StoreContext _context;

        public CustomUserRolesController(StoreContext context)
        {
            this._context = context;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<CustomUserRole>> GetUserRoles()
        {
            var userRoles = await _context.CustomUserRoles.ToListAsync();
            if (userRoles == null) return NotFound();
            return Ok(userRoles);

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<CustomUserRole>> CreateUserRole([FromForm] string name)

        {

            var userRoles = new CustomUserRole
            {
                Name = name
            };

            _context.CustomUserRoles.Add(userRoles);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok(userRoles);

            return BadRequest(new ProblemDetails { Title = "Problem creating new role" });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUserRole(int id)
        {
            var userRole = await _context.CustomUserRoles.FindAsync(id);

            if (userRole == null) return NotFound();

            _context.CustomUserRoles.Remove(userRole);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem deleting role" });
        }
    }
}
