
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
		public ICommand SelectImageCommand { get; private set; }
		public ContactDetailsViewModel(IContactService contactService)
		{
			this.contactService = contactService;
			BackCommand = new Command(() => { updateContactFromProperties(); Shell.Current.GoToAsync(".."); });
			SelectImageCommand = new Command(async () => { await ChangeImage(); });
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
					editMode = true;
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
		private string selectedImage = "face.png";
		public string SelectedImage
		{
			get { return selectedImage; }
			set { if (selectedImage != value) {  selectedImage = value; OnPropertyChanged(); } }
		}
		private void updateProperties()
		{
			Name = Contact.Name;
			Phone = Contact.Phone;
			Email = Contact.Email;
			Address = Contact.Address;
			DateOfBirth = Contact.dateOfBirth;
			SelectedImage = Contact.ImgPath;
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
				Contact.ImgPath = SelectedImage;
			}
			else
			{
				Contact = new Model.Contact();
				Contact.Name = Name;
				Contact.Phone = Phone;
				Contact.Email = Email;
				Contact.Address = Address;
				Contact.dateOfBirth = DateOfBirth;
				Contact.ImgPath = SelectedImage;
				contactService.AddContact(Contact);
			}
		}
		
		private async Task ChangeImage()
		{
			var backup = SelectedImage;
			//SelectedImage = "loadingforever.gif";
			string choice = await Shell.Current.DisplayActionSheet("בחרו פעולה", cancel: "ביטול", null, "צלם", "בחר תמונה");

			FileResult photo = null;

			try
			{
				MediaPickerOptions options = new MediaPickerOptions() { Title = "חייך יפה למצלמה" };
				switch (choice)
				{
					case "צלם":
						if (MediaPicker.Default.IsCaptureSupported)
						{

							photo = await MediaPicker.Default.CapturePhotoAsync(options);
						}

						break;
					case "בחר תמונה":
						photo = await MediaPicker.Default.PickPhotoAsync(options);
						break;
					default:
						SelectedImage = backup;

						break;
				}
				//Add Method Upload Image
				if (photo != null)
				{
					bool success = await contactService.UploadToyImage(photo, Contact);
					if (success)
					{
						// save the file into local storage
						string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

						using Stream sourceStream = await photo.OpenReadAsync();
						using FileStream localFileStream = File.OpenWrite(localFilePath);

						await sourceStream.CopyToAsync(localFileStream);
						SelectedImage = localFilePath;
					}
					else SelectedImage = backup;
				}



			}
			catch (Exception ex) { }
		}
	}
}


