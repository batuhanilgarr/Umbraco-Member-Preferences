using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using Newtonsoft.Json;
using StockMarket.Models;
using StockMarket.Data;

namespace StockMarket.Controllers
{
    [ApiController]
    [Route("umbraco/surface/api/[controller]")]
    public class PreferencesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PreferencesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("savePreferences")]
        public IActionResult SavePreferences([FromBody] PreferencesModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized("User not logged in");

            var preferencesJson = JsonConvert.SerializeObject(model.Checkboxes);
            var userPreferences = _context.UserPreferences.FirstOrDefault(up => up.UserId == userId);

            if (userPreferences != null)
            {
                userPreferences.SelectedCheckboxes = preferencesJson;
                _context.UserPreferences.Update(userPreferences);
            }
            else
            {
                _context.UserPreferences.Add(new UserPreferences
                {
                    UserId = userId,
                    SelectedCheckboxes = preferencesJson
                });
            }

            _context.SaveChanges();
            return Ok();
        }

        [HttpGet("loadPreferences")]
        public IActionResult LoadPreferences()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null) return Unauthorized("User not logged in");

                var userPreferences = _context.UserPreferences.FirstOrDefault(up => up.UserId == userId);
                var checkboxes = userPreferences != null
                    ? JsonConvert.DeserializeObject<List<string>>(userPreferences.SelectedCheckboxes)
                    : new List<string>();

                return Ok(checkboxes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        public class RemovePreferenceModel
        {
            public string Currency { get; set; }
            public decimal Rate { get; set; }
        }

        [HttpDelete("removePreference")]
        public IActionResult RemovePreference([FromBody] RemovePreferenceModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized("User not logged in");

            var userPreferences = _context.UserPreferences.FirstOrDefault(up => up.UserId == userId);
            if (userPreferences == null) return NotFound("User preferences not found");

            var checkboxes = userPreferences.SelectedCheckboxes != null
                ? JsonConvert.DeserializeObject<List<string>>(userPreferences.SelectedCheckboxes)
                : new List<string>();

            var itemToRemove = $"{model.Currency}: {model.Rate.ToString("F4")}";

            checkboxes.Remove(itemToRemove);

            userPreferences.SelectedCheckboxes = JsonConvert.SerializeObject(checkboxes);
            _context.UserPreferences.Update(userPreferences);
            _context.SaveChanges();

            return Ok();
        }
    }
}
