using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Finanzas.Services;
using Finanzas.Views;
using Finanzas.ViewModels;
using Xamarin.Essentials;

namespace Finanzas.Views
{
    public partial class AboutPage : ContentPage
    {
        public static String Username;
        
        public AboutPage()
        {
            InitializeComponent();
            //DisplayAlert(Username, "", "Ok");
        }

        private void ingresos(object obj, EventArgs e)
        {
            Navigation.PushAsync(new Ingresos());
        }
        private void gastos(object obj, EventArgs e)
        {
            Navigation.PushAsync(new Gastos());
        }
    }
}