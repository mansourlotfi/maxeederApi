using AutoMapper;
using ecommerceApi.Data;
using ecommerceApi.DTOs;
using ecommerceApi.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ecommerceApi.Controllers
{
    public class CeoOptimizationController:BaseApiController
    {
            private readonly StoreContext _context;
            private readonly IMapper _mapper;
            private readonly IWebHostEnvironment _hostEnvironment;
        public CeoOptimizationController(StoreContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;

        }

        [Authorize(Roles = "Admin")]
        [HttpPost()]
        public async Task<ActionResult<CeoOptimization>> CreateCeoOptimization([FromForm] CeoOptimizationDto ceoOptimizationDto) 
        {
            var previusValue = await _context.CeoOptimizations.FirstOrDefaultAsync();

            if(previusValue == null)
            {
                var newSetting = _mapper.Map<CeoOptimization>(ceoOptimizationDto);

                _context.CeoOptimizations.Add(newSetting);

                var resultNewSetting = await _context.SaveChangesAsync() > 0;

                if (resultNewSetting) return Ok(newSetting);
            }
            else
            {
                _mapper.Map(ceoOptimizationDto, previusValue);

                var result = await _context.SaveChangesAsync() > 0;

                if (result) return Ok(previusValue);
            }
            return BadRequest(new ProblemDetails { Title = "Problem creating new Setting" });

        }

        [HttpGet()]
        public async Task<ActionResult<CeoOptimization>> GetCeoOptimization()
        {
            var setting = _context.CeoOptimizations
           .FirstOrDefault();

            if (setting == null) return NotFound();

            return setting;
        }

    }
}
