using ContactsMauiApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ContactsMauiApp.ViewModels
{
	[QueryProperty("Id", "id")]
	public class ContactDetailsViewModel : ViewModelBase
	{
		private IContactService contactService;
		public Model.Contact Contact { get; set; }
		private bool editMode = false;
		public bool EditMode
		{
			get => editMode;
			set { if (editMode != value) { editMode = value; } OnPropertyChanged(); }
		}
		public ICommand BackCommand { get; private set; }
		public ContactDetailsViewModel(IContactService contactService)
		{
			this.contactService = contactService;
			BackCommand = new Command(() => { updateContactFromProperties(); Shell.Current.GoToAsync(".."); });
		}
		private int id = -1;
		public int Id
		{
			get { return id; }
			set
			{
				if (id != value)
				{
					id = value;
					Contact = contactService.GetContact(id).Result;
					updateProperties();
				}
			}
		}

		private string name;

		public string Name
		{
			get { return name; }
			set { if (value != name) { name = value; OnPropertyChanged(); } }
		}

		private string phone;

		public string Phone
		{
			get { return phone; }
			set { if (phone != value) { phone = value; OnPropertyChanged(); } }
		}

		private string email;

		public string Email
		{
			get { return email; }
			set { if (email != value) { email = value; OnPropertyChanged(); } }
		}

		private string address;

		public string Address
		{
			get { return address; }
			set { if (address != value) { address = value; OnPropertyChanged(); } }
		}

		private DateTime dateOfBirth;

		public DateTime DateOfBirth
		{
			get { return dateOfBirth; }
			set { if (dateOfBirth != value) { dateOfBirth = value; OnPropertyChanged(); } }
		}

		private void updateProperties()
		{
			Name = Contact.Name;
			Phone = Contact.Phone;
			Email = Contact.Email;
			Address = Contact.Address;
			DateOfBirth = Contact.dateOfBirth;
		}

		private void updateContactFromProperties()
		{
			if (editMode)
			{
				Contact.Name = Name;
				Contact.Phone = Phone;
				Contact.Email = Email;
				Contact.Address = Address;
				Contact.dateOfBirth = DateOfBirth;
			}
		}
	}
}
