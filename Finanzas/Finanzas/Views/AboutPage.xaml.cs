using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Finanzas.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
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