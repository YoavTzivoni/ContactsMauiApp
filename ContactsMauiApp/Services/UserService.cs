using ContactsMauiApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsMauiApp.Services
{
	public class UserService : IUserService
	{
		private User currentUser;
		private List<User> allUsers = new List<User>();
		public UserService()
		{
			loadData();
			Load();
		}

		public User CurrentUser { get { return currentUser; }  }
		public Task<User> Login(string email, string password)
		{
			User user = FindUser(email);
			
			if (user != null && user.Password == password) 
			{
				currentUser = user;
				Save();
				return new Task<User>(()=> user);
			}
			return null;
		}

		public Task<User> Create(string email, string password, string displayName)
		{
			if (FindUser(email) != null) return null;

			currentUser = new User { DisplayName = displayName, Email = email, Password = password };
			Save();
			return new Task<User>(()=> currentUser);
		}

		public void Logout()
		{
			clear();
			currentUser = null;
		}

		private User FindUser(string username)
		{
			return allUsers.Find((u) => u.Email == username);
		}
		private void Load()
		{
			if (Preferences.ContainsKey(nameof(currentUser.DisplayName)))
			{
				currentUser = new User();
				currentUser.DisplayName = Preferences.Get(nameof(currentUser.DisplayName), string.Empty);
				currentUser.Email = Preferences.Get(nameof(currentUser.Email), string.Empty);
				currentUser.Password = Preferences.Get(nameof(currentUser.Password), string.Empty);
			}
		}
		private void Save()
		{
			if (currentUser == null) return;

			Preferences.Set(nameof(currentUser.DisplayName), currentUser.DisplayName);
			Preferences.Set(nameof(currentUser.Email), currentUser.Email);
			Preferences.Set(nameof(currentUser.Password), currentUser.Password);
		}

		private void clear()
		{
			Preferences.Default.Remove(nameof(currentUser.DisplayName));
			Preferences.Default.Remove(nameof(currentUser.Email));
			Preferences.Default.Remove(nameof(currentUser.Password));
		}

		private void loadData()
		{
			allUsers.Add(new User() { DisplayName = "Yoav Tzivoni", Email = "yoav@gmail.com", Id = 1, Password = "123456" });
			allUsers.Add(new User() { DisplayName = "Leshem", Email = "leshem@gmail.com", Id = 2, Password = "123456" });
		}
		
	}
}
