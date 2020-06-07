using System;
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

namespace Windows_CLient.Views.ManagerViews.Pages
{
    /// <summary>
    /// Interaction logic for TeamsPage.xaml
    /// </summary>
    public partial class TeamsPage : Page
    {
        public TeamsPage()
        {
            InitializeComponent();
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is null) return;
            if (e.NewValue.GetType() == typeof(Team))
                ((TeamsViewModel)DataContext).SelectedTeam = (Team)e.NewValue;
        }
    }
}
