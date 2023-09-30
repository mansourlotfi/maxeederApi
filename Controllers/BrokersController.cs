using AutoMapper;
using ecommerceApi.Data;
using ecommerceApi.DTOs;
using ecommerceApi.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ecommerceApi.Controllers
{
    public class BrokersController : BaseApiController
    {
        private readonly StoreContext _context;
        private readonly IMapper _mapper;

        public BrokersController(StoreContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<Broker>> GetBrokers()
        {
            var brokers = await _context.Brokers.ToListAsync();
            if (brokers == null) return NotFound();
            return Ok(brokers);

        }
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}", Name = "GetBroker")]
         public async Task<ActionResult<Broker>> GetBrand(int id)
        {
            var broker = await _context.Brokers.FindAsync(id);

            if (broker == null) return NotFound();

            return broker;

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Broker>> CreateBroker([FromForm] CreateBrokerDto brokersDto)

        {
       
            var broker = _mapper.Map<Broker>(brokersDto);


            _context.Brokers.Add(broker);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return CreatedAtRoute("GetBroker", new { Id = broker.Id }, broker);

            return BadRequest(new ProblemDetails { Title = "Problem creating new broker" });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBroker(int id)
        {
            var broker = await _context.Brokers.FindAsync(id);

            if (broker == null) return NotFound();

            _context.Brokers.Remove(broker);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem deleting category" });
        }
    }
}
