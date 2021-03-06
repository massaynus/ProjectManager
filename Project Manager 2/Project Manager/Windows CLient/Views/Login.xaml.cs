﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Windows_CLient.ViewModels;

namespace Windows_CLient.Views
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {

        public Login()
        {
            APIClient.initClient();
            InitializeComponent();

        }

        private void LoginWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var context = new LoginViewModel(this);
            DataContext = context;

            txtPassword.PasswordChanged += delegate { context.OnPropertyChanged(nameof(context.PasswordVisibility)); };
        }
    }
}
