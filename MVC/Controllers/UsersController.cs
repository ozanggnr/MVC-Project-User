    using APP.Models;
    using APP.Services;
    using CORE.APP.Services;
    using CORE.APP.Services.Authentication.MVC;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System;
    using System.Linq;

// Generated from Custom MVC Template.

    namespace MVC.Controllers
    {
        public class UsersController : Controller
        {
            // Service injections:
            private readonly IService<UserRequest, UserResponse> _userService;
            private readonly UserService _userServiceAuth;
            private readonly IService<CityRequest, CityResponse> _cityService;
            private readonly IService<GroupRequest, GroupResponse> _groupService;
            private readonly IService<RoleRequest, RoleResponse> _roleService;
            private const string AllowedCountryName = "Turkey";

            public UsersController(
			    IService<UserRequest, UserResponse> userService
                , UserService userServiceAuth
                , IService<CityRequest, CityResponse> cityService
                , IService<GroupRequest, GroupResponse> groupService
                , IService<RoleRequest, RoleResponse> roleService
            )
            {
                _userService = userService;
                _userServiceAuth = userServiceAuth;
                _cityService = cityService;
                _groupService = groupService;
                _roleService = roleService;
            }

            private void SetViewData()
            {
                /* 
                ViewBag and ViewData are the same collection (dictionary).
                They carry extra data other than the model from a controller action to its view, or between views.
                */

                // Related items service logic to set ViewData (Id and Name parameters may need to be changed in the SelectList constructor according to the model):
                var cities = (_cityService.List() ?? new List<CityResponse>())
                    .Where(city => string.Equals(city.Country?.CountryName, AllowedCountryName, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                var groups = _groupService.List() ?? new List<GroupResponse>();
                var roles = _roleService.List() ?? new List<RoleResponse>();

                ViewData["CityId"] = new SelectList(cities, "Id", "CityName");
                ViewData["GroupId"] = new SelectList(groups, "Id", "Title");
                ViewBag.RoleIds = new MultiSelectList(roles, "Id", "Name");
            }

            private void SetTempData(string message, string key = "Message")
            {
                /*
                TempData is used to carry extra data to the redirected controller action's view.
                */

                TempData[key] = message;
            }

        // GET: Users
        [AllowAnonymous]
        public IActionResult Index()
            {
                // Get collection service logic:
                var list = _userService.List();
                return View(list); // return response collection as model to the Index view
            }

            // GET: Users/Details/5
            [AllowAnonymous]
            public IActionResult Details(int id)
            {
                // Get item service logic:
                var item = _userService.Item(id);
                return View(item); // return response item as model to the Details view
            }

        // GET: Users/Create
        [Authorize]
        public IActionResult Create()
            {
                SetViewData(); // set ViewData dictionary to carry extra data other than the model to the view
                return View(); // return Create view with no model
            }

            // POST: Users/Create
            [HttpPost, ValidateAntiForgeryToken]
            [Authorize]
        public IActionResult Create(UserRequest user)
            {
                if (ModelState.IsValid) // check data annotation validation errors in the request
                {
                    // Insert item service logic:
                    var response = _userService.Create(user);
                    if (response.IsSuccessful)
                    {
                        SetTempData(response.Message); // set TempData dictionary to carry the message to the redirected action's view
                        return RedirectToAction(nameof(Details), new { id = response.Id }); // redirect to Details action with id parameter as response.Id route value
                    }
                    ModelState.AddModelError("", response.Message); // to display service error message in the validation summary of the view
                }
                SetViewData(); // set ViewData dictionary to carry extra data other than the model to the view
                return View(user); // return request as model to the Create view
            }

            // GET: Users/Edit/5
            [Authorize]
            public IActionResult Edit(int id)
            {
                // Get item to edit service logic:
                var item = _userService.Edit(id);
                SetViewData(); // set ViewData dictionary to carry extra data other than the model to the view
                return View(item); // return request as model to the Edit view
            }

            // POST: Users/Edit
            [HttpPost, ValidateAntiForgeryToken]
            [Authorize]
            public IActionResult Edit(UserRequest user)
            {
                if (ModelState.IsValid) // check data annotation validation errors in the request
                {
                    // Update item service logic:
                    var response = _userService.Update(user);
                    if (response.IsSuccessful)
                    {
                        SetTempData(response.Message); // set TempData dictionary to carry the message to the redirected action's view
                        return RedirectToAction(nameof(Details), new { id = response.Id }); // redirect to Details action with id parameter as response.Id route value
                    }
                    ModelState.AddModelError("", response.Message); // to display service error message in the validation summary of the view
                }
                SetViewData(); // set ViewData dictionary to carry extra data other than the model to the view
                return View(user); // return request as model to the Edit view
            }

            // GET: Users/Delete/5
            [Authorize]
            public IActionResult Delete(int id)
            {
                // Get item to delete service logic:
                var item = _userService.Item(id);
                return View(item); // return response item as model to the Delete view
            }

            // POST: Users/Delete
            [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
            [Authorize]
            public IActionResult DeleteConfirmed(int id)
            {
                // Delete item service logic:
                var response = _userService.Delete(id);
                SetTempData(response.Message); // set TempData dictionary to carry the message to the redirected action's view
                return RedirectToAction(nameof(Index)); // redirect to the Index action
            }

            // GET: Users/Register
            [AllowAnonymous]
            public IActionResult Register()
            {
                SetViewData(); // set ViewData dictionary to carry extra data other than the model to the view
                return View();
            }

            // POST: Users/Register
            [HttpPost, ValidateAntiForgeryToken]
            [AllowAnonymous]
            public IActionResult Register(UserRequest request)
            {
                // Set default values for registration
                request.IsActive = true;
                
                // If no roles specified, assign "User" role
                if (request.RoleIds == null || !request.RoleIds.Any())
                {
                    var userRole = _roleService.List()?.FirstOrDefault(r => r.Name == "User");
                    if (userRole != null)
                    {
                        request.RoleIds = new List<int> { userRole.Id };
                    }
                }

                // Remove RoleIds from ModelState to bypass required validation for registration
                ModelState.Remove("RoleIds");

                if (ModelState.IsValid)
                {
                    var response = _userService.Create(request);
                    if (response.IsSuccessful)
                    {
                        SetTempData(response.Message);
                        return RedirectToAction(nameof(Login));
                    }
                    ModelState.AddModelError("", response.Message);
                }
                SetViewData(); // set ViewData dictionary to carry extra data other than the model to the view
                return View(request);
            }

            // GET: Users/Login
            [AllowAnonymous]
            [Route("/Login")]
            public IActionResult Login()
            {
                return View();
            }

            // POST: Users/Login
            [HttpPost, ValidateAntiForgeryToken]
            [AllowAnonymous]
            [Route("/Login")]
            public async Task<IActionResult> Login(UserLoginRequest request)
            {
                if (ModelState.IsValid)
                {
                    var response = await _userServiceAuth.Login(request);
                    if (response.IsSuccessful)
                    {
                        SetTempData(response.Message);
                        return RedirectToAction(nameof(Index));
                    }
                    ModelState.AddModelError("", response.Message);
                }
                return View(request);
            }

            // GET: Users/Logout
            public async Task<IActionResult> Logout()
            {
                await _userServiceAuth.Logout();
                SetTempData("User logged out successfully.");
                return RedirectToAction(nameof(Login));
            }
        }
    }
