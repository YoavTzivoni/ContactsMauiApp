using ContactsMauiApp.Services;
using ContactsMauiApp.ViewModels;
using ContactsMauiApp.Views;
using Microsoft.Extensions.Logging;

namespace ContactsMauiApp
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();
			builder
				.UseMauiApp<App>()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
					fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
					fonts.AddFont("MaterialSymbolsRounded.ttf", "MaterialSymbolsRounded");
				});
			builder.Services.AddSingleton<IContactService, ContactService>();
			
			//builder.Services.AddSingleton<IUserService, UserService>();
			builder.Services.AddSingleton<IUserService, UserService>();

			builder.Services.AddTransient<ContactDetailsViewModel>();			
			builder.Services.AddSingleton<ContactsViewModel>();
			
			builder.Services.AddTransient<ContactDetailsPage>();
			builder.Services.AddTransient<ContactsPage>();

			builder.Services.AddTransient<RegistrationViewModel>();
			builder.Services.AddTransient<RegistrationPage>();
#if DEBUG
			builder.Logging.AddDebug();
#endif
			builder.Logging.Services.AddLogging();
			return builder.Build();
		}
	}
}
