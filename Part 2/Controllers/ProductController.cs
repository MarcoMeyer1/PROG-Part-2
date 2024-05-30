using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Part_2.Data;
using Part_2.Models;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Part_2.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductRepository _productRepository;
        private readonly UserRepository _userRepository;

        public ProductController(ProductRepository productRepository, UserRepository userRepository)
        {
            _productRepository = productRepository;
            _userRepository = userRepository;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                var farmer = await _userRepository.GetUserByEmailAsync(userEmail);
                if (farmer != null && farmer.Role == "Farmer")
                {
                    product.FarmerId = farmer.Id; // Set the FarmerId based on the logged-in farmer
                    await _productRepository.AddAsync(product);
                    return RedirectToAction("Index");
                }
            }
            return View(product);
        }

        public async Task<IActionResult> Index()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var farmer = await _userRepository.GetUserByEmailAsync(userEmail);
            if (farmer != null && farmer.Role == "Farmer")
            {
                var products = await _productRepository.GetByFarmerIdAsync(farmer.Id);
                return View(products);
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> AllProducts()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userRepository.GetUserByEmailAsync(userEmail);
            if (user != null && user.Role == "Employee")
            {
                var products = await _productRepository.GetAllWithFarmerAsync(); // Changed to get ProductViewModel
                return View(products);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userRepository.GetUserByEmailAsync(userEmail);
            if (user != null && user.Role == "Employee")
            {
                await _productRepository.DeleteAsync(id);
                return RedirectToAction("AllProducts");
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> FilterProducts(DateTime? startDate, DateTime? endDate, string category)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userRepository.GetUserByEmailAsync(userEmail);
            if (user != null && user.Role == "Employee")
            {
                var products = await _productRepository.GetFilteredProductsAsync(startDate, endDate, category);
                var categories = await _productRepository.GetDistinctCategoriesAsync();
                var farmers = await _userRepository.GetAllFarmersAsync();

                ViewBag.Categories = new SelectList(categories);
                ViewBag.Farmers = farmers.ToDictionary(f => f.Id, f => f.Name);

                return View(products);
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _productRepository.GetByIdWithFarmerAsync(id); // Changed to get ProductViewModel
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                await _productRepository.UpdateAsync(product);
                return RedirectToAction("Index");
            }
            return View(product);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productRepository.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
