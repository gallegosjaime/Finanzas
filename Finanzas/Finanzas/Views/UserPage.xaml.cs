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
    public partial class UserPage : ContentPage
    {
        UserVM userVM;
        public UserPage(string username)
        {
            //string username = UserVM.value;
            InitializeComponent();
            userVM = new UserVM(username);
            BindingContext = userVM;
        }
    }
}