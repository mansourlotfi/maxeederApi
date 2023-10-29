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
    public class UsageController:BaseApiController
    {
        private readonly StoreContext _context;
        private readonly IMapper _mapper;

        public UsageController(StoreContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<Usage>>> GetUsage([FromQuery] PaginationParams? paginationParams)
        {
            var query =  _context.Usages.AsQueryable();
            var items = await PagedList<Usage>.ToPagedList(query, paginationParams.PageNumber, paginationParams.PageSize);

            Response.AddPaginationHeader(items.MetaData);

            return items;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Usage>> CreateUsage([FromForm] CreateUsageDto createUsageDto)

        {

            var usage = _mapper.Map<Usage>(createUsageDto);

            _context.Usages.Add(usage);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok(usage);

            return BadRequest(new ProblemDetails { Title = "Problem creating new usage" });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUsage(int id)
        {
            var usage = await _context.Usages.FindAsync(id);

            if (usage == null) return NotFound();

            _context.Usages.Remove(usage);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem deleting usage" });
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateMultipleItems")]
        public async Task<ActionResult> UpdateUsages([FromBody] List<int> ids)
        {

            if (ids == null || ids.Count == 0) return NotFound();

            foreach (var id in ids)
            {
                var item = await _context.Usages.FindAsync(id);

                if (item == null) return NotFound();

                item.IsActive = !item.IsActive;

            }

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem updating Usages" });
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("DeleteMultipleItems")]
        public async Task<ActionResult> DeleteUsages([FromBody] List<int> ids)
        {

            if (ids == null || ids.Count == 0) return NotFound();

            foreach (var id in ids)
            {

                var item = await _context.Usages.FindAsync(id);

                if (item == null) return NotFound();

                _context.Usages.Remove(item);
            }


            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem deleting Usages" });
        }
    }
}
