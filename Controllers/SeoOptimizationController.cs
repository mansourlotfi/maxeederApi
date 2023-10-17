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
    public class SeoOptimizationController:BaseApiController
    {
            private readonly StoreContext _context;
            private readonly IMapper _mapper;
        public SeoOptimizationController(StoreContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;

        }


        [HttpGet]
        public async Task<ActionResult<PagedList<SeoOptimization>>> GetPageItems([FromQuery] SeoOptParams? ceoOptParams)
        {
            // return await _context.PageItems.ToListAsync();
            var query = _context.CeoOptimizations.Where(x => x.Page == ceoOptParams.Page).AsQueryable();

            var items = await PagedList<SeoOptimization>.ToPagedList(query, ceoOptParams.PageNumber, ceoOptParams.PageSize);

            Response.AddPaginationHeader(items.MetaData);

            return items;

        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<SeoOptimization>>  CreateCeoOptimization([FromQuery] SeoOptimizationDto ceoOptimizationDto)
        {
            var existing = await _context.CeoOptimizations.FirstOrDefaultAsync(x => x.Priority == ceoOptimizationDto.Priority);
            if (existing != null) return BadRequest(new ProblemDetails { Title = "Item with this priority exist" });

            var item = _mapper.Map<SeoOptimization>(ceoOptimizationDto);

            
            _context.CeoOptimizations.Add(item);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok(item);

            return BadRequest(new ProblemDetails { Title = "Problem creating new Item" });
        }


        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult<SeoOptimization>> UpdateCeoItem([FromForm] UpdateSeoOptimizationDto updateCeoOptimizationDto)
        {
            var item = await _context.CeoOptimizations.FindAsync(updateCeoOptimizationDto.Id);


            if (item == null) return NotFound();

            var existing = await _context.CeoOptimizations.FirstOrDefaultAsync(x => x.Priority == updateCeoOptimizationDto.Priority && updateCeoOptimizationDto.Priority != item.Priority);
            if (existing != null) return BadRequest(new ProblemDetails { Title = "Item with this priority exist" });

        

            _mapper.Map(updateCeoOptimizationDto, item);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok(item);

            return BadRequest(new ProblemDetails { Title = "Problem updating item" });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItem(int id)
        {
            var item = await _context.CeoOptimizations.FindAsync(id);

            if (item == null) return NotFound();

            _context.CeoOptimizations.Remove(item);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem deleting item" });
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateMultipleItems")]
        public async Task<ActionResult> UpdateCeoItemsList([FromBody] List<int> ids)
        {

            if (ids == null || ids.Count == 0) return NotFound();

            foreach (var id in ids)
            {
                var item = await _context.CeoOptimizations.FindAsync(id);

                if (item == null) return NotFound();

                item.IsActive = !item.IsActive;

            }

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem updating ceo items" });
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("DeleteMultipleItems")]
        public async Task<ActionResult> DeleteCeoItemsList([FromBody] List<int> ids)
        {

            if (ids == null || ids.Count == 0) return NotFound();

            foreach (var id in ids)
            {

                var item = await _context.CeoOptimizations.FindAsync(id);

                if (item == null) return NotFound();

              
                _context.CeoOptimizations.Remove(item);
            }


            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem deleting ceo Items" });
        }


    }
}
