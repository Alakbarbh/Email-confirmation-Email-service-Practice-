using EntityFramework_Slider.Data;
using EntityFramework_Slider.Models;
using EntityFramework_Slider.Services;
using EntityFramework_Slider.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework_Slider.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ExpertHeaderController : Controller
    {
        private readonly IFlowerService _flowerService;
        private readonly AppDbContext _context;
        public ExpertHeaderController(IFlowerService flowerService,
                                  AppDbContext context)
        {
            _flowerService = flowerService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _flowerService.GetInfos());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExpertHeader expertHeader)
        {
            try
            {
                var existData = await _context.ExpertHeaders.FirstOrDefaultAsync(m => m.Title.Trim().ToLower() == expertHeader.Description.Trim().ToLower());
                if (existData is not null)
                {
                    ModelState.AddModelError("Name", "This data already exist");
                    return View();
                }


                await _context.ExpertHeaders.AddAsync(expertHeader);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { msj = ex.Message });
            }
        }

        public IActionResult Error(string msj)
        {
            ViewBag.error = msj;
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest();
            ExpertHeader expertHeaders = await _context.ExpertHeaders.FindAsync(id);
            if (expertHeaders is null) return NotFound();
            _context.ExpertHeaders.Remove(expertHeaders);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SoftDelete(int? id)
        {
            if (id is null) return BadRequest();
            ExpertHeader expertHeaders = await _context.ExpertHeaders.FindAsync(id);
            if (expertHeaders is null) return NotFound();
            expertHeaders.SoftDelete = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();
            ExpertHeader expertHeader = await _context.ExpertHeaders.FindAsync(id);
            if (expertHeader is null) return NotFound();
            return View(expertHeader);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int? id, ExpertHeader expertHeader)
        {
            if (id is null) return BadRequest();
            ExpertHeader dbExpertHeader = await _context.ExpertHeaders.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
            if (dbExpertHeader is null) return NotFound();

            if (dbExpertHeader.Title.Trim().ToLower() == expertHeader.Title.Trim().ToLower())
            {
                return RedirectToAction(nameof(Index));
            }


            _context.ExpertHeaders.Update(expertHeader);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();
            ExpertHeader expertHeaders = await _context.ExpertHeaders.FindAsync(id);
            if (expertHeaders is null) return NotFound();
            return View(expertHeaders);
        }

    }
}
