using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactsMauiApp.Model;

namespace ContactsMauiApp.Services
{
	public class ContactService : IContactService
	{
		private List<Model.Contact> contacts = new();
		public async Task<List<Model.Contact>> GetContacts() { return contacts.ToList(); }

		public ContactService()
		{
			contacts.Add(new Model.Contact()
			{
				Id = 1,
				Name = "יואב צבעוני",
				Address = "חנקין 9 תל אביב",
				dateOfBirth = new DateTime(1974, 7, 20),
				Email = "yoav@gmail.com",
				Phone = "052-8998874"
			});

			contacts.Add(new Model.Contact()
			{
				Id = 2,
				Name = "נטע",
				Address = "חנקין 9 תל אביב",
				dateOfBirth = new DateTime(2020, 8, 10),
				Email = "neta@gmail.com",
				Phone = "052-6778812"
			});
		}

		public async Task<Model.Contact> GetContact(int id)
		{
			return contacts.Find(c=>c.Id == id);
		}
		public async Task<bool> AddContact(Model.Contact contact)
		{
			contacts.Add(contact);
			return true;
		}

		public bool UpdateContact(Model.Contact contact)
		{
			Model.Contact c = contacts.Find(Contact => Contact.Id == contact.Id);
			if (c != null) return false;
			c.Name = contact.Name;
			c.Address = contact.Address;
			c.dateOfBirth = contact.dateOfBirth;
			c.Email = contact.Email;
			c.Phone = contact.Phone;
			return true;
		}
		public bool Delete(Model.Contact contact)
		{
			return contacts.Remove(contact);
		}
	}
}
