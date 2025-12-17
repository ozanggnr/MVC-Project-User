#nullable disable
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CORE.APP.Services;
using APP.Models;

// Generated from Custom MVC Template.

namespace MVC.Controllers
{
    public class CitiesController : Controller
    {
        // Service injections:
        private readonly IService<CityRequest, CityResponse> _cityService;
        private readonly IService<CountryRequest, CountryResponse> _countryService;

        /* Can be uncommented and used for many to many relationships, "entity" may be replaced with the related entity name in the controller and views. */
        //private readonly IService<EntityRequest, EntityResponse> _EntityService;

        public CitiesController(
			IService<CityRequest, CityResponse> cityService
            , IService<CountryRequest, CountryResponse> countryService

            /* Can be uncommented and used for many to many relationships, "entity" may be replaced with the related entity name in the controller and views. */
            //, IService<EntityRequest, EntityResponse> EntityService
        )
        {
            _cityService = cityService;
            _countryService = countryService;

            /* Can be uncommented and used for many to many relationships, "entity" may be replaced with the related entity name in the controller and views. */
            //_EntityService = EntityService;
        }

        private void SetViewData()
        {
            /* 
            ViewBag and ViewData are the same collection (dictionary).
            They carry extra data other than the model from a controller action to its view, or between views.
            */

            // Related items service logic to set ViewData (Id and Name parameters may need to be changed in the SelectList constructor according to the model):
            var countries = _countryService.List() ?? new List<CountryResponse>();
            ViewData["CountryId"] = new SelectList(countries, "Id", "CountryName");
        }

        private void SetTempData(string message, string key = "Message")
        {
            /*
            TempData is used to carry extra data to the redirected controller action's view.
            */

            TempData[key] = message;
        }

        // GET: Cities
        [AllowAnonymous]
        public IActionResult Index()
        {
            // Get collection service logic:
            var list = _cityService.List();
            return View(list); // return response collection as model to the Index view
        }

        // GET: Cities/Details/5
        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            // Get item service logic:
            var item = _cityService.Item(id);
            return View(item); // return response item as model to the Details view
        }

        // GET: Cities/Create
        [Authorize]
        public IActionResult Create()
        {
            SetViewData(); // set ViewData dictionary to carry extra data other than the model to the view
            return View(); // return Create view with no model
        }

        // POST: Cities/Create
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Create(CityRequest city)
        {
            if (ModelState.IsValid) // check data annotation validation errors in the request
            {
                // Insert item service logic:
                var response = _cityService.Create(city);
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message); // set TempData dictionary to carry the message to the redirected action's view
                    return RedirectToAction(nameof(Details), new { id = response.Id }); // redirect to Details action with id parameter as response.Id route value
                }
                ModelState.AddModelError("", response.Message); // to display service error message in the validation summary of the view
            }
            SetViewData(); // set ViewData dictionary to carry extra data other than the model to the view
            return View(city); // return request as model to the Create view
        }

        // GET: Cities/Edit/5
        [Authorize]
        public IActionResult Edit(int id)
        {
            // Get item to edit service logic:
            var item = _cityService.Edit(id);
            SetViewData(); // set ViewData dictionary to carry extra data other than the model to the view
            return View(item); // return request as model to the Edit view
        }

        // POST: Cities/Edit
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Edit(CityRequest city)
        {
            if (ModelState.IsValid) // check data annotation validation errors in the request
            {
                // Update item service logic:
                var response = _cityService.Update(city);
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message); // set TempData dictionary to carry the message to the redirected action's view
                    return RedirectToAction(nameof(Details), new { id = response.Id }); // redirect to Details action with id parameter as response.Id route value
                }
                ModelState.AddModelError("", response.Message); // to display service error message in the validation summary of the view
            }
            SetViewData(); // set ViewData dictionary to carry extra data other than the model to the view
            return View(city); // return request as model to the Edit view
        }

        // GET: Cities/Delete/5
        [Authorize]
        public IActionResult Delete(int id)
        {
            // Get item to delete service logic:
            var item = _cityService.Item(id);
            return View(item); // return response item as model to the Delete view
        }

        // POST: Cities/Delete
        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        [Authorize]
        public IActionResult DeleteConfirmed(int id)
        {
            // Delete item service logic:
            var response = _cityService.Delete(id);
            SetTempData(response.Message); // set TempData dictionary to carry the message to the redirected action's view
            return RedirectToAction(nameof(Index)); // redirect to the Index action
        }
    }
}
