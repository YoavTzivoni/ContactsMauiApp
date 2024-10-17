using ContactsMauiApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ContactsMauiApp.ViewModels
{
	public class RegistrationViewModel : ViewModelBase
	{
		private IUserService _userService;
		public ICommand RegisterCommand { get; private set; }
		public ICommand ShowPasswordCommand { get; private set; }

		public RegistrationViewModel(IUserService userService)
		{
			_userService = userService;
			RegisterCommand = new Command(async () => 
			{
				if (editMode) await _userService.Create(email, password, displayName);
				else await _userService.Login(email, password);

				if (_userService.CurrentUser != null)  Shell.Current.GoToAsync(".."); 				
			});
			ShowPasswordCommand = new Command(() => { Shell.Current.DisplayAlert("test", "ShowPasswordCommand", "OK"); });
		}
		private bool editMode = false;

		public bool EditMode
		{
			get { return editMode; }
			set { editMode = value; OnPropertyChanged(); OnPropertyChanged(nameof(EditModeLabel)); }
		}
		public string EditModeLabel { get { return editMode ? "רישום" : "התחברות"; } }
		private string displayName = "";

		public string DisplayName
		{
			get { return displayName; }
			set { if (displayName != value) displayName = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsDisplayNameValid)); OnPropertyChanged(nameof(DisplayNameValidationIcon)); OnPropertyChanged(nameof(DisplayNameValidationIconColor)); }
		}
		public bool IsDisplayNameValid { get { return displayName.Length > 4; } }
		public string DisplayNameValidationIcon { get { return IsDisplayNameValid ? "\ue876" : "\ue645"; } }
		public string DisplayNameValidationIconColor { get { return IsDisplayNameValid ? "Green" : "Red"; } }

		private string email;

		public string Email
		{
			get { return email; }
			set { if (email != value) email = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsEmailValid)); OnPropertyChanged(nameof(EmailValidationIcon)); OnPropertyChanged(nameof(EmailValidationIconColor)); }
		}

		public bool IsEmailValid
		{
			get
			{
				string regex = @"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$";
				return !string.IsNullOrEmpty(email) && Regex.IsMatch(email, regex, RegexOptions.IgnoreCase);
			}
		}
		public string EmailValidationIcon { get { return IsEmailValid ? "\ue876" : "\ue645"; } }
		public string EmailValidationIconColor { get { return IsEmailValid ? "Green" : "Red"; } }
		private string password;

		public string Password
		{
			get { return password; }
			set { if (password != value) password = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsPasswordValid)); OnPropertyChanged(nameof(PasswordValidationIcon)); OnPropertyChanged(nameof(PasswordValidationIconColor)); }
		}

		public bool IsPasswordValid
		{
			get
			{
				Regex regex = new Regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)[a-zA-Z\\d]+$");
				return !string.IsNullOrEmpty(password) && regex.IsMatch(password);
			}
		}
		public string PasswordValidationIcon { get { return IsPasswordValid ? "\ue876" : "\ue645"; } }
		public string PasswordValidationIconColor { get { return IsPasswordValid ? "Green" : "Red"; } }



	}
}
