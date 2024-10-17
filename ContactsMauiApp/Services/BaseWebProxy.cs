using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ContactsMauiApp.Services
{
	public class BaseWebProxy
	{
		protected const string BASE_URL = "https://localhost:7028/";
		protected HttpClient client;//יטפל בבקשות ובתשובות מהשרת
		protected JsonSerializerOptions options;//להגדיר את האופן שבו יתבצעו פעולות הסיריליאזציה והדה סירי

		public BaseWebProxy()
		{
			//Work With Cookies
			HttpClientHandler handle = new HttpClientHandler() { CookieContainer = new System.Net.CookieContainer(), UseCookies = true };
			//Create Client
			client = new HttpClient(handle, true);
			//הגדרות הסיריליאזציה
			options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true, WriteIndented = true };
		}
	}
}
