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

                    // Add token to verify email
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = Url.Action(nameof(ConfirmEmail), "Authentication", 
                        new {token, email = user.Email}, Request.Scheme);
                    var message = new Message(new string[] { user.Email }, "Confirmation email link", confirmationLink);
                    _emailService.SendEmail(message);

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

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByIdAsync(email);

            if(user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status200OK, 
                        new Response { Status = "Success", Message = "Email sent successfuly."});
                }
            }
            return StatusCode(StatusCodes.Status500InternalServerError,
                        new Response { Status = "Error", Message = "Internal Server Error." });
        }
    }
}
