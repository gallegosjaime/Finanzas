using Finanzas.Models;
using Finanzas.Services;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Finanzas.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Historial : ContentPage
	{
		public Historial ()
		{
			InitializeComponent ();
            getData();
        }

        public async void getData()
        {
            var ingresos = await FirebaseHelper.getHistorialIngresos();
            ListViewName.ItemsSource = ingresos;
        }

        protected override void OnAppearing()
        {
            getData();
        }
        private void Editar(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            StackLayout listViewItem = (StackLayout)button.Parent;
            Label idRegistro = (Label)listViewItem.Children[1];
            Label qty = (Label)listViewItem.Children[2];
            Label desc = (Label)listViewItem.Children[3];
            Navigation.PushAsync(new EditarHistorial(idRegistro.Text, qty.Text, desc.Text));
        }
        private async void borrarRegistro(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            StackLayout listViewItem = (StackLayout)button.Parent;
            Label idRegistro = (Label)listViewItem.Children[1];
            await FirebaseHelper.deleteRegistro(Guid.Parse(idRegistro.Text));
            getData();
        }
    }
}