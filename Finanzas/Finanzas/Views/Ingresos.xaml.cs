using Finanzas.Models;
using Finanzas.Services;
using Finanzas.ViewModels;
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
    public partial class Ingresos : ContentPage
    {
        public Ingresos()
        {
            InitializeComponent();
        }

        //Método que se manda a llamar cuando se presiona el botón de agregar
        private async void newEntry(object sender, EventArgs e)
        {
            //Validar que no estén vacíos
            if (cantidad.Text == "" ||  descripcion.Text == "")
            {
                await DisplayAlert("Aviso", "Debes llenar todos los campos", "OK");
                return;
            }
            //Verificar si es tipo de dato numérico
            try { 
                int.Parse(cantidad.Text);
            }
            catch(Exception) {
                await DisplayAlert("Advertencia", "EL tipo de dato debe ser numérico", "OK");
                cantidad.Text = "";
                return;
            }

            //FirebaseHelper = Clase donde está el código para enviar a firebase

            //Paso 1: Mandar a llamar a la clase Firebase Helper
            // Estas variables son del X:Name que están en los Entry/Editor del xaml (como si fuera un ID)
            //                                                             ↓              ↓
            var data = await FirebaseHelper.AgregarIngresos(int.Parse(cantidad.Text), descripcion.Text);
            //                                Convertir a entero ↑

            //validar que la operación se haya completado
            if (data == false)
            {
                await DisplayAlert("Error", "No se pudo guardar la información, intentelo de nuevo", "OK");
                return;
            }
            else
            {
                await DisplayAlert("Capturado", "Datos guardos exitosamente", "OK");
                //Cierra la pestaña una vez que termine
                await Navigation.PopAsync();
            }
 
        }
        private void backToHome(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }


    }
}