using ContactsWebAPI.Model;
using ContactsWebAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContactsWebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private ContactRepository contactRepository;
		private UserRepository _userRepository;
		public UsersController(UserRepository userRepository, ContactRepository contactRepository)
		{
			_userRepository = userRepository;
			this.contactRepository = contactRepository;
		}


		[HttpPost("Register")]
		public IActionResult Register([FromBody] User user)
		{
			User newUser = new User() { Email = user.Email, DisplayName = user.DisplayName, Password = user.Password };
			if (!_userRepository.AddUser(newUser))
				return Conflict("Unable to Add user");
			return Ok(newUser);
		}


		// POST api/login
		[HttpPost("login")]
		public IActionResult Login([FromBody] LoginInfo? loginDto)
		{
			try
			{
				//HttpContext.Session?.Clear(); //Logout any previous login attempt

				//Get model user class from DB with matching email. 

				var user = _userRepository.Login(loginDto.Email, loginDto.Password);

				//Check if user exist for this email and if password match, if not return Access Denied (Error 403) 
				if (user == null || user.Password != loginDto.Password)
				{
					return Unauthorized("Login Failed UserName or Password Incorrect");
				}

				//Login suceed! now mark login in session memory!
				//HttpContext.Session?.SetString("loggedInUser", user.Email);

				return Ok(user);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}

		}

		
	}
}
