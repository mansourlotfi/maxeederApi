using AutoMapper;
using ecommerceApi.Data;
using ecommerceApi.DTOs;
using ecommerceApi.Entities;
using ecommerceApi.Extensions;
using ecommerceApi.RequestHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;

namespace ecommerceApi.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly StoreContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductsController(StoreContext context, IMapper mapper,IWebHostEnvironment hostEnvironment)
        {
            _mapper = mapper;
            this._hostEnvironment = hostEnvironment;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<Product>>> GetProducts([FromQuery] ProductParams? productParams)
        {
            // return await _context.Products.ToListAsync();
            var query = _context.Products.Include(x=>x.Features).ThenInclude(f=>f.Feature).Select(x=> new Product()
            {
                Id = x.Id,
                Brand = x.Brand,
                Name = x.Name,
                Description = x.Description,
                Price = x.Price,
                QuantityInStock = x.QuantityInStock,
                SubCategoryId = x.SubCategoryId,
                SubCategory = x.SubCategory,
                IsFeatured = x.IsFeatured,
                PictureUrl = String.Format("{0}://{1}{2}/Images/{3}",Request.Scheme,Request.Host,Request.PathBase, x.PictureUrl) ,
                Features=x.Features,
                IsActive = x.IsActive,
                Size = x.Size,
                DescriptionEn=x.DescriptionEn,
                NameEn=x.NameEn,
                Usage=x.Usage,
                MediaList = (List<Media>)x.MediaList.Select(M => new Media()
                {
                    Id = M.Id,
                    MediaFileName = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, M.MediaFileName) ,
                }),
                Priority = x.Priority,
                ShowPrice = x.ShowPrice,
                CommentList = (List<Comment>)x.CommentList
            })
            .Sort(productParams.OrderBy)
            .Search(productParams.SearchTerm)
            .Filter(productParams.Brands, productParams.Types,productParams.Size, productParams.Usage, productParams.IsActive, productParams.ShowPrice)
            .AsQueryable();

            var products = await PagedList<Product>.ToPagedList(query, productParams.PageNumber, productParams.PageSize);

            Response.AddPaginationHeader(products.MetaData);

            return products;

        }

        [HttpGet]
        [Route("GetFeaturedProducts")]

        public async Task<ActionResult<PagedList<Product>>> GetFeaturedProducts()
        {
            var query = _context.Products.Where(p=> p.IsFeatured == true).Select(x => new Product()
            {
                Id = x.Id,
                Brand = x.Brand,
                Name = x.Name,
                Description = x.Description,
                Price = x.Price,
                QuantityInStock = x.QuantityInStock,
                SubCategoryId = x.SubCategoryId,
                SubCategory = x.SubCategory,
                IsFeatured = x.IsFeatured,
                PictureUrl = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.PictureUrl),
                Features=x.Features,
                IsActive=x.IsActive,
                Size = x.Size,
                DescriptionEn = x.DescriptionEn,
                NameEn = x.NameEn,
                Usage = x.Usage,
                MediaList=x.MediaList,
                Priority = x.Priority,
                ShowPrice = x.ShowPrice,
                CommentList=x.CommentList
            }).AsQueryable();

            var products = await PagedList<Product>.ToPagedList(query, 1, 10);


            return products;
        }

        [HttpGet("{id}", Name = "GetProduct")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.Include(x => x.Features).ThenInclude(f => f.Feature).Include(x=>x.MediaList).Include(Y=>Y.CommentList).Include(z=>z.SubCategory).ThenInclude(w=>w.Category).FirstOrDefaultAsync(x=>x.Id == id);

            if (product == null) return NotFound();

            product.PictureUrl = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, product.PictureUrl);
            product.MediaList = (List<Media>)product.MediaList.Select(M => new Media()
            {
                Id = M.Id,
                MediaFileName = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, M.MediaFileName),
                Product=product,
                ProductId=product.Id
            }).ToList();

            return product;

        }

        [HttpGet("filters")]
        public async Task<IActionResult> GetFilters()
        {
            var brands = await _context.Products.Select(p => p.Brand).Distinct().ToListAsync();
            var types = await _context.Products.Select(p => p.SubCategory).Distinct().ToListAsync();
            var size = await _context.Products.Select(p => p.Size).Distinct().ToListAsync();
            var usage = await _context.Products.Select(p => p.Usage).Distinct().ToListAsync();

            return Ok(new { brands, types, size, usage });

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromForm] CreateProductDto productDto)
        {

            var existing = await _context.Products.FirstOrDefaultAsync(x => x.Priority == productDto.Priority);
            if (existing != null) return BadRequest(new ProblemDetails { Title = "Item with this priority exist" });

            var product = _mapper.Map<Product>(productDto);

            if (productDto.Features?.Count > 0)
            {
                foreach (var item in productDto.Features)
                {
                product.Features.Add(new ProductFeature()
                {
                    Product = product,
                    FeatureId = item
                });
                }
            }

            if (productDto.SubCategoryId != null)
            {

                var existingSubCategory = await _context.SubCategories.FirstOrDefaultAsync(x => x.Id == productDto.SubCategoryId);

                if (existingSubCategory != null)
                {
                    product.SubCategoryId = productDto.SubCategoryId;
                    product.SubCategory = existingSubCategory;

                }
                else
                {
                    return BadRequest(new ProblemDetails { Title = "Problem creating new product with given sub category id" });

                }

            }


            if (productDto.IsFeatured != true)
            {
                product.IsFeatured = false;
            }

            if (productDto.File != null)
            {
                var fileName = await WriteFile(productDto.File);

                if (fileName.Length == 0)
                    return BadRequest(new ProblemDetails { Title = "Problem uploading new image" });

                product.PictureUrl = fileName;

            }

            _context.Products.Add(product);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return CreatedAtRoute("GetProduct", new { Id = product.Id }, product);

            return BadRequest(new ProblemDetails { Title = "Problem creating new product" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult<Product>> UpdateProduct([FromForm] UpdateProductDto productDto)
        {
            var product = await _context.Products.Include(x=>x.Features).ThenInclude(y=>y.Feature).Include(z=>z.SubCategory).FirstOrDefaultAsync(x=>x.Id == productDto.Id);


            if (product == null) return NotFound();

            var existing = await _context.Products.FirstOrDefaultAsync(x => x.Priority == productDto.Priority && productDto.Priority != product.Priority);
            if (existing != null) return BadRequest(new ProblemDetails { Title = "Item with this priority exist" });

            if (productDto.File != null)
            {
                var fileName = await WriteFile(productDto.File);

                if (fileName.Length == 0)
                    return BadRequest(new ProblemDetails { Title = "Problem uploading new image" });

                product.PictureUrl = fileName;

            }
            else
            {
                product.PictureUrl = product.PictureUrl;

            }

            if (productDto.Features?.Count > 0)
            {
               
                var existingIds = product.Features.Select(x => x.FeatureId).ToList();
                var selectedIds = productDto.Features.ToList();
                var toAdd = selectedIds.Except(existingIds).ToList();
                var toRemove = existingIds.Except(selectedIds).ToList();

                product.Features = product.Features.Where(x => !toRemove.Contains(x.FeatureId)).ToList();
                foreach (var item in toAdd)
                {
                    product.Features.Add(new ProductFeature
                    {
                        FeatureId = item
                    });
                }

            }



            _mapper.Map(productDto, product);

      
            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok(product);

            return BadRequest(new ProblemDetails { Title = "Problem updating product" });
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateProductMedia")]
        public async Task<ActionResult> UpdateProductMedia([FromForm] UpdateProductMediaDto productDto)
        {
            var product = await _context.Products.Include(x => x.Features).ThenInclude(y => y.Feature).FirstOrDefaultAsync(x => x.Id == productDto.Id);


            if (product == null) return NotFound();
   
            if (productDto.File != null)
            {
                var fileName = await WriteFile(productDto.File);

                if (fileName.Length == 0)
                    return BadRequest(new ProblemDetails { Title = "Problem uploading new image" });

                product.MediaList.Add(new Media
                {
                    MediaFileName = fileName
                });

            }

            _mapper.Map(productDto, product);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok(product);

            return BadRequest(new ProblemDetails { Title = "Problem updating product media" });
        }

        [HttpPost("UpdateProductComments")]
        public async Task<ActionResult> UpdateProductComments([FromForm] CreateProductCommentDto productDto)
        {
            var product = await _context.Products.Include(x => x.Features).ThenInclude(y => y.Feature).FirstOrDefaultAsync(x => x.Id == productDto.Id);


            if (product == null) return NotFound();

     
                product.CommentList.Add(new Comment
                {
                    Email=productDto.Email,
                    Name=productDto.Name,
                    Text=productDto.Text,
                    IsActive=false
                });

            _mapper.Map(productDto, product);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok(product);

            return BadRequest(new ProblemDetails { Title = "Problem updating product comment" });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null) return NotFound();

            if (!string.IsNullOrEmpty(product.PictureUrl))
            {

                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "..//var//lib//Upload//Images", product.PictureUrl);
                if (System.IO.File.Exists(filepath))    
                    System.IO.File.Delete(filepath);
            }

            if (product.MediaList.Count>0)
            {

            _context.MediaList.RemoveRange(product.MediaList);
            }



            _context.Products.Remove(product);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem deleting product" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateMultipleItems")]
        public async Task<ActionResult> UpdateProducts([FromBody] List<int> ids)
        {

            if (ids == null || ids.Count == 0) return NotFound();

            foreach (var id in ids)
            {
                var product = await _context.Products.FindAsync(id);

                if (product == null) return NotFound();

                product.IsActive = !product.IsActive;

            }

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem updating products" });
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("DeleteMultipleItems")]
        public async Task<ActionResult> DeleteProducts([FromBody] List<int> ids)
        {

            if (ids == null || ids.Count == 0) return NotFound();

            foreach (var id in ids)
            {

            var product = await _context.Products.FindAsync(id);

            if (product == null) return NotFound();

            if (!string.IsNullOrEmpty(product.PictureUrl))
            {

                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "..//var//lib//Upload//Images", product.PictureUrl);
                if (System.IO.File.Exists(filepath))
                    System.IO.File.Delete(filepath);
            }


            _context.Products.Remove(product);
            }


            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem deleting products" });
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

        private async Task<string> GetFilePath(string filename)
        {

            var filepath = Path.Combine(Directory.GetCurrentDirectory(), "..\\var\\lib\\Upload\\Images", filename);
            var provider = new FileExtensionContentTypeProvider();


            if (!provider.TryGetContentType(filepath, out var contenttype))
            {
                contenttype = "application/octet-stream";

            }

            var bytes = await System.IO.File.ReadAllBytesAsync(filepath);


            //return File(bytes, contenttype, Path.GetFileName(filepath));
            return filepath;

        }


        [NonAction]
         public void DeleteImage(string filename)
        {
            if (_hostEnvironment.IsDevelopment())
            {
                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "..\\var\\lib\\Upload\\Images", filename);

                if (System.IO.File.Exists(filepath))
                    System.IO.File.Delete(filepath);
            }
            else
            {
                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "..//var//lib//Upload//Images", filename);

                if (System.IO.File.Exists(filepath))
                    System.IO.File.Delete(filepath);

            }


        }
    }
}