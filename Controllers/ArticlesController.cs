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
    public class ArticlesController:BaseApiController
    {
        private readonly StoreContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ArticlesController(StoreContext context, IMapper mapper, IWebHostEnvironment hostEnvironment)
        {
            this._context = context;
            this._mapper = mapper;
            this._hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<Article>>> GetPageItems([FromQuery] PaginationParams? paginationParams)
        {
            // return await _context.PageItems.ToListAsync();
            var query = _context.Articles.Select(x => new Article()
            {
                Id = x.Id,
                Title = x.Title,
                Link = x.Link,
                PictureUrl = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.PictureUrl),
                Priority = x.Priority,
                RitchText = x.RitchText,
                IsActive = x.IsActive,
                RitchTextEn=x.RitchTextEn,
                TitleEn=x.TitleEn,
            }).AsQueryable();

            var items = await PagedList<Article>.ToPagedList(query, paginationParams.PageNumber, paginationParams.PageSize);

            Response.AddPaginationHeader(items.MetaData);

            return items;

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Article>> CreateArticle([FromForm] CreateArticleDto createArticleDto)
        {
            var existing = await _context.Articles.FirstOrDefaultAsync(x => x.Priority == createArticleDto.Priority);
            if (existing != null) return BadRequest(new ProblemDetails { Title = "Item with this priority exist" });

            var article = _mapper.Map<Article>(createArticleDto);



            if (createArticleDto.File != null)
            {
                var fileName = await WriteFile(createArticleDto.File);

                if (fileName.Length == 0)
                    return BadRequest(new ProblemDetails { Title = "Problem uploading new image" });

                article.PictureUrl = fileName;

            }


            _context.Articles.Add(article);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok(article);

            return BadRequest(new ProblemDetails { Title = "Problem creating new article" });
        }


        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult<Article>> UpdateArticle([FromForm] UpdateArticleDto updateArticleDto)
        {
            var item = await _context.Articles.FindAsync(updateArticleDto.Id);


            if (item == null) return NotFound();

            var existing = await _context.Articles.FirstOrDefaultAsync(x => x.Priority == updateArticleDto.Priority && updateArticleDto.Priority != item.Priority);
            if (existing != null) return BadRequest(new ProblemDetails { Title = "Item with this priority exist" });

            if (updateArticleDto.File != null)
            {
                var fileName = await WriteFile(updateArticleDto.File);

                if (fileName.Length == 0)
                    return BadRequest(new ProblemDetails { Title = "Problem uploading new image" });

                item.PictureUrl = fileName;

            }
            else
            {
                item.PictureUrl = item.PictureUrl;

            }

            _mapper.Map(updateArticleDto, item);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok(item);

            return BadRequest(new ProblemDetails { Title = "Problem updating Articles" });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteArticle(int id)
        {
            var item = await _context.Articles.FindAsync(id);

            if (item == null) return NotFound();

            if (!string.IsNullOrEmpty(item.PictureUrl))
            {

                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "..//var//lib//Upload//Images", item.PictureUrl);
                if (System.IO.File.Exists(filepath))
                    System.IO.File.Delete(filepath);
            }


            _context.Articles.Remove(item);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem deleting Article" });
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateMultipleItems")]
        public async Task<ActionResult> UpdateArticles([FromBody] List<int> ids)
        {

            if (ids == null || ids.Count == 0) return NotFound();

            foreach (var id in ids)
            {
                var item = await _context.Articles.FindAsync(id);

                if (item == null) return NotFound();

                item.IsActive = !item.IsActive;

            }

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem updating Articles" });
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("DeleteMultipleItems")]
        public async Task<ActionResult> DeleteArticles([FromBody] List<int> ids)
        {

            if (ids == null || ids.Count == 0) return NotFound();

            foreach (var id in ids)
            {

                var item = await _context.Articles.FindAsync(id);

                if (item == null) return NotFound();

                if (!string.IsNullOrEmpty(item.PictureUrl))
                {

                    var filepath = Path.Combine(Directory.GetCurrentDirectory(), "..//var//lib//Upload//Images", item.PictureUrl);
                    if (System.IO.File.Exists(filepath))
                        System.IO.File.Delete(filepath);
                }

                _context.Articles.Remove(item);
            }


            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem deleting Articles" });
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
