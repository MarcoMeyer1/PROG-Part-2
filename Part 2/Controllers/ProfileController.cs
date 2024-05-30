using Microsoft.AspNetCore.Mvc;
using Part_2.Data;
using Part_2.Models;
using System.Threading.Tasks;

namespace Part_2.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserRepository _userRepository;
        private readonly ProductRepository _productRepository;

        public ProfileController(UserRepository userRepository, ProductRepository productRepository)
        {
            _userRepository = userRepository;
            _productRepository = productRepository;
        }

        public async Task<IActionResult> Details(int id)
        {
            var farmer = await _userRepository.GetUserByIdAsync(id);
            if (farmer == null || farmer.Role != "Farmer")
            {
                return NotFound();
            }

            var products = await _productRepository.GetByFarmerIdAsync(farmer.Id);
            var userProfile = await _userRepository.GetUserProfileByUserIdAsync(farmer.Id); // Add this line to get UserProfile

            var viewModel = new UserProfileViewModel
            {
                Farmer = farmer,
                Products = products,
                Profile = userProfile  // Add this line to set UserProfile
            };

            return View(viewModel);
        }
    }
}
