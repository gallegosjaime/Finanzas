using Finanzas.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Finanzas.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}