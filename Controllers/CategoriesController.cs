using AutoMapper;
using ecommerceApi.Data;
using ecommerceApi.DTOs;
using ecommerceApi.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ecommerceApi.Controllers
{
    public class CategoriesController: BaseApiController
    {
        private readonly StoreContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostEnvironment;
        public CategoriesController(StoreContext context, IMapper mapper, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _mapper = mapper;
            this._hostEnvironment = hostEnvironment;

        }

        [HttpGet]
        public async Task<ActionResult<Category>> GetCategories()
        {
            var categories = await _context.Categories.Select(x => new Category()
            {
                Id = x.Id,
                Name = x.Name,
                PictureUrl = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.PictureUrl),
            }).ToListAsync();
                           
           if (categories == null) return NotFound();
           return Ok(categories);
        }


        [HttpGet("{id}", Name = "GetCategory")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null) return NotFound();

            return category;

        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory([FromForm] CreateCategoryDto categoryDto)

        {

            var existing = await _context.Categories.FirstOrDefaultAsync(x => x.Priority == categoryDto.Priority);
            if (existing != null) return BadRequest(new ProblemDetails { Title = "Item with this priority exist" });

            //var category = new Category()
            //{
            //    Id = Guid.NewGuid(),
            //    Name = categoryDto.Name
            //};

            var category = _mapper.Map<Category>(categoryDto);

            if (categoryDto.File != null)
            {
                var fileName = await WriteFile(categoryDto.File);

                if (fileName.Length == 0)
                    return BadRequest(new ProblemDetails { Title = "Problem uploading new image" });

                category.PictureUrl = fileName;
            }

            _context.Categories.Add(category);


            var result = await _context.SaveChangesAsync() > 0;

            if (result) return CreatedAtRoute("GetCategory", new { Id = category.Id }, category);

            return BadRequest(new ProblemDetails { Title = "Problem creating new category" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult<Category>> UpdateCategory([FromForm] UpdateCategoryDto updateCategoryDto)
        {
            var category = await _context.Categories.FindAsync(updateCategoryDto.Id);


            if (category == null) return NotFound();

            var existing = await _context.Categories.FirstOrDefaultAsync(x => x.Priority == updateCategoryDto.Priority && updateCategoryDto.Priority != category.Priority);
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

            return BadRequest(new ProblemDetails { Title = "Problem updating social network" });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null) return NotFound();

            if (!string.IsNullOrEmpty(category.PictureUrl))
            {

                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "..//var//lib//Upload//Images", category.PictureUrl);
                if (System.IO.File.Exists(filepath))
                    System.IO.File.Delete(filepath);
            }

            _context.Categories.Remove(category);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem deleting category" });
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
