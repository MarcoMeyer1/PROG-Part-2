using Microsoft.AspNetCore.Mvc;
using Part_2.Data;
using System.Threading.Tasks;

namespace Part_2.Controllers
{
    public class MarketplaceController : Controller
    {
        private readonly ProductRepository _productRepository;
        private readonly UserRepository _userRepository;

        public MarketplaceController(ProductRepository productRepository, UserRepository userRepository)
        {
            _productRepository = productRepository;
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllWithFarmerAsync();
            return View(products); // Ensure this returns IEnumerable<ProductViewModel>
        }

        public async Task<IActionResult> ProductDetails(int id)
        {
            var product = await _productRepository.GetByIdWithFarmerAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.Farmer = product.FarmerName;
            return View(product); // Ensure this returns ProductViewModel
        }

        public async Task<IActionResult> FarmerDetails(int id)
        {
            var farmer = await _userRepository.GetUserByIdAsync(id);
            if (farmer == null || farmer.Role != "Farmer")
            {
                return NotFound();
            }
            return View(farmer);
        }
    }
}
