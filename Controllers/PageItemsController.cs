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
    public class PageItemsController : BaseApiController
    {
        private readonly StoreContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostEnvironment;
        public PageItemsController(StoreContext context, IMapper mapper, IWebHostEnvironment hostEnvironment)
        {
            _mapper = mapper;
            this._hostEnvironment = hostEnvironment;
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<PagedList<PageItem>>> GetPageItems([FromQuery] PageItemParams? pageItemParams)
        {
            // return await _context.PageItems.ToListAsync();
            var query = _context.PageItems.Where(x => x.Page == pageItemParams.Page).Select(x => new PageItem()
            {
                Id = x.Id,
                Title = x.Title,
                Text = x.Text,
                Link = x.Link,
                PictureUrl = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.PictureUrl),
                Priority = x.Priority,
                Page = x.Page,
                RitchText=x.RitchText,
                IsActive=x.IsActive,
                TitleEn=x.TitleEn,
                TextEn=x.TextEn,
                ShortDesc=x.ShortDescEn,
                ShortDescEn=x.ShortDescEn,
            }).SearchPageItem(pageItemParams.SearchTerm).AsQueryable();

            var items = await PagedList<PageItem>.ToPagedList(query, pageItemParams.PageNumber, pageItemParams.PageSize);

            Response.AddPaginationHeader(items.MetaData);

            return items;

        }


        [HttpGet("MaxPriority")]
        public async Task<ActionResult<int>> GetMaxPriorityPageItems()
        {
            var maxPriorityNo = _context.PageItems.Max(p => p.Priority);

            if (maxPriorityNo == null || maxPriorityNo == 0) return 1;

            return Ok(maxPriorityNo);

        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<PageItem>> CreatePageItem([FromForm] CreatePageItemDto createPageItemDto)
        {
            var existing = await _context.PageItems.FirstOrDefaultAsync(x => x.Priority == createPageItemDto.Priority);
            if (existing != null) return BadRequest(new ProblemDetails { Title = "Item with this priority exist" });

            var pageItem = _mapper.Map<PageItem>(createPageItemDto);



            if (createPageItemDto.File != null)
            {
                var fileName = await WriteFile(createPageItemDto.File);

                if (fileName.Length == 0)
                    return BadRequest(new ProblemDetails { Title = "Problem uploading new image" });

                pageItem.PictureUrl = fileName;

            }

            //if (_hostEnvironment.IsDevelopment())
            //{
            //    bool exists = System.IO.Directory.Exists("..\\var\\lib\\Upload\\RTF");
            //    if (!exists)
            //        System.IO.Directory.CreateDirectory("..\\var\\lib\\Upload\\RTF");
            //    var ContactUsPath = "..\\var\\lib\\Upload\\RTF\\ContactUsRitchText.txt";
            //    System.IO.File.WriteAllText(ContactUsPath, String.Empty);
            //    using StreamWriter swc = new StreamWriter(ContactUsPath);
            //    swc.Write(createPageItemDto.RitchText);
            //    swc.Close();

            //}
            //else
            //{
            //    bool exists = System.IO.Directory.Exists("..//var//lib//Upload//RTF");
            //    if (!exists)
            //        System.IO.Directory.CreateDirectory("..//var//lib//Upload//RTF");

            //    var ContactUsPath = "..//var//lib//Upload//RTF//ContactUsRitchText.txt";
            //    System.IO.File.WriteAllText(ContactUsPath, String.Empty);
            //    using StreamWriter swc = new StreamWriter(ContactUsPath);
            //    swc.Write(createPageItemDto.RitchText);
            //    swc.Close();


            //}

            _context.PageItems.Add(pageItem);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok(pageItem);

            return BadRequest(new ProblemDetails { Title = "Problem creating new page Item" });
        }


        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult<PageItem>> UpdatePageItem([FromForm] UpdatePageItemDto updatePageItemDto)
        {
            var item = await _context.PageItems.FindAsync(updatePageItemDto.Id);


            if (item == null) return NotFound();

            var existing = await _context.PageItems.FirstOrDefaultAsync(x => x.Priority == updatePageItemDto.Priority && updatePageItemDto.Priority != item.Priority);
            if (existing != null) return BadRequest(new ProblemDetails { Title = "Item with this priority exist" });

            if (updatePageItemDto.File != null)
            {
                var fileName = await WriteFile(updatePageItemDto.File);

                if (fileName.Length == 0)
                    return BadRequest(new ProblemDetails { Title = "Problem uploading new image" });

                item.PictureUrl = fileName;

            }
            else
            {
                item.PictureUrl = item.PictureUrl;

            }

            _mapper.Map(updatePageItemDto, item);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok(item);

            return BadRequest(new ProblemDetails { Title = "Problem updating item" });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePageItem(int id)
        {
            var item = await _context.PageItems.FindAsync(id);

            if (item == null) return NotFound();

            if (!string.IsNullOrEmpty(item.PictureUrl))
            {

                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "..//var//lib//Upload//Images", item.PictureUrl);
                if (System.IO.File.Exists(filepath))
                    System.IO.File.Delete(filepath);
            }


            _context.PageItems.Remove(item);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem deleting item" });
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateMultipleItems")]
        public async Task<ActionResult> UpdatePageItemsList([FromBody] List<int> ids)
        {

            if (ids == null || ids.Count == 0) return NotFound();

            foreach (var id in ids)
            {
                var item = await _context.PageItems.FindAsync(id);

                if (item == null) return NotFound();

                item.IsActive = !item.IsActive;

            }

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem updating PageItems" });
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("DeleteMultipleItems")]
        public async Task<ActionResult> DeletePageItemsList([FromBody] List<int> ids)
        {

            if (ids == null || ids.Count == 0) return NotFound();

            foreach (var id in ids)
            {

                var item = await _context.PageItems.FindAsync(id);

                if (item == null) return NotFound();

                if (!string.IsNullOrEmpty(item.PictureUrl))
                {

                    var filepath = Path.Combine(Directory.GetCurrentDirectory(), "..//var//lib//Upload//Images", item.PictureUrl);
                    if (System.IO.File.Exists(filepath))
                        System.IO.File.Delete(filepath);
                }

                _context.PageItems.Remove(item);
            }


            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem deleting Page Items" });
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
