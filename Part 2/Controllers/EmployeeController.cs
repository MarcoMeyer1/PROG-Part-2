using Microsoft.AspNetCore.Mvc;
using Part_2.Data;
using System.Threading.Tasks;

namespace Part_2.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly UserRepository _userRepository;

        public EmployeeController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Index()
        {
            var employees = await _userRepository.GetAllEmployeesAsync();
            return View(employees);
        }
    }
}
