using BLL.DTOs;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDTO registerRequest)
        {
            if(ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser
                {
                    FirstName = registerRequest.FirstName,
                    LastName = registerRequest.LastName,
                    UserName = registerRequest.UserName,
                    Email = registerRequest.Email,
                    PasswordHash = registerRequest.Password
                };
                var result = await _userManager.CreateAsync(user, registerRequest.Password);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return BadRequest(ModelState);
                }
                return Ok("User registered successfully.");
            }
            return BadRequest(ModelState);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO loginRequest)
        {
           ApplicationUser result= await _userManager.FindByNameAsync(loginRequest.UserName);
            if (result != null)
            {
                var found = await _userManager.CheckPasswordAsync(result, loginRequest.Password);

                if (found)
                {
                    //Design the token
                    //get user claims and put it into the token
                    List<Claim> UserClaims = new List<Claim>();
                    UserClaims.Add(new Claim(ClaimTypes.Name, result.UserName));
                    UserClaims.Add(new Claim(ClaimTypes.NameIdentifier, result.Id));
                    UserClaims.Add(new Claim(ClaimTypes.Email, result.Email));
                    //generate jti (unique id for each token)
                    UserClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                    var roles= await _userManager.GetRolesAsync(result);
                    foreach(var role in roles)
                    {
                        UserClaims.Add(new Claim(ClaimTypes.Role, role));
                    }
                    var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisIsMySuper@123456789SecretKey"));
                    SigningCredentials signing = new SigningCredentials(signingKey,SecurityAlgorithms.HmacSha256);
                    JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                        //payload
                        issuer: "https://localhost:44314/",
                        audience: "https://localhost:4200/",
                        expires:DateTime.UtcNow.AddDays(1),
                        claims: UserClaims,
                        //signing credentials
                        signingCredentials: signing

                        );
                    //generate token string
                    return Ok(new
                    {
                     token=new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                        expiration=jwtSecurityToken.ValidTo
                    });

                }

            }
            ModelState.AddModelError("UserName", "Invalid login attempt.");
            return BadRequest(ModelState);
        }
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok("Logged out successfully.");
        }

    }
}
