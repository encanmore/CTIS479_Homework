using APP.Services;
using Microsoft.AspNetCore.Mvc;
using APP.Domain;
using APP.Models;

namespace MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            var list = _userService.Query<Group>().ToList();

            ViewBag.Count = list.Count == 0 ? "No users found!" : list.Count == 1 ? "1 user found." : $"{list.Count} users found.";

            return View(list);
        }

        public IActionResult Details(int id)
        {
            var item = _userService.Query<Group>().SingleOrDefault(group => group.Id == id);

            if (item is null)
                ViewBag.Message = "User not found!";

            return View(item);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(UserRequest request)
        {
            if (ModelState.IsValid)
            {
                var response = _userService.Create(request);

                if (response.IsSuccessful)
                {
                    TempData["Message"] = response.Message;

                    return RedirectToAction(nameof(Index));
                }

                ViewBag.Message = response.Message;
            }

            return View(request);
        }

        public IActionResult Edit(int id)
        {
            var request = _userService.Edit(id);

            if (request is null)
                ViewBag.Message = "User not found!";

            return View(request);
        }

        [HttpPost]
        public IActionResult Edit(UserRequest request)
        {
            if (ModelState.IsValid)
            {
                var response = _userService.Update(request);

                if (response.IsSuccessful)
                    return RedirectToAction(nameof(Details), new { id = response.Id });

                ViewBag.Message = response.Message;
            }

            return View(request);
        }

        public IActionResult Delete(int id)
        {
            var response = _userService.Delete(id);

            TempData["Message"] = response.Message;

            return RedirectToAction(nameof(Index));
        }
    }
}