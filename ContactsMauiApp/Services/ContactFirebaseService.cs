using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsMauiApp.Services
{
	public class ContactFirebaseService : IContactService
	{
		private FirebaseClient client;

		public ContactFirebaseService(IUserService userService)
		{
			UserFirebaseService userFirebaseService = (UserFirebaseService)userService;
			FirebaseClient client = new FirebaseClient("https://maui-contacts-default-rtdb.firebaseio.com/",
			new FirebaseOptions
			{
				AuthTokenAsyncFactory = () => Task.FromResult(userFirebaseService.FirebaseUser.Credential.IdToken)
			});
		}
		public async Task<bool> AddContact(Model.Contact contact)
		{
			try
			{
				var result = await client.Child("Contacts").PostAsync<Model.Contact>(contact);
				return true;
			}
			catch (Exception ex) { }
			return false;
		}

		public bool Delete(Model.Contact contact)
		{
			try
			{
				client.Child("Contacts").Child(contact.FirebaseKey).DeleteAsync();
				return true;
			}
			catch { }
			return false;
		}

		public Task<Model.Contact> GetContact(int id)
		{
			throw new NotImplementedException();
		}

		public async Task<List<Model.Contact>> GetContacts()
		{
			var result = await client.Child("Contacts").OnceAsync<Model.Contact>();
			return result.Select(c=>new Model.Contact() 
			{ 
				FirebaseKey = c.Key,
				Id = c.Object.Id,
				Name = c.Object.Name,
				Address = c.Object.Address,
				dateOfBirth = c.Object.dateOfBirth,
				Email = c.Object.Email,
				Phone = c.Object.Phone,
				ImgPath = c.Object.ImgPath
			}).ToList();
		}

		public bool UpdateContact(Model.Contact contact)
		{
			try
			{
				client.Child("Contacts").Child(contact.FirebaseKey).PutAsync<Model.Contact>(contact);
				return true;
			}
			catch { }
			return false;
		}

		public Task<bool> UploadToyImage(FileResult photo, Model.Contact contact)
		{
			throw new NotImplementedException();
		}
	}
}
