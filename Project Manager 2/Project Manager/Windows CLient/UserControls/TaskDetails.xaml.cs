﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Windows_CLient.Models;
using Windows_CLient.ViewModels;
using Windows_CLient.Views;
using WCM = Windows_CLient.Models;

namespace Windows_CLient.UserControls
{
    /// <summary>
    /// Interaction logic for TaskDetails.xaml
    /// </summary>
    public partial class TaskDetails : UserControl
    {
        public TaskDetails()
        {
            InitializeComponent();
        }


        public WCM.Task Task
        {
            get { return (WCM.Task)GetValue(TaskProperty); }
            set { SetValue(TaskProperty, value); }
        }

        public static readonly DependencyProperty TaskProperty =
            DependencyProperty.Register("Task", typeof(WCM.Task), typeof(TaskDetails), new PropertyMetadata(new WCM.Task()));


        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object issueObject = ((ListView)sender).SelectedItem;
            if (issueObject is null) return;

            var issue = (Issue)issueObject;

            Views.FlagTask flag = new FlagTask();
            var context = new FlagTasksViewModel();
            
            context.Issue = issue;
            context.Self = flag;
            context.FlagMode = false;
            
            flag.DataContext = context;
            flag.ShowDialog();
        }
    }
}
