using ContactsMauiApp.Views;

namespace ContactsMauiApp
{
	public partial class AppShell : Shell
	{
		public AppShell()
		{
			InitializeComponent();
			Routing.RegisterRoute(nameof(ContactDetailsPage), typeof(ContactDetailsPage));
			Routing.RegisterRoute(nameof(RegistrationPage), typeof(RegistrationPage));
		}
	}
}
