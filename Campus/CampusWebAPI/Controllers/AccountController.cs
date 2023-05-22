using Campus.Core.DTO;
using Campus.Core.Enums;
using Campus.Core.Identity;
using Campus.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CampusWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly IAcademicService _academicService;
        private readonly IStudentService _studentService;
        private readonly IJwtService _jwtService;

        public AccountController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, 
            SignInManager<ApplicationUser> signInManager, IAcademicService academicService, IStudentService studentService, IJwtService jwtService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _academicService = academicService;
            _studentService = studentService;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            await FillRolesTable();

            if (!ModelState.IsValid)
            {
                IEnumerable<string> errors = ModelState.Values.SelectMany(value => value.Errors)
                    .Select(error => error.ErrorMessage);

                return BadRequest(string.Join(", ", errors));
            }

            ApplicationUser user = new ApplicationUser()
            {
                UserName = registerDTO.Name,
                Email = registerDTO.Email
            };

            var result = await _userManager.CreateAsync(user, registerDTO.Password);

            if(result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, UserRole.Admin.ToString());

                await _signInManager.SignInAsync(user, isPersistent: false);

                var response = _jwtService.GetJwt(user);

                return Ok(response);
            }
            else
            {
                return BadRequest(string.Join(", ", result.Errors.Select(error => error.Description)));    
            }
        }

        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpPost("register/student")]
        public async Task<IActionResult> RegisterStudent(StudentAddRequest studentAddRequest)
        {
            await FillRolesTable();

            StudentResponse studentResponse = await _studentService.Create(studentAddRequest);

            ApplicationUser user = new ApplicationUser()
            {
                UserName = studentAddRequest.Email,
                Email = studentAddRequest.Email,
                UserId = studentResponse.Id
            };

            var result = await _userManager.CreateAsync(user, studentAddRequest.Password);

            if(result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, UserRole.Student.ToString());

                return Ok();
            }
            else
            {
                return BadRequest(string.Join(", ", result.Errors.Select(error => error.Description)));
            }
        }

        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpPost("register/academic")]
        public async Task<IActionResult> RegisterAcademic(AcademicAddRequest academicAddRequest)
        {
            await FillRolesTable();

            AcademicResponse academicResponse = await _academicService.Add(academicAddRequest);

            ApplicationUser user = new ApplicationUser()
            {
                UserName = academicAddRequest.Email,
                Email = academicAddRequest.Email,
                UserId = academicResponse.Id
            };

            var result = await _userManager.CreateAsync(user, academicAddRequest.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, UserRole.Academic.ToString());

                return Ok();
            }
            else
            {
                return BadRequest(string.Join(", ", result.Errors.Select(error => error.Description)));
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            ApplicationUser? user = await _userManager.FindByEmailAsync(loginDTO.Email);

            if(user == null)
            {
                return NotFound("Email not found");
            }

            var result = await _signInManager.PasswordSignInAsync(user, loginDTO.Password, 
                isPersistent: false, lockoutOnFailure: false);

            if(result.Succeeded)
            {
                var response = _jwtService.GetJwt(user);

                return Ok(response);
            }
            else
            {
                return BadRequest("Password does not match");
            }
        }

        [Authorize]
        [HttpGet("sign-out")]
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();

            return Ok("Successfully signed out");
        }

        private async Task FillRolesTable()
        {
            if(await _roleManager.FindByNameAsync(UserRole.Admin.ToString()) is null)
            {
                ApplicationRole role = new ApplicationRole() { Name = UserRole.Admin.ToString() };
                await _roleManager.CreateAsync(role);
            }

            if (await _roleManager.FindByNameAsync(UserRole.Academic.ToString()) is null)
            {
                ApplicationRole role = new ApplicationRole() { Name = UserRole.Academic.ToString() };
                await _roleManager.CreateAsync(role);
            }

            if (await _roleManager.FindByNameAsync(UserRole.Student.ToString()) is null)
            {
                ApplicationRole role = new ApplicationRole() { Name = UserRole.Student.ToString() };
                await _roleManager.CreateAsync(role);
            }
        }
    }
}
