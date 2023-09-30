using AutoMapper;
using ecommerceApi.Data;
using ecommerceApi.DTOs;
using ecommerceApi.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace ecommerceApi.Controllers
{
    public class SettingsController:BaseApiController
    {
        private readonly StoreContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IMapper _mapper;

        public SettingsController(StoreContext context, IMapper mapper, IWebHostEnvironment hostEnvironment)
        {
            _mapper = mapper;
            this._hostEnvironment = hostEnvironment;
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost()]
        public async Task<ActionResult<Setting>> CreateProduct([FromForm] CreateSettingsDto settingsDto)
        {
            var previusSetting = await _context.Settings.FirstOrDefaultAsync();

            if (previusSetting == null)
            {

                var newSetting = _mapper.Map<Setting>(settingsDto);             

                if (settingsDto.File != null)
                {
                    var fileName = await WriteFile(settingsDto.File);

                    if (fileName.Length == 0)
                        return BadRequest(new ProblemDetails { Title = "Problem uploading new image" });

                    newSetting.ServicePictureUrl = fileName;

                }

                _context.Settings.Add(newSetting);

                var resultNewSetting = await _context.SaveChangesAsync() > 0;

                if (resultNewSetting) return Ok(resultNewSetting);

            }
            else
            {          

            if (settingsDto.File != null)
            {
                var fileName = await WriteFile(settingsDto.File);

                if (fileName.Length == 0)
                    return BadRequest(new ProblemDetails { Title = "Problem uploading new image" });

                previusSetting.ServicePictureUrl = fileName;

            }
            else
            {
                previusSetting.ServicePictureUrl = previusSetting.ServicePictureUrl;

            }


           

            if (_hostEnvironment.IsDevelopment())
            {
                bool exists = System.IO.Directory.Exists("..\\var\\lib\\Upload\\RTF");
                if (!exists)
                    System.IO.Directory.CreateDirectory("..\\var\\lib\\Upload\\RTF");
                var ContactUsPath = "..\\var\\lib\\Upload\\RTF\\ContactUsRitchText.txt";
                System.IO.File.WriteAllText(ContactUsPath, String.Empty);
                using StreamWriter swc = new StreamWriter(ContactUsPath);
                swc.Write(settingsDto.ContactUsRitchText);
                swc.Close();


                var ServicesPath = "..\\var\\lib\\Upload\\RTF\\ServicesRitchText.txt";
                System.IO.File.WriteAllText(ServicesPath, String.Empty);
                using StreamWriter sws = new StreamWriter(ServicesPath);
                sws.Write(settingsDto.ServicesRitchText);
                sws.Close();

            }
            else
            {
                bool exists = System.IO.Directory.Exists("..//var//lib//Upload//RTF");
                if (!exists)
                    System.IO.Directory.CreateDirectory("..//var//lib//Upload//RTF");

                var ContactUsPath = "..//var//lib//Upload//RTF//ContactUsRitchText.txt";
                System.IO.File.WriteAllText(ContactUsPath, String.Empty);
                using StreamWriter swc = new StreamWriter(ContactUsPath);
                swc.Write(settingsDto.ContactUsRitchText);
                swc.Close();

                var ServicesPath = "..//var//lib//Upload//RTF//ServicesRitchText.txt";
                System.IO.File.WriteAllText(ServicesPath, String.Empty);
                using StreamWriter sws = new StreamWriter(ServicesPath);
                sws.Write(settingsDto.ServicesRitchText);
                sws.Close();

            }

            _mapper.Map(settingsDto, previusSetting);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok(previusSetting);

            }
            return BadRequest(new ProblemDetails { Title = "Problem creating new Setting" });
        }





        [HttpGet()]
        public async Task<ActionResult<Setting>> GetSetting()
        {
           
             var setting = _context.Settings
            .FirstOrDefault();


            if (setting == null) return NotFound();


            if (_hostEnvironment.IsDevelopment())
            {


                var ContactUsText = String.Empty;
                StreamReader src = new StreamReader("..\\var\\lib\\Upload\\RTF\\ContactUsRitchText.txt");
                var linec = src.ReadLine();
                while (linec != null)
                {
                    ContactUsText += linec;
                    linec = src.ReadLine();
                }

                src.Close();
                setting.ContactUsRitchText = ContactUsText;


                var ServicesText = String.Empty;
                StreamReader srs = new StreamReader("..\\var\\lib\\Upload\\RTF\\ServicesRitchText.txt");
                var lines = srs.ReadLine();
                while (lines != null)
                {
                    ServicesText += lines;
                    lines = srs.ReadLine();
                }

                srs.Close();
                setting.ServicesRitchText = ServicesText;



            }
            else
            {


                var ContactUsText = String.Empty;
                StreamReader src = new StreamReader("..//var//lib//Upload//RTF//ContactUsRitchText.txt");
                var linec = src.ReadLine();
                while (linec != null)
                {
                    ContactUsText += linec;
                    linec = src.ReadLine();
                }

                src.Close();
                setting.ContactUsRitchText = ContactUsText;


                var ServicesText = String.Empty;
                StreamReader srs = new StreamReader("..//var//lib//Upload//RTF//ServicesRitchText.txt");
                var lines = srs.ReadLine();
                while (lines != null)
                {
                    ServicesText += lines;
                    lines = srs.ReadLine();
                }

                srs.Close();
                setting.ServicesRitchText = ServicesText;
            }



            return setting;

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



