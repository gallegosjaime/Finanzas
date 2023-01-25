using Finanzas.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Finanzas.ViewModels
{
    public class RegViewModel : BaseViewModel
    {
        public Command RegCommand { get; }

        public RegViewModel()
        {
            RegCommand = new Command(OnRegClicked);
        }

        private async void OnRegClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}
