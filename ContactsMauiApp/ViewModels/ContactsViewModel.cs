using ContactsMauiApp.Services;
using ContactsMauiApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ContactsMauiApp.ViewModels 
{
	public class ContactsViewModel : ViewModelBase
	{
		private IContactService contactsService;
		IUserService userService;
		public ObservableCollection<Model.Contact> Contacts { get; set; } = new();
		public ICommand DeleteCommand { get; private set; }
		public ICommand DetailsCommand { get; private set; }
		public ICommand AddCommand { get; private set; }
		private string filter = string.Empty;
		
		public ContactsViewModel(IContactService contactsService, IUserService userService)
		{
			this.contactsService = contactsService;
			this.userService = userService;

			DeleteCommand = new Command<Model.Contact>((c) => {  if (contactsService.Delete(c)) Refresh(); });
			DetailsCommand = new Command<Model.Contact>(async c => { await Shell.Current.GoToAsync("/ContactDetailsPage?id=" + c.Id); });
			AddCommand = new Command(async () => await Shell.Current.GoToAsync("/ContactDetailsPage"));
		
			if (userService.CurrentUser == null) Shell.Current.GoToAsync(nameof(RegistrationPage));
			//Refresh();
		}
		


		public void Refresh()
		{
			Contacts.Clear();
			foreach (var contact in contactsService.GetContacts().Result)
				Contacts.Add(contact);
		}
		public string Filter
		{
			get { return filter; } set { if (filter != value) { filter = value; SetFilter();} }
		}
		private void SetFilter()
		{
			var filterList = contactsService.GetContacts().Result.Where(c=>c.Name.ToLower().Contains(Filter) || c.Phone.Contains(filter)).ToList();
			Contacts.Clear();
			foreach (var contact in filterList)
				Contacts.Add(contact);

		}





		
	}
}
