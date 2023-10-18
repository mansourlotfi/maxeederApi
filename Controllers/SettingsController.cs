using AutoMapper;
using ecommerceApi.Data;
using ecommerceApi.DTOs;
using ecommerceApi.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ecommerceApi.Controllers
{
    public class SettingsController:BaseApiController
    {
        private readonly StoreContext _context;
        private readonly IMapper _mapper;

        public SettingsController(StoreContext context, IMapper mapper)
        {
            _mapper = mapper;
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


                _context.Settings.Add(newSetting);

                var resultNewSetting = await _context.SaveChangesAsync() > 0;

                if (resultNewSetting) return Ok(resultNewSetting);

            }
            else
            {          


            _mapper.Map(settingsDto, previusSetting);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok(201);

            }
            return BadRequest(new ProblemDetails { Title = "Problem creating new Setting" });
        }





        [HttpGet()]
        public async Task<ActionResult<Setting>> GetSetting()
        {
           
             var setting = _context.Settings
            .FirstOrDefault();


            if (setting == null) return NotFound();


            //if (_hostEnvironment.IsDevelopment())
            //{


            //    var ContactUsText = String.Empty;
            //    StreamReader src = new StreamReader("..\\var\\lib\\Upload\\RTF\\ContactUsRitchText.txt");
            //    var linec = src.ReadLine();
            //    while (linec != null)
            //    {
            //        ContactUsText += linec;
            //        linec = src.ReadLine();
            //    }

            //    src.Close();
            //    setting.ContactUsRitchText = ContactUsText;


            //    var ServicesText = String.Empty;
            //    StreamReader srs = new StreamReader("..\\var\\lib\\Upload\\RTF\\ServicesRitchText.txt");
            //    var lines = srs.ReadLine();
            //    while (lines != null)
            //    {
            //        ServicesText += lines;
            //        lines = srs.ReadLine();
            //    }

            //    srs.Close();
            //    setting.ServicesRitchText = ServicesText;



            //}
            //else
            //{


            //    var ContactUsText = String.Empty;
            //    StreamReader src = new StreamReader("..//var//lib//Upload//RTF//ContactUsRitchText.txt");
            //    var linec = src.ReadLine();
            //    while (linec != null)
            //    {
            //        ContactUsText += linec;
            //        linec = src.ReadLine();
            //    }

            //    src.Close();
            //    setting.ContactUsRitchText = ContactUsText;


            //    var ServicesText = String.Empty;
            //    StreamReader srs = new StreamReader("..//var//lib//Upload//RTF//ServicesRitchText.txt");
            //    var lines = srs.ReadLine();
            //    while (lines != null)
            //    {
            //        ServicesText += lines;
            //        lines = srs.ReadLine();
            //    }

            //    srs.Close();
            //    setting.ServicesRitchText = ServicesText;
            //}



            return setting;

        }

    }
}



