using AutoMapper;
using ecommerceApi.Data;
using ecommerceApi.DTOs;
using ecommerceApi.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ecommerceApi.Controllers
{
    public class MessagesController:BaseApiController
    {
        private readonly StoreContext _context;
        private readonly IMapper _mapper;

        public MessagesController(StoreContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<Message>> GetMessages()
        {
            var meessages = await _context.Messages.ToListAsync();
            if (meessages == null) return NotFound();
            return Ok(meessages);
        }

   
        [HttpPost]
        public async Task<ActionResult<Message>> CreateMessage([FromForm] CreateMessageDto createMessageDto)

        {

            var message = _mapper.Map<Message>(createMessageDto);
                
            message.AddedDate = DateTime.Now;

            _context.Messages.Add(message);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok(message);

            return BadRequest(new ProblemDetails { Title = "Problem creating new department" });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id)
        {
            var message = await _context.Messages.FindAsync(id);

            if (message == null) return NotFound();

            _context.Messages.Remove(message);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem deleting message" });
        }
    }
}
