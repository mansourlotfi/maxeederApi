using AutoMapper;
using ecommerceApi.Data;
using ecommerceApi.DTOs;
using ecommerceApi.Entities;
using ecommerceApi.Extensions;
using ecommerceApi.RequestHelpers;
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
        public async Task<ActionResult<PagedList<Department>>> GetDepartments([FromQuery] PaginationParams? paginationParams)
        {
            var query =  _context.Departments.AsQueryable();
            var items = await PagedList<Department>.ToPagedList(query, paginationParams.PageNumber, paginationParams.PageSize);

            Response.AddPaginationHeader(items.MetaData);

            return items;
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


        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateMultipleItems")]
        public async Task<ActionResult> UpdateDepartments([FromBody] List<int> ids)
        {

            if (ids == null || ids.Count == 0) return NotFound();

            foreach (var id in ids)
            {
                var item = await _context.Departments.FindAsync(id);

                if (item == null) return NotFound();

                item.IsActive = !item.IsActive;

            }

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem updating Departments" });
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("DeleteMultipleItems")]
        public async Task<ActionResult> DeleteDepartments([FromBody] List<int> ids)
        {

            if (ids == null || ids.Count == 0) return NotFound();

            foreach (var id in ids)
            {

                var item = await _context.Departments.FindAsync(id);

                if (item == null) return NotFound();

                _context.Departments.Remove(item);
            }


            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem deleting Departments" });
        }
    }
}
