using Finanzas.Models;
using Finanzas.Services;
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
	public partial class EditarHistorial : ContentPage
	{
        private string id;
        private string nombreAccion;
        //Constructor
        public EditarHistorial(string id, string qty, string desc, string nombreAccion)
        {
            InitializeComponent(); 
            //Los datos recibidos se colocan en sus respectivos "inputs"
            this.nombreAccion = nombreAccion;
            cantidad.Text = qty;
            descripcion.Text = desc;
            this.id = id;
        }
        private void back(object sender, EventArgs e)
        {
            //Regresar a la pantalla anterior
            Navigation.PopAsync();
        }
        private async void update(object sender, EventArgs e)
        {
            //Mando a llamar la función para actualizar el registro y le paso los nuevos datos
            await FirebaseHelper.updateRegistro(id, int.Parse(cantidad.Text), descripcion.Text, nombreAccion);
            //cerrar pantalla
            await Navigation.PopAsync();
        }
    }
}