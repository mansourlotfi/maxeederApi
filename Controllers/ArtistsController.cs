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
    public class ArtistsController:BaseApiController
    {
        private readonly StoreContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostEnvironment;
        public ArtistsController(StoreContext context, IMapper mapper, IWebHostEnvironment hostEnvironment)
        {
            _mapper = mapper;
            this._hostEnvironment = hostEnvironment;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<Artist>>> GetArtists([FromQuery] PaginationParams? paginationParams)
        {
            // return await _context.Artists.ToListAsync();
            var query = _context.Artists.Select(x => new Artist()
            {
                Id = x.Id,
                Name = x.Name,
                Text = x.Text,
                PictureUrl = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.PictureUrl),
                Priority = x.Priority,
                IsActive=x.IsActive,
                NameEn=x.NameEn,
                TextEn=x.TextEn,
            }).SearchArtist(paginationParams.SearchTerm).AsQueryable();

            var artist = await PagedList<Artist>.ToPagedList(query, paginationParams.PageNumber, paginationParams.PageSize);

            Response.AddPaginationHeader(artist.MetaData);

            return artist;

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Artist>> CreateArtist([FromForm] CreateArtistDto createArtistDto)
        {
            var existing = await _context.Artists.FirstOrDefaultAsync(x => x.Priority == createArtistDto.Priority);
            if (existing != null) return BadRequest(new ProblemDetails { Title = "Item with this priority exist" });

            var artist = _mapper.Map<Artist>(createArtistDto);



            if (createArtistDto.File != null)
            {
                var fileName = await WriteFile(createArtistDto.File);

                if (fileName.Length == 0)
                    return BadRequest(new ProblemDetails { Title = "Problem uploading new image" });

                artist.PictureUrl = fileName;

            }

            _context.Artists.Add(artist);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok(artist);

            return BadRequest(new ProblemDetails { Title = "Problem creating new artist" });
        }


        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult<Artist>> UpdateArtist([FromForm] UpdateArtistDto updateArtistDto)
        {
            var artist = await _context.Artists.FindAsync(updateArtistDto.Id);


            if (artist == null) return NotFound();

            var existing = await _context.Artists.FirstOrDefaultAsync(x => x.Priority == updateArtistDto.Priority && updateArtistDto.Priority != artist.Priority);
            if (existing != null) return BadRequest(new ProblemDetails { Title = "Item with this priority exist" });

            if (updateArtistDto.File != null)
            {
                var fileName = await WriteFile(updateArtistDto.File);

                if (fileName.Length == 0)
                    return BadRequest(new ProblemDetails { Title = "Problem uploading new image" });

                artist.PictureUrl = fileName;

            }
            else
            {
                artist.PictureUrl = artist.PictureUrl;

            }

            _mapper.Map(updateArtistDto, artist);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok(artist);

            return BadRequest(new ProblemDetails { Title = "Problem updating artist" });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteArtist(int id)
        {
            var artist = await _context.Artists.FindAsync(id);

            if (artist == null) return NotFound();

            if (!string.IsNullOrEmpty(artist.PictureUrl))
            {

                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "..//var//lib//Upload//Images", artist.PictureUrl);
                if (System.IO.File.Exists(filepath))
                    System.IO.File.Delete(filepath);
            }


            _context.Artists.Remove(artist);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem deleting artist" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateMultipleItems")]
        public async Task<ActionResult> UpdateArtists([FromBody] List<int> ids)
        {

            if (ids == null || ids.Count == 0) return NotFound();

            foreach (var id in ids)
            {
                var item = await _context.Artists.FindAsync(id);

                if (item == null) return NotFound();

                item.IsActive = !item.IsActive;

            }

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem updating Artists" });
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("DeleteMultipleItems")]
        public async Task<ActionResult> DeleteArtists([FromBody] List<int> ids)
        {

            if (ids == null || ids.Count == 0) return NotFound();

            foreach (var id in ids)
            {

                var item = await _context.Artists.FindAsync(id);

                if (item == null) return NotFound();

                if (!string.IsNullOrEmpty(item.PictureUrl))
                {

                    var filepath = Path.Combine(Directory.GetCurrentDirectory(), "..//var//lib//Upload//Images", item.PictureUrl);
                    if (System.IO.File.Exists(filepath))
                        System.IO.File.Delete(filepath);
                }

                _context.Artists.Remove(item);
            }


            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem deleting Artists" });
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
