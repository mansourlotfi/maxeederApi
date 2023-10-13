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
    public class SlidesController:BaseApiController
    {
        private readonly StoreContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostEnvironment;
        public SlidesController(StoreContext context, IMapper mapper, IWebHostEnvironment hostEnvironment)
        {
            _mapper = mapper;
            this._hostEnvironment = hostEnvironment;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<Slide>>> GetSlides([FromQuery] SlideParams? slideParams)
        {
            // return await _context.Products.ToListAsync();
            var query = _context.Slides.Where(x=>x.Page == slideParams.Page).Select(x => new Slide()
            {
                Id = x.Id,
                Name = x.Name,
                Link = x.Link,
                PictureUrl = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.PictureUrl),
                Priority = x.Priority,
                Page=x.Page,
                IsActive=x.IsActive,
                NameEn=x.NameEn,
            }).AsQueryable();

            var slides = await PagedList<Slide>.ToPagedList(query, slideParams.PageNumber, slideParams.PageSize);

            Response.AddPaginationHeader(slides.MetaData);

            return slides;

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Slide>> CreateSlide([FromForm] CreateSlideDto createSlideDto)
        {
            var existing = await _context.Slides.FirstOrDefaultAsync(x => x.Priority == createSlideDto.Priority);
            if (existing != null) return BadRequest(new ProblemDetails { Title = "Item with this priority exist" });

            var slide = _mapper.Map<Slide>(createSlideDto);



            if (createSlideDto.File != null)
            {
                var fileName = await WriteFile(createSlideDto.File);

                if (fileName.Length == 0)
                    return BadRequest(new ProblemDetails { Title = "Problem uploading new image" });

                slide.PictureUrl = fileName;

            }

            _context.Slides.Add(slide);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok(slide);

            return BadRequest(new ProblemDetails { Title = "Problem creating new slide" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult<Slide>> UpdateSlide([FromForm] UpdateSlideDto updateSlideDto)
        {
            var slide = await _context.Slides.FindAsync(updateSlideDto.Id);


            if (slide == null) return NotFound();

            var existing = await _context.Slides.FirstOrDefaultAsync(x => x.Priority == updateSlideDto.Priority && updateSlideDto.Priority != slide.Priority);
            if (existing != null) return BadRequest(new ProblemDetails { Title = "Item with this priority exist" });

            if (updateSlideDto.File != null)
            {
                var fileName = await WriteFile(updateSlideDto.File);

                if (fileName.Length == 0)
                    return BadRequest(new ProblemDetails { Title = "Problem uploading new image" });

                slide.PictureUrl = fileName;

            }
            else
            {
                slide.PictureUrl = slide.PictureUrl;

            }

            _mapper.Map(updateSlideDto, slide);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok(slide);

            return BadRequest(new ProblemDetails { Title = "Problem updating slide" });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSlide(int id)
        {
            var slide = await _context.Slides.FindAsync(id);

            if (slide == null) return NotFound();

            if (!string.IsNullOrEmpty(slide.PictureUrl))
            {

                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "..//var//lib//Upload//Images", slide.PictureUrl);
                if (System.IO.File.Exists(filepath))
                    System.IO.File.Delete(filepath);
            }


            _context.Slides.Remove(slide);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem deleting slide" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateMultipleItems")]
        public async Task<ActionResult> UpdateSlides([FromBody] List<int> ids)
        {

            if (ids == null || ids.Count == 0) return NotFound();

            foreach (var id in ids)
            {
                var item = await _context.Slides.FindAsync(id);

                if (item == null) return NotFound();

                item.IsActive = !item.IsActive;

            }

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem updating slides" });
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("DeleteMultipleItems")]
        public async Task<ActionResult> DeleteSlides([FromBody] List<int> ids)
        {

            if (ids == null || ids.Count == 0) return NotFound();

            foreach (var id in ids)
            {

                var item = await _context.Slides.FindAsync(id);

                if (item == null) return NotFound();

                if (!string.IsNullOrEmpty(item.PictureUrl))
                {

                    var filepath = Path.Combine(Directory.GetCurrentDirectory(), "..//var//lib//Upload//Images", item.PictureUrl);
                    if (System.IO.File.Exists(filepath))
                        System.IO.File.Delete(filepath);
                }


                _context.Slides.Remove(item);
            }


            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem deleting slides" });
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
