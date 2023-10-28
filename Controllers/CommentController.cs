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
    public class CommentController:BaseApiController
    {
        private readonly StoreContext _context;
        private readonly IMapper _mapper;
        public CommentController(StoreContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<Comment>>> GetComments([FromQuery] PaginationParams? paginationParams)
        {
            var query = _context.Comments.Select(x=>new Comment()
            {
                Id = x.Id,
                Email = x.Email,
                IsActive = x.IsActive,
                Name = x.Name,
                Text=x.Text,   
            }).AsQueryable();

            var comments = await PagedList<Comment>.ToPagedList(query, paginationParams.PageNumber, paginationParams.PageSize);

            Response.AddPaginationHeader(comments.MetaData);

            return comments;

        }

        [HttpPost]
        public async Task<ActionResult<Comment>> CreateComment([FromForm] CreateComment createComment)
        {
 
            var comment = _mapper.Map<Comment>(createComment);

            _context.Comments.Add(comment);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok(comment);

            return BadRequest(new ProblemDetails { Title = "Problem creating new comment" });
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);

            if (comment == null) return NotFound();

            _context.Comments.Remove(comment);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem deleting comment" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateMultipleItems")]
        public async Task<ActionResult> UpdateComments([FromBody] List<int> ids)
        {

            if (ids == null || ids.Count == 0) return NotFound();

            foreach (var id in ids)
            {
                var item = await _context.Comments.FindAsync(id);

                if (item == null) return NotFound();

                item.IsActive = !item.IsActive;

            }

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem updating comments" });
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("DeleteMultipleItems")]
        public async Task<ActionResult> DeleteArtists([FromBody] List<int> ids)
        {

            if (ids == null || ids.Count == 0) return NotFound();

            foreach (var id in ids)
            {

                var item = await _context.Comments.FindAsync(id);

                if (item == null) return NotFound();


                _context.Comments.Remove(item);
            }


            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem deleting comments" });
        }
    }
}
