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
    public class MainMenuController:BaseApiController
    {
        private readonly StoreContext _context;
        private readonly IMapper _mapper;

        public MainMenuController(StoreContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<MainMenu>>> GetSocialMainMenuItems([FromQuery] PaginationParams? paginationParams)
        {
            // return await _context.MainMenuItems.ToListAsync();
            var query = _context.MainMenuItems.Select(x => new MainMenu()
            {
                Id = x.Id,
                Title = x.Title,
                Link = x.Link,
                Priority = x.Priority,
                IsActive=x.IsActive,
                TitleEn=x.TitleEn,
            }).SearchMainMenu(paginationParams.SearchTerm).AsQueryable();

            var menu = await PagedList<MainMenu>.ToPagedList(query, paginationParams.PageNumber, paginationParams.PageSize);

            Response.AddPaginationHeader(menu.MetaData);

            return menu;

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<MainMenu>> CreateMenu([FromForm] MainMenuDto mainMenuDto)
        {

            var existing = await _context.MainMenuItems.FirstOrDefaultAsync(x => x.Priority == mainMenuDto.Priority);
            if (existing != null) return BadRequest(new ProblemDetails { Title = "Item with this priority exist" });

            var mainMenu = _mapper.Map<MainMenu>(mainMenuDto);

            _context.MainMenuItems.Add(mainMenu);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok(mainMenu);

            return BadRequest(new ProblemDetails { Title = "Problem creating new menu item" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult<MainMenu>> UpdateMainMenu([FromForm] UpdateMainMenuDto updateMainMenuDto)
        {
            var mainMenu = await _context.MainMenuItems.FindAsync(updateMainMenuDto.Id);


            if (mainMenu == null) return NotFound();

            var existing = await _context.MainMenuItems.FirstOrDefaultAsync(x => x.Priority == updateMainMenuDto.Priority && updateMainMenuDto.Priority != mainMenu.Priority);
            if (existing != null) return BadRequest(new ProblemDetails { Title = "Item with this priority exist" });

            _mapper.Map(updateMainMenuDto, mainMenu);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok(mainMenu);

            return BadRequest(new ProblemDetails { Title = "Problem updating main menu" });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMainMenuItem(int id)
        {
            var mainMenu = await _context.MainMenuItems.FindAsync(id);

            if (mainMenu == null) return NotFound();

            _context.MainMenuItems.Remove(mainMenu);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem deleting main menu items" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateMultipleItems")]
        public async Task<ActionResult> UpdateMenus([FromBody] List<int> ids)
        {

            if (ids == null || ids.Count == 0) return NotFound();

            foreach (var id in ids)
            {
                var item = await _context.MainMenuItems.FindAsync(id);

                if (item == null) return NotFound();

                item.IsActive = !item.IsActive;

            }

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem updating Main Menu" });
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("DeleteMultipleItems")]
        public async Task<ActionResult> DeleteMainMenus([FromBody] List<int> ids)
        {

            if (ids == null || ids.Count == 0) return NotFound();

            foreach (var id in ids)
            {

                var item = await _context.MainMenuItems.FindAsync(id);

                if (item == null) return NotFound();


                _context.MainMenuItems.Remove(item);
            }


            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem deleting Main Menus" });
        }

    }
}
