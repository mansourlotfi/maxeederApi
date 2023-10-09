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
    public class SocialNetworkController:BaseApiController
    {
        private readonly StoreContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostEnvironment;

        public SocialNetworkController(StoreContext context, IMapper mapper, IWebHostEnvironment hostEnvironment)
        {
            _mapper = mapper;
            this._hostEnvironment = hostEnvironment;
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<PagedList<SocialNetwork>>> GetSocialNetworks([FromQuery] PaginationParams? paginationParams)
        {
            // return await _context.Products.ToListAsync();
            var query = _context.SocialNetworks.Select(x => new SocialNetwork()
            {
                Id = x.Id,
                Name = x.Name,
                Link=x.Link,
                PictureUrl = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.PictureUrl),
                Priority = x.Priority,
                IsActive=x.IsActive,
            }).AsQueryable();

            var socialNetwork = await PagedList<SocialNetwork>.ToPagedList(query, paginationParams.PageNumber, paginationParams.PageSize);

            Response.AddPaginationHeader(socialNetwork.MetaData);

            return socialNetwork;

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<SocialNetwork>> CreateSocialNetwork([FromForm] SocialNetworkDto socialNetworkDto)
        {
            var existing = await _context.SocialNetworks.FirstOrDefaultAsync(x => x.Priority == socialNetworkDto.Priority);
            if (existing != null) return BadRequest(new ProblemDetails { Title = "Item with this priority exist" });

            var socialNetwork = _mapper.Map<SocialNetwork>(socialNetworkDto);

        

            if (socialNetworkDto.File != null)
            {
                var fileName = await WriteFile(socialNetworkDto.File);

                if (fileName.Length == 0)
                    return BadRequest(new ProblemDetails { Title = "Problem uploading new image" });

                socialNetwork.PictureUrl = fileName;

            }

            _context.SocialNetworks.Add(socialNetwork);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok(socialNetwork);

            return BadRequest(new ProblemDetails { Title = "Problem creating new Social Network" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult<SocialNetwork>> UpdateSocialNetwork([FromForm] UpdateSocialNetworkDto updateSocialNetworkDto)
        {
            var socialNetwork = await _context.SocialNetworks.FindAsync(updateSocialNetworkDto.Id);


            if (socialNetwork == null) return NotFound();

            var existing = await _context.SocialNetworks.FirstOrDefaultAsync(x => x.Priority == updateSocialNetworkDto.Priority && updateSocialNetworkDto.Priority != socialNetwork.Priority);
            if (existing != null) return BadRequest(new ProblemDetails { Title = "Item with this priority exist" });

            if (updateSocialNetworkDto.File != null)
            {
                var fileName = await WriteFile(updateSocialNetworkDto.File);

                if (fileName.Length == 0)
                    return BadRequest(new ProblemDetails { Title = "Problem uploading new image" });

                socialNetwork.PictureUrl = fileName;

            }
            else
            {
                socialNetwork.PictureUrl = socialNetwork.PictureUrl;

            }

            _mapper.Map(updateSocialNetworkDto, socialNetwork);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok(socialNetwork);

            return BadRequest(new ProblemDetails { Title = "Problem updating social network" });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSocialNetwork(int id)
        {
            var socialNetwork = await _context.SocialNetworks.FindAsync(id);

            if (socialNetwork == null) return NotFound();

            if (!string.IsNullOrEmpty(socialNetwork.PictureUrl))
            {

                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "..//var//lib//Upload//Images", socialNetwork.PictureUrl);
                if (System.IO.File.Exists(filepath))
                    System.IO.File.Delete(filepath);
            }


            _context.SocialNetworks.Remove(socialNetwork);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem deleting social network" });
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateMultipleItems")]
        public async Task<ActionResult> UpdateSocialNetworks([FromBody] List<int> ids)
        {

            if (ids == null || ids.Count == 0) return NotFound();

            foreach (var id in ids)
            {
                var item = await _context.SocialNetworks.FindAsync(id);

                if (item == null) return NotFound();

                item.IsActive = !item.IsActive;

            }

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem updating Social Network" });
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("DeleteMultipleItems")]
        public async Task<ActionResult> DeleteSocialNetworks([FromBody] List<int> ids)
        {

            if (ids == null || ids.Count == 0) return NotFound();

            foreach (var id in ids)
            {

                var item = await _context.SocialNetworks.FindAsync(id);

                if (item == null) return NotFound();

                if (!string.IsNullOrEmpty(item.PictureUrl))
                {

                    var filepath = Path.Combine(Directory.GetCurrentDirectory(), "..//var//lib//Upload//Images", item.PictureUrl);
                    if (System.IO.File.Exists(filepath))
                        System.IO.File.Delete(filepath);
                }


                _context.SocialNetworks.Remove(item);
            }


            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem Social Network" });
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
