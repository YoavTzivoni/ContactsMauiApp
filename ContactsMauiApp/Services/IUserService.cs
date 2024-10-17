using ContactsMauiApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsMauiApp.Services
{
    public interface IUserService
    {
		public User CurrentUser { get; }
		public Task<User> Login(string email, string password);
		public Task<User> Create(string email, string password, string displayName);
		public void Logout();			
	}
}
