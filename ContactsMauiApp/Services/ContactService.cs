using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
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
			//contacts.Add(new Model.Contact()
			//{				
			//	Name = "יואב צבעוני",
			//	Address = "חנקין 9 תל אביב",
			//	dateOfBirth = new DateTime(1974, 7, 20),
			//	Email = "yoav@gmail.com",
			//	Phone = "052-8998874"
			//});

			//contacts.Add(new Model.Contact()
			//{			
			//	Name = "נטע",
			//	Address = "חנקין 9 תל אביב",
			//	dateOfBirth = new DateTime(2020, 8, 10),
			//	Email = "neta@gmail.com",
			//	Phone = "052-6778812"
			//});
			LoadContacts();
		}

		public async Task<Model.Contact> GetContact(int id)
		{
			return contacts.Find(c => c.Id == id);
		}
		public async Task<bool> AddContact(Model.Contact contact)
		{
			contacts.Add(contact);
			SaveContact(contact);
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
			SaveContact(contact);
			return true;
		}
		public bool Delete(Model.Contact contact)
		{
			DeleteContact(contact);
			return contacts.Remove(contact);
		}

		public async Task<bool> UploadToyImage(FileResult photo, Model.Contact contact)
		{
			SaveImage(photo, contact.Id.ToString());
			return true;
		}


		private async void SaveImage(FileResult photo, string filename)
		{
			byte[] streamBytes;

			var memoryStream = new MemoryStream();
			var stream = await photo.OpenReadAsync();
			await stream.CopyToAsync(memoryStream);
			streamBytes = memoryStream.ToArray();




			//using var stream = await DrawView.GetImageStream(1024, 1024);
			//using var memoryStream = new MemoryStream();
			//stream.CopyTo(memoryStream);

			stream.Position = 0;
			memoryStream.Position = 0;
#if WINDOWS
		await System.IO.File.WriteAllBytesAsync(
			@"C:\Users\joverslu\Desktop\DrawingView\Test.png", memoryStream.ToArray());
#elif ANDROID
        var context = Platform.CurrentActivity;

        if (OperatingSystem.IsAndroidVersionAtLeast(29))
        {
            Android.Content.ContentResolver resolver = context.ContentResolver;
            Android.Content.ContentValues contentValues = new();
            contentValues.Put(Android.Provider.MediaStore.IMediaColumns.DisplayName, "test.png");
            contentValues.Put(Android.Provider.MediaStore.IMediaColumns.MimeType, "image/png");
            contentValues.Put(Android.Provider.MediaStore.IMediaColumns.RelativePath, "DCIM/" + "test");
            Android.Net.Uri imageUri = resolver.Insert(Android.Provider.MediaStore.Images.Media.ExternalContentUri, contentValues);
            var os = resolver.OpenOutputStream(imageUri);
            Android.Graphics.BitmapFactory.Options options = new();
            options.InJustDecodeBounds = true;
            var bitmap = Android.Graphics.BitmapFactory.DecodeStream(stream);
            bitmap.Compress(Android.Graphics.Bitmap.CompressFormat.Png, 100, os);
            os.Flush();
            os.Close();
        }
        else
        {
            Java.IO.File storagePath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures);
            string path = System.IO.Path.Combine(storagePath.ToString(), "test.png");
            System.IO.File.WriteAllBytes(path, memoryStream.ToArray());
            var mediaScanIntent = new Android.Content.Intent(Android.Content.Intent.ActionMediaScannerScanFile);
            mediaScanIntent.SetData(Android.Net.Uri.FromFile(new Java.IO.File(path)));
            context.SendBroadcast(mediaScanIntent);
        }
#elif IOS || MACCATALYST
			var image = new UIKit.UIImage(Foundation.NSData.FromArray(memoryStream.ToArray()));
			image.SaveToPhotosAlbum((image, error) =>
			{
			});
#endif
		}

		private void SaveContact(Model.Contact contact) 
		{
			JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true, WriteIndented = true };
			string json = JsonSerializer.Serialize(contact, options);
			Preferences.Set($"contact_{contact.Id}", json);
		}

		private void DeleteContact(Model.Contact contact)
		{
			if (Preferences.ContainsKey($"contact_{contact.Id}"))
			{
				Preferences.Remove($"contact_{contact.Id}");
			}
		}
		private void SaveContacts()
		{			
			JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true, WriteIndented = true };
			foreach (var contact in contacts)
			{
				string json = JsonSerializer.Serialize(contact, options);
				Preferences.Set($"contact_{contact.Id}", json);
			}
		}

		private void LoadContacts()
		{
			JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true, WriteIndented = true };
			contacts.Clear();
			int contactId = 1;
			while(Preferences.ContainsKey($"contact_{contactId}"))
			{
				string json = Preferences.Get($"contact_{contactId}", string.Empty);
				if (json != string.Empty)
				{
					Model.Contact c = JsonSerializer.Deserialize<Model.Contact>(json, options);
					contacts.Add(c);
				}
				contactId++;
			}
				
		}
	}
}
