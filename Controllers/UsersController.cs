using FinanceApp.Data.Service;
using FinanceApp.Models;
using FinanceApp.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Controllers
{
    public class UsersController : BaseController
    {
        public readonly IUserService _userService;
        private readonly IExpensesService _expensesService;
        
        public UsersController(IUserService userService, IExpensesService expensesService)
        {
            _userService = userService;
            _expensesService = expensesService;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllUsers();
            return View(users);
        }

        public async Task<IActionResult> Details(int id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        public IActionResult Create()
        {
            return View(new UserRegistrationDTO());
        }

        public IActionResult Login()
        {
            return View(new UserLoginDTO());
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginDTO loginDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var loginResponse = await _userService.LoginUser(loginDto);
                    
                    HttpContext.Session.SetString("JWTToken", loginResponse.Token);
                    HttpContext.Session.SetString("Username", loginResponse.Username);
                    HttpContext.Session.SetString("UserId", loginResponse.UserId.ToString());
                    HttpContext.Session.SetString("UserEmail", loginResponse.Email);
                    HttpContext.Session.SetString("UserRole", loginResponse.Role);
                    
                    TempData["SuccessMessage"] = "Login successful!";
                    return RedirectToAction("Index", "Home");
                }
                catch (UnauthorizedAccessException)
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                    return View(loginDto);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Login failed: {ex.Message}");
                    return View(loginDto);
                }
            }
            return View(loginDto);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["SuccessMessage"] = "You have been logged out successfully.";
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserRegistrationDTO registrationDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = new User
                    {
                        Username = registrationDto.Username,
                        Email = registrationDto.Email,
                        Role = registrationDto.Role ?? "User"
                    };

                    await _userService.RegisterUser(user, registrationDto.Password);
                
                    var createdUser = await _userService.GetAllUsers();
                    var newUser = createdUser.FirstOrDefault(u => u.Username == user.Username);
                    
                    if (newUser != null)
                    {
                        return View("Index", new List<User> { newUser });
                    }
                    else
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Registration failed: {ex.Message}");
                    return View(registrationDto);
                }
            }
            return View(registrationDto);
        }

        public async Task<IActionResult> Profile()
        {
            var currentUserId = GetCurrentUserId();
            
            if (currentUserId == null)
            {
                TempData["ErrorMessage"] = "Please log in to view your profile.";
                return RedirectToAction("Login");
            }

            var user = await _userService.GetUserById(currentUserId.Value);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction("Login");
            }

            var expenses = await _expensesService.GetByUserId(currentUserId.Value);
            ViewBag.UserExpenses = expenses.ToList();
            ViewBag.ExpenseCount = expenses.Count();
            ViewBag.TotalAmount = expenses.Sum(e => e.Amount);
            ViewBag.AverageAmount = expenses.Any() ? expenses.Average(e => e.Amount) : 0;
            
            return View(user);
        }
        
        public async Task<IActionResult> Edit(User user)
        {
            if (ModelState.IsValid)
            {
                await _userService.UpdateUser(user);
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }
        
        public async Task<IActionResult> Delete(int id)
        {
            await _userService.DeleteUser(id);
            return RedirectToAction(nameof(Index));
        }
    }
}