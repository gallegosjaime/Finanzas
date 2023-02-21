using Finanzas.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
using System.Text.RegularExpressions;
using System.Linq.Expressions;
using Firebase.Auth.Providers;
using Firebase.Auth;

namespace Finanzas.ViewModels
{
    public class SignUpVM : INotifyPropertyChanged
    {
        private string username;
        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Username"));
            }
        }

        private string email;
        public string Email
        {

            get { return email; }
            set
            {
                email = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Email"));
            }

        }



        private string password;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Password"));
            }
        }

        private string confirmpassword;
        public string ConfirmPassword
        {
            get { return confirmpassword; }
            set
            {
                confirmpassword = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ConfirmPassword"));
            }
        }
        public Command SignUpCommand
        {
            get
            {
                return new Command(() =>
                {
                    if (Password == ConfirmPassword)
                        SignUp();
                    else
                        App.Current.MainPage.DisplayAlert("", "Password must be same as above!", "OK");
                });
            }
        }
        private async void SignUp()
        {
            //null or empty field validation, check weather email and password is null or empty    

            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password) || string.IsNullOrWhiteSpace(Email))
                await App.Current.MainPage.DisplayAlert("Campos vacios", "Introduzca sus datos", "OK");
            else
            {
                String expresion;
                expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
                if (Regex.IsMatch(Email, expresion))
                {

                    try
                    {
                        //call AddUser function which we define in Firebase helper class    
                        var user = await FirebaseHelper.AddUser(Username.Trim(), Email, Password.Trim());

                        //AddUser return true if data insert successfuly     
                        if (user)
                        {
                            await App.Current.MainPage.DisplayAlert("Registro Exitoso", "", "Ok");
                            //Navigate to Wellcom page after successfuly SignUp    
                            //pass user email to welcom page    
                            await App.Current.MainPage.Navigation.PushAsync(new LoginPage());
                        }
                        else
                            await App.Current.MainPage.DisplayAlert("Error", "Fallo en el registro", "OK");
                    }
                    catch (Exception e)
                    {
                        await App.Current.MainPage.DisplayAlert("Error", "Fallo", "OK");
                    }

                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Email no", "Introduzca sus datos", "OK");
                }

            }
        }
    }
}
