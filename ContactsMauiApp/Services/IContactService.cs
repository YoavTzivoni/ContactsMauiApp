namespace ContactsMauiApp.Services
{
	public interface IContactService
	{
		Task<List<Model.Contact>> GetContacts();
		Task<Model.Contact> GetContact(int id);
		Task<bool> AddContact(Model.Contact contact);
		bool Delete(Model.Contact contact);
		bool UpdateContact(Model.Contact contact);
		Task<bool> UploadToyImage(FileResult photo, Model.Contact contact);
	}
}