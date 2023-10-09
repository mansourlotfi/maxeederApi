using AutoMapper;
using ecommerceApi.Data;
using ecommerceApi.DTOs;
using ecommerceApi.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ecommerceApi.Controllers
{
    public class ProductFeaturesController:BaseApiController
    {
        private readonly StoreContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostEnvironment;
        public ProductFeaturesController(StoreContext context, IMapper mapper, IWebHostEnvironment hostEnvironment)
        {
            this._context = context;
            this._mapper = mapper;
            this._hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        public async Task<ActionResult<Feature>> GetFeatures()
        {
            var features = await _context.Features.Select(x => new Feature()
            {
                Id = x.Id,
                Name = x.Name,
                PictureUrl = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.PictureUrl),
                Description = x.Description,
                IsActive = x.IsActive,
                Products=x.Products,
            }).ToListAsync();

            if (features == null) return NotFound();
            return Ok(features);

        }

        [HttpGet("{id}", Name = "GetFeature")]
        public async Task<ActionResult<Feature>> GetFeature(int id)
        {
            var feature = await _context.Features.FindAsync(id);

            if (feature == null) return NotFound();

            return feature;

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Feature>> CreateFeature([FromForm] CreateFeatureDto featureDto)

        {
            //var category = new Feature()
            //{
            //    Id = Guid.NewGuid(),
            //    Name = categoryDto.Name
            //};
            var feature = _mapper.Map<Feature>(featureDto);

            if (featureDto.File != null)
            {
                var fileName = await WriteFile(featureDto.File);

                if (fileName.Length == 0)
                    return BadRequest(new ProblemDetails { Title = "Problem uploading new image" });

                feature.PictureUrl = fileName;
            }

            _context.Features.Add(feature);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return CreatedAtRoute("GetFeature", new { Id = feature.Id }, feature);

            return BadRequest(new ProblemDetails { Title = "Problem creating new feature" });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFeature(int id)
        {
            var feature = await _context.Features.FindAsync(id);

            if (feature == null) return NotFound();

            if (!string.IsNullOrEmpty(feature.PictureUrl))
            {

                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "..//var//lib//Upload//Images", feature.PictureUrl);
                if (System.IO.File.Exists(filepath))
                    System.IO.File.Delete(filepath);
            }

            _context.Features.Remove(feature);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem deleting feature" });
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateMultipleItems")]
        public async Task<ActionResult> UpdateFeaturesList([FromBody] List<int> ids)
        {

            if (ids == null || ids.Count == 0) return NotFound();

            foreach (var id in ids)
            {
                var item = await _context.Features.FindAsync(id);

                if (item == null) return NotFound();

                item.IsActive = !item.IsActive;

            }

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem updating Features List" });
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("DeleteMultipleItems")]
        public async Task<ActionResult> DeleteFeaturesList([FromBody] List<int> ids)
        {

            if (ids == null || ids.Count == 0) return NotFound();

            foreach (var id in ids)
            {

                var item = await _context.Features.FindAsync(id);

                if (item == null) return NotFound();

                if (!string.IsNullOrEmpty(item.PictureUrl))
                {

                    var filepath = Path.Combine(Directory.GetCurrentDirectory(), "..//var//lib//Upload//Images", item.PictureUrl);
                    if (System.IO.File.Exists(filepath))
                        System.IO.File.Delete(filepath);
                }

                _context.Features.Remove(item);
            }


            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem deleting Features List" });
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
