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
    }
}