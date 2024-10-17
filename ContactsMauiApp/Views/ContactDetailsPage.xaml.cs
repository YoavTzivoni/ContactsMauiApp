using ContactsMauiApp.ViewModels;

namespace ContactsMauiApp.Views;

public partial class ContactDetailsPage : ContentPage
{
	public ContactDetailsPage(ContactDetailsViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}