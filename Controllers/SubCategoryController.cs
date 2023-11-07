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
    public class SubCategoryController:BaseApiController
    {
        private readonly StoreContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostEnvironment;
        public SubCategoryController(StoreContext context, IMapper mapper, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _mapper = mapper;
            this._hostEnvironment = hostEnvironment;
        }


        [HttpGet]
        public async Task<ActionResult<PagedList<SubCategory>>> GetCategories([FromQuery] SubCategoryParams? paginationParams)
        {
            var query =  _context.SubCategories.Where(x => x.CategoryId == paginationParams.CategoryId).Select(x => new SubCategory()
            {
                Id = x.Id,
                Name = x.Name,
                Link = x.Link,
                PictureUrl = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.PictureUrl),
                Priority = x.Priority,
                IsActive = x.IsActive,
                NameEn = x.NameEn,
                Category=x.Category,
                CategoryId = x.CategoryId,
                
                
            }).SearchSubCategory(paginationParams.SearchTerm).AsQueryable();

            var items = await PagedList<SubCategory>.ToPagedList(query, paginationParams.PageNumber, paginationParams.PageSize);

            Response.AddPaginationHeader(items.MetaData);

            return items;
        }

        [HttpGet("{id}", Name = "GetSubCategory")]
        public async Task<ActionResult<SubCategory>> GetCategory(int id)
        {
            var category = await _context.SubCategories.FindAsync(id);

            if (category == null) return NotFound();

            return category;

        }

        [HttpGet("MaxPriority")]
        public async Task<ActionResult<int>> GetMaxPrioritySubCategory()
        {
            var maxPriorityNo =  _context.SubCategories.Max(p => p.Priority) ;

            if (maxPriorityNo == null || maxPriorityNo == 0) return 1;

            return Ok(maxPriorityNo);

        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<SubCategory>> CreateSubCategory([FromForm] CreateSubCategoryDto categoryDto)

        {

            var existing = await _context.SubCategories.FirstOrDefaultAsync(x => x.Priority == categoryDto.Priority);
            if (existing != null) return BadRequest(new ProblemDetails { Title = "Item with this priority exist" });

            //var category = new Category()
            //{
            //    Id = Guid.NewGuid(),
            //    Name = categoryDto.Name
            //};

            var category = _mapper.Map<SubCategory>(categoryDto);

            if (categoryDto.File != null)
            {
                var fileName = await WriteFile(categoryDto.File);

                if (fileName.Length == 0)
                    return BadRequest(new ProblemDetails { Title = "Problem uploading new image" });

                category.PictureUrl = fileName;
            }

            var existingCategory = await _context.Categories.FirstOrDefaultAsync(x => x.Id == categoryDto.CategoryId);

            if (existingCategory != null)
            {
                category.CategoryId = categoryDto.CategoryId;
                category.Category = existingCategory;

            }
            else
            {
                return BadRequest(new ProblemDetails { Title = "Problem creating new sub category" });

            }

            _context.SubCategories.Add(category);


            var result = await _context.SaveChangesAsync() > 0;

            if (result) return CreatedAtRoute("GetSubCategory", new { Id = category.Id }, category);

            return BadRequest(new ProblemDetails { Title = "Problem creating new category" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult<SubCategory>> UpdateSubCategory([FromForm] UpdateSubCategoryDto updateCategoryDto)
        {
            var category = await _context.SubCategories.FindAsync(updateCategoryDto.Id);


            if (category == null) return NotFound();

            var existing = await _context.SubCategories.FirstOrDefaultAsync(x => x.Priority == updateCategoryDto.Priority && updateCategoryDto.Priority != category.Priority);
            if (existing != null) return BadRequest(new ProblemDetails { Title = "Item with this priority exist" });

            if (updateCategoryDto.File != null)
            {
                var fileName = await WriteFile(updateCategoryDto.File);

                if (fileName.Length == 0)
                    return BadRequest(new ProblemDetails { Title = "Problem uploading new image" });

                category.PictureUrl = fileName;

            }
            else
            {
                category.PictureUrl = category.PictureUrl;

            }

            _mapper.Map(updateCategoryDto, category);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok(category);

            return BadRequest(new ProblemDetails { Title = "Problem updating sub category" });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSubCategory(int id)
        {
            var category = await _context.SubCategories.FindAsync(id);

            if (category == null) return NotFound();

            if (!string.IsNullOrEmpty(category.PictureUrl))
            {

                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "..//var//lib//Upload//Images", category.PictureUrl);
                if (System.IO.File.Exists(filepath))
                    System.IO.File.Delete(filepath);
            }

            _context.SubCategories.Remove(category);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem deleting category" });
        }



        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateMultipleItems")]
        public async Task<ActionResult> UpdateSubCategories([FromBody] List<int> ids)
        {

            if (ids == null || ids.Count == 0) return NotFound();

            foreach (var id in ids)
            {
                var item = await _context.SubCategories.FindAsync(id);

                if (item == null) return NotFound();

                item.IsActive = !(bool)item.IsActive;

            }

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem updating SubCategories" });
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("DeleteMultipleItems")]
        public async Task<ActionResult> DeleteSubCategories([FromBody] List<int> ids)
        {

            if (ids == null || ids.Count == 0) return NotFound();

            foreach (var id in ids)
            {

                var item = await _context.SubCategories.FindAsync(id);

                if (item == null) return NotFound();

                if (!string.IsNullOrEmpty(item.PictureUrl))
                {

                    var filepath = Path.Combine(Directory.GetCurrentDirectory(), "..//var//lib//Upload//Images", item.PictureUrl);
                    if (System.IO.File.Exists(filepath))
                        System.IO.File.Delete(filepath);
                }


                _context.SubCategories.Remove(item);
            }


            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem deleting SubCategories" });
        }

        private async Task<string> WriteFile(IFormFile file)
        {
            var fileName = DateTime.Now.Ticks.ToString() + Path.GetExtension(file.FileName);

            if (_hostEnvironment.IsDevelopment())
            {
                var filePath = Path.Combine(_hostEnvironment.ContentRootPath, "..\\var\\lib\\Upload\\Images", fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
            else
            {
                var filePath = Path.Combine(_hostEnvironment.ContentRootPath, "..//var//lib//Upload//Images", fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }


            return fileName;

        }

    }
}
