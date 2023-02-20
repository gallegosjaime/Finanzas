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
        private Guid id;
        public EditarHistorial (String id, String qty, String desc)
        {
            InitializeComponent ();
            cantidad.Text = qty;
            descripcion.Text = desc;
            this.id = Guid.Parse(id);
        }
        private void back(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
        private async void update(object sender, EventArgs e)
        {
            await FirebaseHelper.updateRegistro(id, int.Parse(cantidad.Text), descripcion.Text);
            await Navigation.PopAsync();
        }
    }
}