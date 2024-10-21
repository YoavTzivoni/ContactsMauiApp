using ContactsWebAPI.Data;
using ContactsWebAPI.Model;

namespace ContactsWebAPI.Repositories
{
	public class ContactRepository
	{
		private static int contact_id = 0;
		private List<Contact> contacts;
		private DataContext context;
		public ContactRepository(DataContext context)
		{
			this.context = context;
			InitContacts();
		}

		private void InitContacts()
		{
			contacts = new List<Contact>();
			contacts.Add(new Contact()
			{
				Id = ++contact_id,
				Name = "yoav tzivoni",
				Phone = "052665612",
				Email = "yoav@gmail.com",
				Address = "tel aviv",
				dateOfBirth = new DateTime(1974, 7, 20),
			});
		}

		public List<Contact> GetContacts() { return context.Contacts.ToList(); /*contacts;*/ }
		public Contact GetContact(int id) { return context.Contacts.Find(id); /*contacts.Find((c) => c.Id == id);*/ }
		public Contact AddContact(Contact contact)
		{
			context.Contacts.Add(contact);
			context.SaveChanges();
			return contact;

			//if (contacts.Find((c) => c.Id == contact_id) != null) return null;
			contact.Id = ++contact_id;
			contacts.Add(contact);
			return contact;
		}

		public Contact UpdateContact(int id, Contact contact)
		{
			Contact c = GetContact(id);// contacts.Find((c) => c.Id == contact_id);
			if (c == null) return null;
			c.Name = contact.Name;
			c.Phone = contact.Phone;
			c.Email = contact.Email;
			c.Address = contact.Address;
			c.dateOfBirth = contact.dateOfBirth;
			context.SaveChanges();
			return contact;

		}

		public bool DeleteContact(int id)
		{
			Contact contact = GetContact(id);
			if (contact != null)
			{
				context.Contacts.Remove(contact);
				context.SaveChanges();
				return true;
			}
			return false;
		}

		public bool UpdateImage(int contactId, string imageName)
		{
			var contact = GetContact(contactId); // contacts.Find(x => x.Id == contactId);
			if (contact != null)
			{
				contact.ImgPath = imageName;
				context.SaveChanges();
				return true;
			}
			return false;
		}

	}
}
