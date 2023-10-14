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
    public class PartnersController:BaseApiController
    {
        private readonly StoreContext _context;
        private readonly IMapper _mapper;
        public PartnersController(StoreContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<Partner>>> GetPartners([FromQuery] PartnerParams? partnerParams)
        {
            // return await _context.Partners.ToListAsync();
            var query = _context.Partners.Select(x => new Partner()
            {
                Id = x.Id,
                Title = x.Title,
                CEO = x.CEO,
                State = x.State,
                City=x.City,
                Tel=x.Tel,
                Long=x.Long,
                Lat=x.Lat,
                IsActive=x.IsActive,
                TitleEn=x.TitleEn,
                CeoEn=x.CeoEn,
                CityEn=x.CityEn,
                StateEn=x.StateEn,
            })
            .Search(partnerParams.SearchTerm)
            .Filter(partnerParams.State)
            .AsQueryable();

            var partners = await PagedList<Partner>.ToPagedList(query, partnerParams.PageNumber, partnerParams.PageSize);

            Response.AddPaginationHeader(partners.MetaData);

            return partners;

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Partner>> CreatePartner([FromForm] PartnerDto partnerDto)
        {

            var partner = _mapper.Map<Partner>(partnerDto);

            _context.Partners.Add(partner);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok(partner);

            return BadRequest(new ProblemDetails { Title = "Problem creating new partner" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult<Partner>> UpdatePartner([FromForm] UpdatePartnerDto updatePartnerDto)
        {
            var partner = await _context.Partners.FindAsync(updatePartnerDto.Id);


            if (partner == null) return NotFound();


            _mapper.Map(updatePartnerDto, partner);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok(partner);

            return BadRequest(new ProblemDetails { Title = "Problem updating partner" });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePartner(int id)
        {
            var partner = await _context.Partners.FindAsync(id);

            if (partner == null) return NotFound();

            _context.Partners.Remove(partner);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem deleting partner" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateMultipleItems")]
        public async Task<ActionResult> UpdatePartners([FromBody] List<int> ids)
        {

            if (ids == null || ids.Count == 0) return NotFound();

            foreach (var id in ids)
            {
                var item = await _context.Partners.FindAsync(id);

                if (item == null) return NotFound();

                item.IsActive = !item.IsActive;

            }

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem updating Partners" });
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("DeleteMultipleItems")]
        public async Task<ActionResult> DeletePartners([FromBody] List<int> ids)
        {

            if (ids == null || ids.Count == 0) return NotFound();

            foreach (var id in ids)
            {

                var item = await _context.Partners.FindAsync(id);

                if (item == null) return NotFound();

                _context.Partners.Remove(item);
            }


            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem deleting Partners" });
        }
    }
}
