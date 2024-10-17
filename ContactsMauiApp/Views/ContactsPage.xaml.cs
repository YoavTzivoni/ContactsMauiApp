using ContactsMauiApp.ViewModels;

namespace ContactsMauiApp.Views;

public partial class ContactsPage : ContentPage
{
	private readonly ContactsViewModel viewModel;
	public ContactsPage(ContactsViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
		viewModel = vm;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		viewModel.Refresh();
	}

	protected override void OnDisappearing()
	{
		DisplayAlert("test", "on disappearing", "OK");
		base.OnDisappearing();
	}
}