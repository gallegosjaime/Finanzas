﻿using Finanzas.Models;
using Finanzas.Services;
using Finanzas.ViewModels;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Finanzas.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Gastos : ContentPage
    {

        public Gastos()
        {
            InitializeComponent();
        }
        private void NewEntry(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }


        private void backToHome(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

    }
}