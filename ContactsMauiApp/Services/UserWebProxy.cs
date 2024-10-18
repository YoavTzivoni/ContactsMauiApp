using ContactsMauiApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ContactsMauiApp.Services
{
	public class UserWebProxy : IUserService
	{
		private User currentUser = null;
		HttpClient client;//יטפל בבקשות ובתשובות מהשרת
		JsonSerializerOptions options;//להגדיר את האופן שבו יתבצעו פעולות הסיריליאזציה והדה סירי

		private const string URL = "https://m9knbz40-7028.euw.devtunnels.ms/api/Users/";//כתובת השרת באמצעות devtunnels		
		//private string URL = Environment.GetEnvironmentVariable("VS_TUNNEL_URL");
		const string IMAGE_URL = "https://mhmdqzkj-7046.euw.devtunnels.ms/Images/";//כתובת שבו נמצאות התמונות בשרת

		public UserWebProxy()
		{
			//Work With Cookies
			HttpClientHandler handle = new HttpClientHandler() { CookieContainer = new System.Net.CookieContainer(), UseCookies = true };
			//Create Client
			client = new HttpClient(handle, true);
			//הגדרות הסיריליאזציה
			options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true, WriteIndented = true };
		}

		public User CurrentUser => currentUser;

		public async Task<User> Create(string email, string password, string displayName)
		{
			string json = JsonSerializer.Serialize(new
			{
				displayName = displayName,
				Email = email,
				Password = password
			}, options);

			StringContent content = new StringContent(content: json, encoding: Encoding.UTF8, mediaType: "application/json");
			try
			{
				string loginUrl = $"{URL}Register";
				//loginUrl = @"https://dtrhlflz-7028.euw.devtunnels.ms/api/Users/login";
				//שלח את הבקשה לשרת
				var response = await client.PostAsync(loginUrl, content);
				if (response.StatusCode == System.Net.HttpStatusCode.OK)
				{
					//read the json from the result content
					json = await response.Content.ReadAsStringAsync();
					if (json != null)
					{
						//deserialize
						User? u = JsonSerializer.Deserialize<User>(json, options);
						currentUser = u;
						return u;
					}

				}
			}
			catch (Exception ex) { }
			return null;
		}

		public async Task<User> Login(string email, string password)
		{
			//create Json using anonymous type
			string json = JsonSerializer.Serialize(new
			{
				Email = email,
				Password = password
			}, options);

			//make json available for the httpclient
			//StringContent is Http Content Wrapper
			//list of possible media Types: https://www.sitepoint.com/mime-types-complete-list/
			StringContent content = new StringContent(content: json, encoding: Encoding.UTF8, mediaType: "application/json");
			try
			{
				string loginUrl = $"{URL}login";
				//loginUrl = @"https://dtrhlflz-7028.euw.devtunnels.ms/api/Users/login";
				//שלח את הבקשה לשרת
				var response = await client.PostAsync(loginUrl, content);

				if (response.StatusCode == System.Net.HttpStatusCode.OK)
				{
					//read the json from the result content
					json = await response.Content.ReadAsStringAsync();
					if (json != null)
					{
						//deserialize
						User? u = JsonSerializer.Deserialize<User>(json, options);
						currentUser = u;
						return u;
					}
				}
			}
			catch (Exception ex) { Console.WriteLine(ex.Message); }
			return null;

		}

		public void Logout()
		{
			throw new NotImplementedException();
		}		
	}
}
