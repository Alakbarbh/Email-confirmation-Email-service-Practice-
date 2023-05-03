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
    public class ExpertController : Controller
    {


        private readonly AppDbContext _context;
        private readonly IFlowerService _flowerService;
        private readonly IWebHostEnvironment _env;
        public ExpertController(AppDbContext context,
                                IFlowerService flowerService,
                                IWebHostEnvironment env)
        {
            _context = context;
            _flowerService = flowerService;
            _env = env;
        }




        public async Task<IActionResult> Index()
        {
            IEnumerable<Expert> experts = await _context.Experts.ToListAsync();
            return View(experts);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            Expert expert = await _context.Experts.FirstOrDefaultAsync(m => m.Id == id);
            if (expert is null) return NotFound();
            return View(expert);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }









        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Expert expert)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(expert);
                }



                if (!expert.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "File type must be image");
                    return View();
                }




                if (!expert.Photo.CheckFileSize(200))
                {
                    ModelState.AddModelError("Photo", "Image size must be max 200kb");
                    return View();
                }



                string fileName = Guid.NewGuid().ToString() + "_" + expert.Photo.FileName;


                string path = FileHelper.GetFilePath(_env.WebRootPath, "img", fileName);

                await FileHelper.SaveFileAsync(path, expert.Photo);

                expert.Image = fileName;

                await _context.Experts.AddAsync(expert);

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

                Expert expert = await _context.Experts.FirstOrDefaultAsync(m => m.Id == id);

                if (expert is null) return NotFound();

                string path = FileHelper.GetFilePath(_env.WebRootPath, "img", expert.Image);

                FileHelper.DeleteFile(path);

                _context.Experts.Remove(expert);

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
            Expert dbexpert = await _context.Experts.FirstOrDefaultAsync(m => m.Id == id);
            if (dbexpert is null) return NotFound();

            ExpertUpdateVM model = new()
            {
                Image = dbexpert.Image,
                Name = dbexpert.Name,
                Profession = dbexpert.Profession,
            };

            return View(model);

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, ExpertUpdateVM expertUpdate)
        {
            try
            {

                if (id == null) return BadRequest();

                Expert dbexpert = await _context.Experts.FirstOrDefaultAsync(m => m.Id == id);

                if (dbexpert is null) return NotFound();

                ExpertUpdateVM model = new()
                {
                    Image = dbexpert.Image,
                    Name = dbexpert.Name,
                    Profession = dbexpert.Profession,
                };


                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                if (expertUpdate.Photo != null)
                {
                    if (!expertUpdate.Photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photo", "Please choose correct image type");
                        return View(model);
                    }

                    if (!expertUpdate.Photo.CheckFileSize(200))
                    {
                        ModelState.AddModelError("Photo", "Image size must be max 200kb");
                        return View(model);
                    }


                    string dbPath = FileHelper.GetFilePath(_env.WebRootPath, "img", dbexpert.Image);

                    FileHelper.DeleteFile(dbPath);


                    string fileName = Guid.NewGuid().ToString() + "_" + expertUpdate.Photo.FileName;

                    string newPath = FileHelper.GetFilePath(_env.WebRootPath, "img", fileName);

                    await FileHelper.SaveFileAsync(newPath, expertUpdate.Photo);

                    dbexpert.Image = fileName;
                }
                else
                {
                    Expert expert = new()
                    {
                        Image = dbexpert.Image
                    };
                }


                dbexpert.Name = expertUpdate.Name;
                dbexpert.Profession = expertUpdate.Profession;

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
