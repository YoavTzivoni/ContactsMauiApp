using ContactsWebAPI.Model;
using ContactsWebAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ContactsWebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ContactsApiController : ControllerBase
	{
		private ContactRepository contactRepository;
		public ContactsApiController(ContactRepository contactRepository)
		{
			this.contactRepository = contactRepository;			
		}
		// GET: api/<ContactsApiController>
		[HttpGet("Contacts")]
		public IActionResult GetContacts()
		{
			//var user = HttpContext.Session.Get("loggedInUser");
			//if (user == null)
			//	return Unauthorized("You must first LOGIN to the Application");
			return Ok(contactRepository.GetContacts().ToList());
		}

		// GET api/<ContactsApiController>/5
		[HttpGet(@"Contacts/{id}")]
		public IActionResult Get(int id)
		{
			//var user = HttpContext.Session.Get("loggedInUser");
			//if (user == null)
			//	return Unauthorized("You must first LOGIN to the Application");
			var result = contactRepository.GetContact(id);
			if (result == null) return NoContent();
			return Ok(result);
		}

		// POST api/<ContactsApiController>
		[HttpPost("Contacts")]
		public IActionResult Add([FromBody] Contact value)
		{
			 var contact = contactRepository.AddContact(value);
			return Ok(contact);
		}

		// PUT api/<ContactsApiController>/5
		[HttpPut(@"Contacts/{id}")]
		public IActionResult Put(int id, [FromBody] Contact value)
		{
			Contact contact = contactRepository.UpdateContact(id, value);
			if (contact == null) return NoContent();
			return Ok(contact);
		}

		// DELETE api/<ContactsApiController>/5
		[HttpDelete(@"Contacts/{id}")]
		public IActionResult Delete(int id)
		{
			if (!contactRepository.DeleteContact(id))
				return NoContent();
			return Ok();
		}
	}
}
