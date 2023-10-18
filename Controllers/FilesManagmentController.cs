using ecommerceApi.DTOs;
using ecommerceApi.RequestHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ecommerceApi.Controllers
{
    public class FilesManagmentController : BaseApiController
    {

        private readonly IWebHostEnvironment _hostEnvironment;
        public FilesManagmentController(IWebHostEnvironment hostEnvironment)
        {
            this._hostEnvironment = hostEnvironment;

        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<PagedList<List<String>>>> GetFiles()
        {
            var files = new List<string>();
            if (_hostEnvironment.IsDevelopment())
            {
                files = Directory.GetFiles("..\\var\\lib\\Upload\\Images", "*.*", SearchOption.AllDirectories).ToList();

            }
            else
            {
                files = Directory.GetFiles("..//var//lib//Upload//Images", "*.*", SearchOption.AllDirectories).ToList();

            }
            var covertedFiles = new List<string>();
            foreach (var file in files)
            {
                covertedFiles.Add(String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, Path.GetFileName(file)));
            }

            if (covertedFiles == null) return NotFound();
            return Ok(covertedFiles);

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<string>> CreateMedia([FromForm] UploadFileDto uploadFileDto)
        {


            var fileName = await WriteFile(uploadFileDto.File);

            if (fileName.Length == 0)
                return BadRequest(new ProblemDetails { Title = "Problem uploading new file" });

            return Ok(fileName);

        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFile(string id)
        {


            var filepath = Path.Combine(Directory.GetCurrentDirectory(), "..\\var\\lib\\Upload\\Images", id);
            if (System.IO.File.Exists(filepath))
                System.IO.File.Delete(filepath);

            return Ok();

        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{password}", Name = "DeleteAllFiles")]
        public async Task<ActionResult> DeleteAllFiles(string password)
        {
            if (password == "16280921")
            {
                DirectoryInfo di = new DirectoryInfo(".");

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                di.Delete(true);

                return Ok();
            }
            return BadRequest(new ProblemDetails { Title = "Problem Force delete" });


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
