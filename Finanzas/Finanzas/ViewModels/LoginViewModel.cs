using Finanzas.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Finanzas.ViewModels
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }
        public string User { get; set; }
        public string Password { get; set; }


        public LoginViewModel()
        {
            LoginCommand = new Command(Login);
        }
        private async void Login(object obj)
        {
            //Validacion de campos vacios   

            if (string.IsNullOrEmpty(User.Trim()) || string.IsNullOrEmpty(Password))
                await App.Current.MainPage.DisplayAlert("Campos vacios", "Introduzca sus datos", "OK");
            else
            {
                //Llama la funcion GetUser de la clase Firebase helper
                var user = await FirebaseHelper.GetUser(User.Trim());
                //Firebase regresa el valor nulo si el usuario no se encontro en la BD
                if (user != null)
                    if (User.Trim() == user.Username && Password == user.Password)
                    {
                        await App.Current.MainPage.DisplayAlert("Login Exitoso", "Bienvenido "+User, "Ok");
                        //Navega la pagina principal en caso exitoso//
                        //pasa usuario a la pagina AboutPage// 

                        AboutPage.Username = User;
                        await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
                    }
                    else
                        await App.Current.MainPage.DisplayAlert("Error", "Por favor introducir sus datos de manera correcta", "OK");
                else
                    await App.Current.MainPage.DisplayAlert("Error", "Usuario no encontrado", "OK");


            }
        }

    }
}
