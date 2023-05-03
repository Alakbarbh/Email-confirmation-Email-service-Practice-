using EntityFramework_Slider.Areas.Admin.ViewModels;
using EntityFramework_Slider.Data;
using EntityFramework_Slider.Helpers;
using EntityFramework_Slider.Models;
using EntityFramework_Slider.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework_Slider.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderInfoController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ISliderService _sliderService;
        private readonly IWebHostEnvironment _env;
        public SliderInfoController(AppDbContext context, 
                                    ISliderService sliderService,
                                    IWebHostEnvironment env)
        {
            _context = context;
            _sliderService = sliderService;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<SliderInfo> sliderInfos = await _context.SliderInfos.ToListAsync();
            return View(sliderInfos);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            SliderInfo sliderInfo = await _context.SliderInfos.FirstOrDefaultAsync(m => m.Id == id);
            if (sliderInfo is null) return NotFound();
            return View(sliderInfo);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }










        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderInfo sliderInfo)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(sliderInfo);
                }



                if (!sliderInfo.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "File type must be image");
                    return View();
                }




                if (!sliderInfo.Photo.CheckFileSize(200))
                {
                    ModelState.AddModelError("Photo", "Image size must be max 200kb");
                    return View();
                }



                string fileName = Guid.NewGuid().ToString() + "_" + sliderInfo.Photo.FileName;


                string path = FileHelper.GetFilePath(_env.WebRootPath, "img", fileName);

                await FileHelper.SaveFileAsync(path, sliderInfo.Photo);

                sliderInfo.SignatureImage = fileName;

                await _context.SliderInfos.AddAsync(sliderInfo);

                await _context.SaveChangesAsync();


                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null) return BadRequest();

                SliderInfo sliderInfo = await _context.SliderInfos.FirstOrDefaultAsync(m => m.Id == id);

                if (sliderInfo is null) return NotFound();

                string path = FileHelper.GetFilePath(_env.WebRootPath, "img", sliderInfo.SignatureImage);

                FileHelper.DeleteFile(path);

                _context.SliderInfos.Remove(sliderInfo);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                throw;
            }
        }





        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest();
            SliderInfo dbSliderInfo = await _context.SliderInfos.FirstOrDefaultAsync(m => m.Id == id);
            if (dbSliderInfo is null) return NotFound();

            SliderInfoUpdateVM model = new()
            {
                SignatureImage = dbSliderInfo.SignatureImage,
                Title = dbSliderInfo.Title,
                Description = dbSliderInfo.Description
            };

            return View(model);

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, SliderInfoUpdateVM sliderInfo)
        {
            try
            {

                if (id == null) return BadRequest();

                SliderInfo dbSliderInfo = await _context.SliderInfos.FirstOrDefaultAsync(m => m.Id == id);

                if (dbSliderInfo is null) return NotFound();

                SliderInfoUpdateVM model = new()
                {
                    SignatureImage = dbSliderInfo.SignatureImage,
                    Title = dbSliderInfo.Title,
                    Description = dbSliderInfo.Description
                };


                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                if (sliderInfo.Photo != null)
                {
                    if (!sliderInfo.Photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photo", "Please choose correct image type");
                        return View(model);
                    }

                    if (!sliderInfo.Photo.CheckFileSize(200))
                    {
                        ModelState.AddModelError("Photo", "Image size must be max 200kb");
                        return View(model);
                    }


                    string dbPath = FileHelper.GetFilePath(_env.WebRootPath, "img", dbSliderInfo.SignatureImage);

                    FileHelper.DeleteFile(dbPath);


                    string fileName = Guid.NewGuid().ToString() + "_" + sliderInfo.Photo.FileName;

                    string newPath = FileHelper.GetFilePath(_env.WebRootPath, "img", fileName);

                    await FileHelper.SaveFileAsync(newPath, sliderInfo.Photo);

                    dbSliderInfo.SignatureImage = fileName;
                }
                else
                {
                    SliderInfo newSlider = new()
                    {
                        SignatureImage = dbSliderInfo.SignatureImage
                    };
                }


                dbSliderInfo.Title = sliderInfo.Title;
                dbSliderInfo.Description = sliderInfo.Description;



                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                @ViewBag.error = ex.Message;
                return View();
            }
        }


  


    }
}

