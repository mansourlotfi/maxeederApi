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
    public class LogoController:BaseApiController
    {
     
        private readonly StoreContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostEnvironment;

        public LogoController(StoreContext context, IMapper mapper, IWebHostEnvironment hostEnvironment)
        {
            _mapper = mapper;
            this._hostEnvironment = hostEnvironment;
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<PagedList<Logo>>> GetLogos([FromQuery] PaginationParams? paginationParams)
        {
            // return await _context.Logos.ToListAsync();
            var query = _context.Logos.Select(x => new Logo()
            {
                Id = x.Id,
                Name = x.Name,
                Link = x.Link,
                PictureUrl = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.PictureUrl),
                Priority = x.Priority,
                IsActive=x.IsActive,
                NameEn=x.NameEn,
            }).AsQueryable();

            var logo = await PagedList<Logo>.ToPagedList(query, paginationParams.PageNumber, paginationParams.PageSize);

            Response.AddPaginationHeader(logo.MetaData);

            return logo;

        }

        [HttpGet("{id}", Name = "GetLogo")]
        public async Task<ActionResult<Logo>> GetLogo(int id)
        {
            var logo = await _context.Logos.FindAsync(id);

            if (logo == null) return NotFound();

            return logo;

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Logo>> Createlogo([FromForm] LogoDto logoDto)
        {
            var existing = await _context.Logos.FirstOrDefaultAsync(x => x.Priority == logoDto.Priority);
            if (existing != null) return BadRequest(new ProblemDetails { Title = "Item with this priority exist" });

            var logo = _mapper.Map<Logo>(logoDto);

            if (logoDto.File != null)
            {
                var fileName = await WriteFile(logoDto.File);

                if (fileName.Length == 0)
                    return BadRequest(new ProblemDetails { Title = "Problem uploading new image" });

                logo.PictureUrl = fileName;

            }

            _context.Logos.Add(logo);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok(logo);

            return BadRequest(new ProblemDetails { Title = "Problem creating new logo" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult<Logo>> UpdateLogo([FromForm] UpdateLogoDto updateLogoDto)
        {
            var logo = await _context.Logos.FindAsync(updateLogoDto.Id);

            if (logo == null) return NotFound();

            var existing = await _context.Logos.FirstOrDefaultAsync(x => x.Priority == updateLogoDto.Priority && updateLogoDto.Priority != logo.Priority);
            if (existing != null) return BadRequest(new ProblemDetails { Title = "Item with this priority exist" });

            if (updateLogoDto.File != null)
            {
                var fileName = await WriteFile(updateLogoDto.File);

                if (fileName.Length == 0)
                    return BadRequest(new ProblemDetails { Title = "Problem uploading new image" });

                logo.PictureUrl = fileName;

            }
            else
            {
                logo.PictureUrl = logo.PictureUrl;

            }

            _mapper.Map(updateLogoDto, logo);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok(logo);

            return BadRequest(new ProblemDetails { Title = "Problem updating logo" });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteLogo(int id)
        {
            var logo = await _context.Logos.FindAsync(id);

            if (logo == null) return NotFound();

            if (!string.IsNullOrEmpty(logo.PictureUrl))
            {

                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "..//var//lib//Upload//Images", logo.PictureUrl);
                if (System.IO.File.Exists(filepath))
                    System.IO.File.Delete(filepath);
            }


            _context.Logos.Remove(logo);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem deleting logo" });
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateMultipleItems")]
        public async Task<ActionResult> UpdateLogos([FromBody] List<int> ids)
        {

            if (ids == null || ids.Count == 0) return NotFound();

            foreach (var id in ids)
            {
                var item = await _context.Logos.FindAsync(id);

                if (item == null) return NotFound();

                item.IsActive = !item.IsActive;

            }

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem updating Logos" });
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("DeleteMultipleItems")]
        public async Task<ActionResult> DeleteLogos([FromBody] List<int> ids)
        {

            if (ids == null || ids.Count == 0) return NotFound();

            foreach (var id in ids)
            {

                var item = await _context.Logos.FindAsync(id);

                if (item == null) return NotFound();

                if (!string.IsNullOrEmpty(item.PictureUrl))
                {

                    var filepath = Path.Combine(Directory.GetCurrentDirectory(), "..//var//lib//Upload//Images", item.PictureUrl);
                    if (System.IO.File.Exists(filepath))
                        System.IO.File.Delete(filepath);
                }


                _context.Logos.Remove(item);
            }


            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem Logos" });
        }

        private async Task<string> WriteFile(IFormFile file)
        {
            if (_hostEnvironment.IsDevelopment())
            {
                var fileName = DateTime.Now.Ticks.ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(_hostEnvironment.ContentRootPath, "..\\var\\lib\\Upload\\Images", fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                return fileName;
            }
            else
            {

                //string newfile = DateTime.Now.Ticks.ToString() + Path.GetExtension(file.FileName);
                //using (FileStream fs = new FileStream(newfile, FileMode.Create, FileAccess.Write,
                //    FileShare.None, 4096, useAsync: true))
                //{
                //    await file.CopyToAsync(fs);
                //}
                //return newfile;

                var fileName = DateTime.Now.Ticks.ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(_hostEnvironment.ContentRootPath, "..//var//lib//Upload//Images", fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                return fileName;
            }



        }

    
    }
}
