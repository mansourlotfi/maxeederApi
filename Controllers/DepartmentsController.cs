using AutoMapper;
using ecommerceApi.Data;
using ecommerceApi.DTOs;
using ecommerceApi.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ecommerceApi.Controllers
{
    public class DepartmentsController:BaseApiController
    {
        private readonly StoreContext _context;
        private readonly IMapper _mapper;
        public DepartmentsController(StoreContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<Department>> GetDepartments()
        {
            var department = await _context.Departments.ToListAsync();
            if (department == null) return NotFound();
            return Ok(department);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Department>> CreateDepartment([FromForm] CreateDepartmentDto createDepartmentDto)

        {

            var department = _mapper.Map<Department>(createDepartmentDto);

            _context.Departments.Add(department);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok(department);

            return BadRequest(new ProblemDetails { Title = "Problem creating new department" });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDepartment(int id)
        {
            var department = await _context.Departments.FindAsync(id);

            if (department == null) return NotFound();

            _context.Departments.Remove(department);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem deleting department" });
        }
    }
}
