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
    public class BrandsController:BaseApiController
    {
        private readonly StoreContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostEnvironment;

        public BrandsController(StoreContext context, IMapper mapper, IWebHostEnvironment hostEnvironment)
        {
            this._context = context;
            this._mapper = mapper;
            this._hostEnvironment = hostEnvironment;

        }

        [HttpGet]
        public async Task<ActionResult<PagedList<Brand>>> GetBrands([FromQuery] PaginationParams? paginationParams)
        {
            var query =  _context.Brands.Select(x => new Brand()
            {
                Id = x.Id,
                Name = x.Name,
                PictureUrl = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.PictureUrl),
                IsActive=x.IsActive,  
                NameEn=x.NameEn,
            }).SearchBrands(paginationParams.SearchTerm).AsQueryable();

            var items = await PagedList<Brand>.ToPagedList(query, paginationParams.PageNumber, paginationParams.PageSize);

            Response.AddPaginationHeader(items.MetaData);

            return items;

        }

        [HttpGet("{id}", Name = "GetBrand")]
        public async Task<ActionResult<Brand>> GetBrand(int id)
        {
            var brand = await _context.Brands.FindAsync(id);

            if (brand == null) return NotFound();

            return brand;

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Brand>> CreateBrand([FromForm] CreateBrandDto brandsDto)

        {
            //var category = new Category()
            //{
            //    Id = Guid.NewGuid(),
            //    Name = categoryDto.Name
            //};
            var brand = _mapper.Map<Brand>(brandsDto);

            if (brandsDto.File != null)
            {
                var fileName = await WriteFile(brandsDto.File);

                if (fileName.Length == 0)
                    return BadRequest(new ProblemDetails { Title = "Problem uploading new image" });

                brand.PictureUrl = fileName;
            }

            _context.Brands.Add(brand);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return CreatedAtRoute("GetBrand", new { Id = brand.Id }, brand);

            return BadRequest(new ProblemDetails { Title = "Problem creating new brand" });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBrand(int id)
        {
            var brand = await _context.Brands.FindAsync(id);

            if (brand == null) return NotFound();

            if (!string.IsNullOrEmpty(brand.PictureUrl))
            {

                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "..//var//lib//Upload//Images", brand.PictureUrl);
                if (System.IO.File.Exists(filepath))
                    System.IO.File.Delete(filepath);
            }

            _context.Brands.Remove(brand);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem deleting brand" });
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateMultipleItems")]
        public async Task<ActionResult> UpdateBrands([FromBody] List<int> ids)
        {

            if (ids == null || ids.Count == 0) return NotFound();

            foreach (var id in ids)
            {
                var item = await _context.Brands.FindAsync(id);

                if (item == null) return NotFound();

                item.IsActive = !item.IsActive;

            }

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem updating Brands" });
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("DeleteMultipleItems")]
        public async Task<ActionResult> DeleteBrands([FromBody] List<int> ids)
        {

            if (ids == null || ids.Count == 0) return NotFound();

            foreach (var id in ids)
            {

                var item = await _context.Brands.FindAsync(id);

                if (item == null) return NotFound();

                if (!string.IsNullOrEmpty(item.PictureUrl))
                {

                    var filepath = Path.Combine(Directory.GetCurrentDirectory(), "..//var//lib//Upload//Images", item.PictureUrl);
                    if (System.IO.File.Exists(filepath))
                        System.IO.File.Delete(filepath);
                }

                _context.Brands.Remove(item);
            }


            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem deleting Brands" });
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
