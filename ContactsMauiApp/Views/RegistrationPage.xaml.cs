using ContactsMauiApp.ViewModels;

namespace ContactsMauiApp.Views;

public partial class RegistrationPage : ContentPage
{
	public RegistrationPage(RegistrationViewModel registrationViewModel)
	{
		InitializeComponent();
		BindingContext = registrationViewModel;
	}

	protected override bool OnBackButtonPressed()	
	{
		return true;
		//return base.OnBackButtonPressed();
	}
}