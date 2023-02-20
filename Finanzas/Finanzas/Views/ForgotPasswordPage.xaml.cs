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
    public partial class ForgotPasswordPage : ContentPage
    {
        ForgotPasswordVM aboutVM;
        public ForgotPasswordPage()
        {
            InitializeComponent();
            aboutVM = new ForgotPasswordVM();
            BindingContext = aboutVM;
        }
    }
}