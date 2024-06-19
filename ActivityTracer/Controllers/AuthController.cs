using ActivityTracer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace ActivityTracer.Controllers
{

	[ApiController]
	[Route("[controller]")]
	public class AuthController : ControllerBase
	{
		private readonly UserManager<SiteUser> _userManager;

		public AuthController(UserManager<SiteUser> userManager)
		{
			_userManager = userManager;
		}

		[HttpPost]
		public async Task<IActionResult> Login([FromBody] LoginViewModel model)
		{
			
			var user = await _userManager.FindByNameAsync(model.Email);
			if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
			{
				
				var claim = new List<Claim>
				{
					new Claim(JwtRegisteredClaimNames.Email, user.Email),
					new Claim("FirstName", user.FirstName), 
                    new Claim("LastName", user.LastName)
				};
				
				var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("nagyonhosszutitkoskodhelye"));
				;
				var token = new JwtSecurityToken(
				 issuer: "http://www.security.org", audience: "http://www.security.org",
				 claims: claim, expires: DateTime.Now.AddMinutes(60),
				 signingCredentials: new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256)
				);
				;
				var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
				;
				
				return Ok(new
				{
					token = new JwtSecurityTokenHandler().WriteToken(token),
					expiration = token.ValidTo
				});
			}
			return Unauthorized();
		}
	}
}
