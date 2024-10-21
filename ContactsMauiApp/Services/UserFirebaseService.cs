using ContactsMauiApp.Model;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Auth.Repository;
using Firebase.Database;
//using Org.Apache.Http.Authentication;
//using Org.Apache.Http.Conn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsMauiApp.Services
{
	internal class UserFirebaseService : IUserService
	{
		private FirebaseAuthClient authClient;
		private Model.User currentUser;

		

		public UserFirebaseService()
		{
			var config = new FirebaseAuthConfig()
			{
				ApiKey = "AIzaSyCYLKFRF5ypOQY_ZUJzhHHlU4mU84w7DOg",
				AuthDomain = "maui-contacts.firebaseapp.com",
				Providers = new Firebase.Auth.Providers.FirebaseAuthProvider[] { new EmailProvider() },
				UserRepository = new FileUserRepository("firebaseUser")//persist data into %AppData%\appuser
			};
			authClient = new FirebaseAuthClient(config);
			FirebaseClient client = new FirebaseClient("https://maui-contacts-default-rtdb.firebaseio.com/",
			new FirebaseOptions
			{
				AuthTokenAsyncFactory = () => Task.FromResult(authClient.User.Credential.IdToken)
			});
		}
		public Model.User CurrentUser => currentUser;
		public Firebase.Auth.User FirebaseUser => authClient.User;

		public async Task<Model.User> Create(string email, string password, string displayName)
		{
			var fUser = await authClient.CreateUserWithEmailAndPasswordAsync(email, password, displayName);
			currentUser = new Model.User()
			{
				Email = fUser.User.Info.Email,
				DisplayName = fUser.User.Info.DisplayName,
			};
			return currentUser;
		}

		public async Task<Model.User> Login(string email, string password)
		{
			var fUser = await authClient.SignInWithEmailAndPasswordAsync(email, password);
			currentUser = new Model.User()
			{
				Email = fUser.User.Info.Email,
				DisplayName = fUser.User.Info.DisplayName,
			};
			return currentUser;
		}

		public void Logout()
		{
			authClient.SignOut();
		}
	}
}
