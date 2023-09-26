using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserManagementIdentity.Models;
using UserManagementIdentity.Models.Authentication.Signup;

namespace UserManagementIdentity.Controllers.AuthenticationController
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthenticationController(UserManager<IdentityUser> userManager,
                                        RoleManager<IdentityRole> roleManager,
                                        IConfiguration configuration)
        {
            _roleManager = roleManager;
            _configuration = configuration;
            _userManager = userManager;

        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterUser model, string Role)
        {
            // Check user exist
            var userExist = await _userManager.FindByEmailAsync(model.Email);
            if (userExist != null)
            {
                return StatusCode(StatusCodes.Status403Forbidden,
                                  new Response { Status = "Error", Message = "User is already exist." });
            }

            // Add user in the database
            IdentityUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName
            };
            if (await _roleManager.RoleExistsAsync(Role))
            {
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Role);
                    return StatusCode(StatusCodes.Status201Created,
                                     new Response { Status = "Success", Message = "User created successfully." });
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                                      new Response { Status = "Error", Message = "User created failed." });
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                      new Response { Status = "Error", Message = "This role doesn's exists." });
            }
        }
    }
}
