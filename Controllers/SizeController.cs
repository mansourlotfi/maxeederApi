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
    public class SizeController:BaseApiController
    {
        private readonly StoreContext _context;
        private readonly IMapper _mapper;
        public SizeController(StoreContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;

        }

        [HttpGet]
        public async Task<ActionResult<PagedList<Size>>> GetSize([FromQuery] PaginationParams? paginationParams)
        {
            var query =  _context.Sizes.SearchSizes(paginationParams.SearchTerm).AsQueryable();
            var items = await PagedList<Size>.ToPagedList(query, paginationParams.PageNumber, paginationParams.PageSize);

            Response.AddPaginationHeader(items.MetaData);

            return items;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Size>> CreateSize([FromForm] CreateSizeDto createSizeDto)

        {

            var size = _mapper.Map<Size>(createSizeDto);

            _context.Sizes.Add(size);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok(size);

            return BadRequest(new ProblemDetails { Title = "Problem creating new size" });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSize(int id)
        {
            var size = await _context.Sizes.FindAsync(id);

            if (size == null) return NotFound();

            _context.Sizes.Remove(size);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem deleting size" });
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateMultipleItems")]
        public async Task<ActionResult> UpdateSizes([FromBody] List<int> ids)
        {

            if (ids == null || ids.Count == 0) return NotFound();

            foreach (var id in ids)
            {
                var item = await _context.Sizes.FindAsync(id);

                if (item == null) return NotFound();

                item.IsActive = !item.IsActive;

            }

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem updating sizes" });
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("DeleteMultipleItems")]
        public async Task<ActionResult> DeleteSizes([FromBody] List<int> ids)
        {

            if (ids == null || ids.Count == 0) return NotFound();

            foreach (var id in ids)
            {

                var item = await _context.Sizes.FindAsync(id);

                if (item == null) return NotFound();

                _context.Sizes.Remove(item);
            }


            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem deleting sizes" });
        }
    }
}
