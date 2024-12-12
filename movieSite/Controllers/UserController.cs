using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using movies_BLL.DTOs;
using movies_BLL.models;

namespace movies_WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }


        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _userManager.Users.ToList();
            return Ok(users);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }



        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(UserCreateModel model)
        {
            if (!ModelState.IsValid || model.Password is null || model.Email is null || model.UserName is null || (model.Password != model.ConfirmPassword))
            {
                return BadRequest(ModelState);
            }

            // Check if user with the same email or username already exists in the database
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "A user with this email already exists");
                return BadRequest(ModelState);
            }

            existingUser = await _userManager.FindByNameAsync(model.UserName);
            if (existingUser != null)
            {
                ModelState.AddModelError("UserName", "A user with this username already exists");
                return BadRequest(ModelState);
            }

            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                Email = model.Email

            };

            // Create user in database
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!_roleManager.RoleExistsAsync("User").Result)
            {
                _roleManager.CreateAsync(new IdentityRole("User")).Wait();
            }


            if (result.Succeeded)
            {
                _userManager.AddToRoleAsync(user, "User").Wait();
                return Ok(user);
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return BadRequest(ModelState);
            }
        }


        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<User>> UpdateUser(string id, UserUpdateModel model)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            user.UserName = model.UserName;
            user.Email = model.Email;

            if (!string.IsNullOrEmpty(model.Password))
            {
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.Password);
            }

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Ok(user);
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return BadRequest(ModelState);
            }
        }



    }

}
