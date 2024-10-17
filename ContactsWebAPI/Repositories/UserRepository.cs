using ContactsWebAPI.Model;

namespace ContactsWebAPI.Repositories
{

	public class UserRepository
	{
		static int user_id = 0;
		List<User> users;

		public UserRepository()
		{
			InitUsers();
		}
		private void InitUsers()
		{
			users = new List<User>();
			users.Add(
				new()
				{
					Id = ++user_id,
					Email = "yoav@gmail.com",
					DisplayName = "yoav tzivoni",
					Password = "1234"
				}
				);
		}
		public User Login(string? userName, string? password)
		{
			return users.Where(u => u.Email == userName && u.Password == password).FirstOrDefault();
		}

		public bool AddUser(User user)
		{
			if (users.Any(u => u.Email == user.Email))
				return false;
			user.Id = ++user_id;
			users.Add(user);
			return true;
		}

	}
}
