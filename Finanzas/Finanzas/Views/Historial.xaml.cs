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
            //Obtener los registros de la colección de historial
            var ingresos = await FirebaseHelper.getHistorial();
            //Mostrarlos en el ListView
            ListViewName.ItemsSource = ingresos;
        }

        protected override void OnAppearing()
        {
            //Cada vez que se muestre la pantalla se actualiza
            getData();
        }
        private void Editar(object sender, EventArgs e)
        {
            //Se selecciona el botón seleccionado
            Button button = (Button)sender;
            //Se crea una variable de tipo stacklayout para obtener los hijos del mismo de donde se encuentra el botón
            StackLayout listViewItem = (StackLayout)button.Parent;
            //Se selecciona el hijo #2 el cual tiene el ID
            Label idRegistro = (Label)listViewItem.Children[1];
            //Se selecciona el hijo #2 el cual tiene la cantidad
            Label qty = (Label)listViewItem.Children[2];
            //Se selecciona el hijo #2 el cual tiene la descripción
            Label desc = (Label)listViewItem.Children[3];
            StackLayout fechaSubida = (StackLayout)listViewItem.Children[4];
            Label fechaSubidaLabel = (Label)fechaSubida.Children[1];
            //Se manda a llamar la pantalla de editar historial y paso los datos del registro
            Navigation.PushAsync(new EditarHistorial(idRegistro.Text, qty.Text, desc.Text));
        }
        private async void borrarRegistro(object sender, EventArgs e)
        {
            //Si responde que "si" se ejecutará el código
            var response = await DisplayAlert("¿Estas seguro de eliminarlo?","Eliminarlo influirá en presupuesto agregado","Si","No");
            if (response)
            {
                Button button = (Button)sender;
                StackLayout listViewItem = (StackLayout)button.Parent;
                //Nomas se ocupa el ID del registro
                Label idRegistro = (Label)listViewItem.Children[1];
                //Mando a llamar la funcion para borrar registro
                await FirebaseHelper.deleteRegistro(idRegistro.Text);
                //Una vez que se elimine, actualizo los registros
                getData();
            }
        }
    }
}