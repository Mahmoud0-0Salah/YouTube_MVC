using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using outTube.Models;
using outTube.ViewModels;

namespace outTube.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProfileController(UserManager<User> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var model = new ProfileViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Gender = user.Gender,
                DateOfBirth = user.DateOfBirth,
                ImageUrl = user.ImageUrl
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProfileViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            if (ModelState.IsValid)
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Gender = model.Gender;
                user.DateOfBirth = model.DateOfBirth;

                if (model.ImageFile != null)
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "profiles");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(fileStream);
                    }

                    user.ImageUrl = "/images/profiles/" + fileName;
                }

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    TempData["Message"] = "Profile updated successfully!";
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View("Index", model);
        }
    }
}
