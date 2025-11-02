using APP.Services;
using Microsoft.AspNetCore.Mvc;
using APP.Domain;
using APP.Models;

namespace MVC.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleService _roleService;

        public RoleController(RoleService roleService)
        {
            _roleService = roleService;
        }

        public IActionResult Index()
        {
            var list = _roleService.Query<Role>().ToList();

            ViewBag.Count = list.Count == 0 ? "No roles found!" : list.Count == 1 ? "1 role found." : $"{list.Count} roles found.";

            return View(list);
        }

        public IActionResult Details(int id)
        {
            var item = _roleService.Query<Role>().SingleOrDefault(role => role.Id == id);

            if (item is null)
                ViewBag.Message = "Role not found!";

            return View(item);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(RoleRequest request)
        {
            if (ModelState.IsValid)
            {
                var response = _roleService.Create(request);

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
            var request = _roleService.Edit(id);

            if (request is null)
                ViewBag.Message = "Role not found!";

            return View(request);
        }

        [HttpPost]
        public IActionResult Edit(RoleRequest request)
        {
            if (ModelState.IsValid)
            {
                var response = _roleService.Update(request);

                if (response.IsSuccessful)
                    return RedirectToAction(nameof(Details), new { id = response.Id });

                ViewBag.Message = response.Message;
            }

            return View(request);
        }

        public IActionResult Delete(int id)
        {
            var response = _roleService.Delete(id);

            TempData["Message"] = response.Message;

            return RedirectToAction(nameof(Index));
        }
    }
}