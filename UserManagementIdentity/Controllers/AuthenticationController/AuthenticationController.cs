using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NETCore.MailKit.Core;
using UserManagementIdentity.Models;
using UserManagementIdentity.Models.Authentication.Signup;
using UserManagementIdentity.Service.Models;
using IEmailServices = UserManagementIdentity.Service.Services.IEmailServices;

namespace UserManagementIdentity.Controllers.AuthenticationController
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailServices _emailService;

        public AuthenticationController(UserManager<IdentityUser> userManager,
                                        RoleManager<IdentityRole> roleManager,
                                        IConfiguration configuration,
                                        IEmailServices emailService)
        {
            _roleManager = roleManager;
            _configuration = configuration;
            _userManager = userManager;
            _emailService = emailService;

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

        [HttpGet]
        public IActionResult TestEmail()
        {
            var message = new Message(new string[]
                {"mdhasibulhasan.dev@gmail.com"}, "Test", "<h1>Hello this is Hasibul Hasibul</h1>");

            _emailService.SendEmail(message);
            return StatusCode(StatusCodes.Status201Created,
                                     new Response { Status = "Success", Message = "Email sent successfully." });
        }
    }
}
