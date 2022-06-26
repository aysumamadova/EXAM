using Exam._7.DAL;
using Exam._7.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Exam._7.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TeamController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public TeamController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Teams.ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Team model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadedFile(model);
                Team speaker = new Team
                {
                    Name = model.Name,
                    Work = model.Work,
                    Img = uniqueFileName
                };

                _context.Add(speaker);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        private string ProcessUploadedFile(Team model)
        {
            string uniqueFileName = null;

            if (model.Photo != null)
            {
                string uploadsFolder = Path.Combine(_env.WebRootPath, "assets/img");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var speaker = await _context.Teams.FindAsync(id);
            var team = new Team()
            {
                Id = speaker.Id,
                Name = speaker.Name,
                Work = speaker.Work,
                Img = speaker.Img
            };

            if (speaker == null)
            {
                return NotFound();
            }
            return View(team);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Team model)
        {
            if (ModelState.IsValid)
            {
                var speaker = await _context.Teams.FindAsync(model.Id);
                speaker.Name = model.Name;
                speaker.Work = model.Work;

                if (model.Photo != null)
                {
                    if (model.Img != null)
                    {
                        string filePath = Path.Combine(_env.WebRootPath, "assets/img", model.Img);
                        System.IO.File.Delete(filePath);
                    }

                    speaker.Img = ProcessUploadedFile(model);
                }
                _context.Update(speaker);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            Team team = _context.Teams.Find(id);
            _context.Teams.Remove(team);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
