using Microsoft.AspNetCore.Mvc;
using Part_2.Data;
using Part_2.Models;
using System.Threading.Tasks;

namespace Part_2.Controllers
{
    public class FarmerController : Controller
    {
        private readonly UserRepository _userRepository;

        public FarmerController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Index()
        {
            var farmers = await _userRepository.GetAllFarmersAsync();
            return View(farmers);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(User farmer)
        {
            if (ModelState.IsValid)
            {
                farmer.Role = "Farmer";
                await _userRepository.AddUserAsync(farmer);
                return RedirectToAction(nameof(Index));
            }

            return View(farmer);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var farmer = await _userRepository.GetUserByIdAsync(id);
            if (farmer == null || farmer.Role != "Farmer")
            {
                return NotFound();
            }
            return View(farmer);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(User farmer)
        {
            if (ModelState.IsValid)
            {
                farmer.Role = "Farmer"; // Ensure role is not changed
                await _userRepository.UpdateUserAsync(farmer);
                return RedirectToAction(nameof(Index));
            }
            return View(farmer);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var farmer = await _userRepository.GetUserByIdAsync(id);
            if (farmer == null || farmer.Role != "Farmer")
            {
                return NotFound();
            }
            return View(farmer);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _userRepository.DeleteUserAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
