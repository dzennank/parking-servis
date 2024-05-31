using ParkingServis.Server.Controllers.UserControllers;
using ParkingServis.Server.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ParkingServis.Client.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly UserQueryController _userQueryController;
        private string _email;
        private string _password;
        private SecureString _securePassword;

        public LoginViewModel(UserQueryController userController)
        {
            _userQueryController = userController;
            LoginCommand = new RelayCommand(async () => await LoginAsync());
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public SecureString SecurePassword
        {
            private get => _securePassword;
            set
            {
                _securePassword = value;
                OnPropertyChanged(nameof(SecurePassword));
            }
        }

        public ICommand LoginCommand { get; }
        public ICommand OnMouseDownEmailCommand { get; }

        private async Task LoginAsync()
        {
            string password = ConvertSecureStringToString(SecurePassword);
            User user = await _userQueryController.GetUserByEmailPassword(Email, password);
            // Handle login success or failure
        }

        private void OnEmailMouseDown()
        {
            // Handle MouseDown event on the Email TextBox
            
        }
        private string ConvertSecureStringToString(SecureString secureString)
        {
            IntPtr ptr = Marshal.SecureStringToBSTR(secureString);
            try
            {
                return Marshal.PtrToStringBSTR(ptr);
            }
            finally
            {
                Marshal.ZeroFreeBSTR(ptr);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
