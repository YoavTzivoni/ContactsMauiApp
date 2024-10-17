using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ContactsMauiApp.Services
{
	public class ContactWebProxy : IContactService
	{
		private HttpClient client;//יטפל בבקשות ובתשובות מהשרת
		private JsonSerializerOptions options;//להגדיר את האופן שבו יתבצעו פעולות הסיריליאזציה והדה סירי

		private const string URL = "https://8vp3j0n7-7028.euw.devtunnels.ms/api/ContactsApi/";//כתובת השרת באמצעות devtunnels
		//private string URL = Environment.GetEnvironmentVariable("VS_TUNNEL_URL");
		public ContactWebProxy()
		{
			HttpClientHandler handle = new HttpClientHandler() { CookieContainer = new System.Net.CookieContainer(), UseCookies = true };
			client = new HttpClient(handle, true);
			options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true, WriteIndented = true };
		}
		public async Task<List<Model.Contact>> GetContacts()
		{
			string json = string.Empty;//הג'ייסון שיקלט
			List<Model.Contact>? contacts = new();//
			try
			{
				string getUrl = @$"{URL}Contacts";
				var response = await client.GetAsync(getUrl);
				if (response.StatusCode == HttpStatusCode.OK)
				{
					json = await response.Content.ReadAsStringAsync();
					contacts = JsonSerializer.Deserialize<List<Model.Contact>>(json, options);
					return contacts;
				}
			}
			catch (Exception ex)
			{
				Console.Write(ex.ToString());
			}
			return contacts;
		}

		public async Task<Model.Contact> GetContact(int id)
		{
			string json = string.Empty;//הג'ייסון שיקלט
			Model.Contact? contact = null;//
			try
			{
				string getUrl = @$"{URL}Contacts/{id}";
				var response = await client.GetAsync(getUrl);
				if (response.StatusCode == HttpStatusCode.OK)
				{
					json = await response.Content.ReadAsStringAsync();
					contact = JsonSerializer.Deserialize<Model.Contact>(json, options);
					return contact;
				}
			}
			catch (Exception ex)
			{
				Console.Write(ex.ToString());
			}
			return null;
		}
		public async Task<bool> AddContact(Model.Contact contact)
		{
			try
			{
				string json = JsonSerializer.Serialize(contact, options);
				StringContent content = new StringContent(json, encoding: Encoding.UTF8, mediaType: "application/json");
				var response = await client.PostAsync($"{URL}Contacts", content);
				if (response.StatusCode == HttpStatusCode.OK)
					return true;
			}
			catch (Exception ex) { Console.WriteLine(ex.ToString()); }
			return false;
		}

		public bool Delete(Model.Contact contact)
		{
			throw new NotImplementedException();
		}

		public bool UpdateContact(Model.Contact contact)
		{
			throw new NotImplementedException();
		}


	}
}
