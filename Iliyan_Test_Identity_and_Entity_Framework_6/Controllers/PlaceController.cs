using Iliyan_Test_Identity_and_Entity_Framework_6.Data;
using Iliyan_Test_Identity_and_Entity_Framework_6.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Iliyan_Test_Identity_and_Entity_Framework_6.Controllers
{
    [Authorize]
    public class PlaceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlaceController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Index page with all the <see cref="IPlace"/> that are inside the database, place into <see cref="DisplayPlace"/> objects.
        /// </summary>
        /// <returns>The Index View with the datatable filled with <see cref="IPlace"/></returns>
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            List<DisplayPlace> displayPlaces = new List<DisplayPlace>();

             await _context.Countries.ForEachAsync(country => {
                DisplayPlace displayPlace = new DisplayPlace();
                displayPlace.CountryId = country.Id;
                displayPlace.CountryName = country.Name;
                displayPlaces.Add(displayPlace);
            }
            );
             await _context.Cities.ForEachAsync(city =>
            {
                DisplayPlace displayPlace = new DisplayPlace();
                displayPlace.CityId = city.Id;
                displayPlace.CountryId = city.CountryId;
                displayPlace.CountryName = city.Country.Name;
                displayPlace.CityName = city.Name;
                displayPlaces.Add(displayPlace);
            });
            return View(displayPlaces);
        }

        /// <summary>
        /// Action that lead to the form for adding a <see cref="Country"/>.
        /// </summary>
        /// <returns>The form for adding <see cref="Country"/></returns>
        public IActionResult AddCountry()
        {
            return View();
        }

        /// <summary>
        /// Action that leads to the addition of a <see cref="Country"/> within the <see cref="ApplicationDbContext"/> and the database.
        /// </summary>
        /// <param name="newCountry">the new <see cref="Country"/> object.</param>
        /// <returns>Return to the <see cref="Index"/> page</returns>
        [HttpPost]
        public async Task<IActionResult> AddCountry(Country newCountry)
        {
            if(!string.IsNullOrEmpty(newCountry.Name) && !_context.Countries.Any(country => country.Name == newCountry.Name))
            {
                Country country = new Country();
                country.Name = newCountry.Name;
                _context.Countries.Add(country);
                await _context.SaveChangesAsync();
                TempData["informations"] = "Country successfully added!";
            }
            else
            {
                TempData["informations"] = "The operation has failed. One or more parameters were wrong.";
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Action that lead to the form for adding a <see cref="City"/>.
        /// </summary>
        /// <returns>The form for adding <see cref="City"/></returns>
        public IActionResult AddCity()
        {
            if (_context.Countries.Count() != 0)
            {
                ViewBag.Countries = new SelectList(_context.Countries, "Id", "Name");
                return View();
            }
            else
            {
                TempData["informations"] = "You must have added a country first before adding a city.";
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// Action that leads to the addition of a <see cref="City"/> within the <see cref="ApplicationDbContext"/> and the database.
        /// </summary>
        /// <param name="newCity">the new <see cref="City"/> object.</param>
        /// <returns>Return to the <see cref="Index"/> page</returns>
        [HttpPost]
        public async Task<IActionResult> AddCity(City newCity)
        {
            if (!_context.Countries.Any(country => country.Name == newCity.Name) &&
                !_context.Cities.Any(city => city.Name == newCity.Name && city.CountryId == newCity.CountryId))
            {
                newCity.Country = _context.Countries.FirstOrDefault(c => c.Id == newCity.CountryId);
                _context.Cities.Add(newCity);
                await _context.SaveChangesAsync();
                TempData["informations"] = "City successfully added!";
            }
            else
            {
                TempData["informations"] = "The operation has failed. One or more parameters were wrong.";
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Action that lead to the form for editing a <see cref="City"/> or a <see cref="Country"/>.
        /// </summary>
        /// <param name="id">The ID of the <see cref="Country"/> object, ignored if the second parameter isn't null or empty.</param>
        /// <param name="id2">The ID of the <see cref="City"/> object.</param>
        /// <returns>The form for editing a <see cref="City"/> or a <see cref="Country"/>.</returns>
        [HttpPost]
        public IActionResult EditData(string id, string id2)
        {
            if (string.IsNullOrEmpty(id2))
            {
                return RedirectToAction("EditCountryData", "Place", new { id = id });
            }
            else
            {
                return RedirectToAction("EditCityData", "Place", new { id = id2 });
            }
        }

        /// <summary>
        /// Return the View of the form for editing a <see cref="Country"/>.
        /// </summary>
        /// <param name="id">The id of the <see cref="Country"/> we want to modify.</param>
        /// <returns>The View of the form for editing a <see cref="Country"/>.</returns>
        public IActionResult EditCountryData(string id)
        {
            Country country = _context.Countries.FirstOrDefault(c => c.Id.ToString() == id);
            return View("EditCountryData", country);
        }

        /// <summary>
        /// Return the View of the form for editing a <see cref="City"/>.
        /// </summary>
        /// <param name="id">The id of the <see cref="City"/> we want to modify.</param>
        /// <returns>The View of the form for editing a <see cref="City"/>.</returns>
        public IActionResult EditCityData(string id)
        {
            ViewBag.Countries = new SelectList(_context.Countries, "Id", "Name");
            City city = _context.Cities.FirstOrDefault(c => c.Id.ToString() == id);
            return View("EditCityData", city);
        }

        /// <summary>
        /// Action of Update on a <see cref="Country"/> object.
        /// </summary>
        /// <param name="country">The object <see cref="Country"/> with his modified attributes.</param>
        /// <returns>Return to the <see cref="Index"/> action.</returns>
        [HttpPost]
        public async Task<IActionResult> EditCountryData(Country country)
        {
            if(_context.Countries.Any(c => c.Id == country.Id) && !_context.Countries.Any(c => c.Name == country.Name))
            {
                Country oldCountry = _context.Countries.FirstOrDefault(c => c.Id == country.Id);
                oldCountry.Name = country.Name;
                await _context.SaveChangesAsync();
                TempData["informations"] = "The country was successfully updated!";
            }
            else
            {
                TempData["informations"] = "The operation has failed. One or more parameters were wrong.";
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Action of Update on a <see cref="City"/> object.
        /// </summary>
        /// <param name="city">The object <see cref="City"/> with his modified attributes.</param>
        /// <returns>Return to the <see cref="Index"/> action.</returns>
        [HttpPost]
        public async Task<IActionResult> EditCityData(City city)
        {
            if (_context.Cities.Any(c => c.Id == city.Id) &&
                _context.Countries.Any(country => country.Id == city.CountryId && country.Name != city.Name &&
                !country.Cities.Any(c => c.Name == city.Name)))
            {
                City oldCity = _context.Cities.FirstOrDefault(c => c.Id == city.Id);
                oldCity.Name = city.Name;
                oldCity.CountryId = city.CountryId;
                oldCity.Country = _context.Countries.FirstOrDefault(c => c.Id == city.CountryId);
                await _context.SaveChangesAsync();
                TempData["informations"] = "The city was successfully updated!";
            }
            else
            {
                TempData["informations"] = "The operation has failed. One or more parameters were wrong.";
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Action that remove the specified <see cref="Country"/> or <see cref="City"/>.
        /// </summary>
        /// <param name="id">The id of the <see cref="Country"/> that must be removed, ignored if id2 isn't null or empty.</param>
        /// <param name="id2">The id of the <see cref="City"/> that must be removed.</param>
        /// <returns>Return to the <see cref="Index"/> action.</returns>
        [HttpPost]
        public async Task<IActionResult> RemoveData(string id, string id2)
        {
            if (string.IsNullOrEmpty(id2))
            {
                Country country = _context.Countries.FirstOrDefault(c => c.Id.ToString() == id);
                _context.Countries.Remove(country);
                await _context.SaveChangesAsync();
                TempData["informations"] = "The country and all its cities was successfully removed!";
            }
            else
            {
                City city = _context.Cities.FirstOrDefault(c => c.Id.ToString() == id2);
                _context.Cities.Remove(city);
                await _context.SaveChangesAsync();
                TempData["informations"] = "The city was successfully removed!";
            }
            return RedirectToAction("Index");
        }
    }
}
