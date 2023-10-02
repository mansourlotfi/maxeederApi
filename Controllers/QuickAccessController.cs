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
    public class QuickAccessController:BaseApiController
    {
        
        private readonly StoreContext _context;
        private readonly IMapper _mapper;

        public QuickAccessController(StoreContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<QuickAccess>>> GetQuickAccessItems([FromQuery] PaginationParams? paginationParams)
        {
            // return await _context.QuickAccess.ToListAsync();
            var query = _context.QuickAccess.Select(x => new QuickAccess()
            {
                Id = x.Id,
                Title = x.Title,
                Link = x.Link,
                Priority = x.Priority,
            }).AsQueryable();

            var menu = await PagedList<QuickAccess>.ToPagedList(query, paginationParams.PageNumber, paginationParams.PageSize);

            Response.AddPaginationHeader(menu.MetaData);

            return menu;

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<QuickAccess>> CreateQuickAccess([FromForm] QuickAccessDto quickAccessDto)
        {
            var existing = await _context.QuickAccess.FirstOrDefaultAsync(x => x.Priority == quickAccessDto.Priority);
            if (existing != null) return BadRequest(new ProblemDetails { Title = "Item with this priority exist" });

            var menu = _mapper.Map<QuickAccess>(quickAccessDto);

            _context.QuickAccess.Add(menu);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok(menu);

            return BadRequest(new ProblemDetails { Title = "Problem creating new QuickAccess menu item" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult<QuickAccess>> UpdateQuickAccess([FromForm] QuickAccess updateQuickAccessDto)
        {
            var menu = await _context.QuickAccess.FindAsync(updateQuickAccessDto.Id);


            if (menu == null) return NotFound();

            var existing = await _context.QuickAccess.FirstOrDefaultAsync(x => x.Priority == updateQuickAccessDto.Priority && updateQuickAccessDto.Priority != menu.Priority);
            if (existing != null) return BadRequest(new ProblemDetails { Title = "Item with this priority exist" });

            _mapper.Map(updateQuickAccessDto, menu);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok(menu);

            return BadRequest(new ProblemDetails { Title = "Problem updating QuickAccess menu item" });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteQuickAccessItem(int id)
        {
            var mainMenu = await _context.QuickAccess.FindAsync(id);

            if (mainMenu == null) return NotFound();

            _context.QuickAccess.Remove(mainMenu);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem deleting QuickAccess item" });
        }
    }
}
